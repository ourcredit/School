using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Caching;
using Abp.UI;
using Abp.UI.Inputs;
using Microsoft.EntityFrameworkCore;
using School.Models;
using School.Others.Dtos;

namespace School.Others
{
    /// <summary>
    /// Point应用层服务的接口实现方法
    /// </summary>
    public class OtherAppService : SchoolAppServiceBase, IOtherAppService
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Orders> _orderRepository;
        private readonly IRepository<Device> _deviceRepository;
        private readonly IRepository<Channel> _channelRepository;
        private readonly IRepository<Show> _showRepository;
        private readonly IRepository<ChannelShow> _channelShowRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OtherAppService(ICacheManager cacheManager,
            IRepository<Orders> orderRepository, IRepository<Device> deviceRepository, IRepository<Show> showRepository, IRepository<Channel> channelRepository, IRepository<ChannelShow> channelShowRepository)
        {
            _cacheManager = cacheManager;
            _orderRepository = orderRepository;
            _deviceRepository = deviceRepository;
            _showRepository = showRepository;
            _channelRepository = channelRepository;
            _channelShowRepository = channelShowRepository;
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<dsc_Goods>> GetPagedGoods(GetGoodsInput input)
        {
            var ht = await _deviceRepository.GetAllIncluding(c => c.DeviceGoods)
                .FirstOrDefaultAsync(c => c.Id == input.DeviceId);

            var result = await _cacheManager.GetCache(SchoolCache.GoodsCache)
                .GetAsync(SchoolCache.GoodsCache, GetGoodsFromCache);
            var count = result.Items.Count;
            var temp = result.Items.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                c => c.goods_name.Contains(input.Filter));
            var list = temp.OrderBy(c => c.goods_name).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var arr = ht == null || !ht.DeviceGoods.Any() ? new List<DeviceGood>() : ht.DeviceGoods.ToList();
            foreach (var goodse in list)
            {
                var mo = arr.FirstOrDefault(c => c.GoodsId == goodse.goods_id);
                if (mo != null)
                {
                    goodse.IsSeal = true;
                    goodse.Price = mo.Price;
                }
                else
                {
                    goodse.IsSeal = false;
                    goodse.Price = 0;
                }
            }
            return new PagedResultDto<dsc_Goods>(count, list);
        }

        /// <summary>
        /// 获取机构下机器内商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceGoodsListDto>> GetProducts(GetDeviceGoodsInput input)
        {
            var device = await _deviceRepository.GetAllIncluding(c => c.DeviceGoods)
                .FirstOrDefaultAsync(c => c.DeviceNum.Equals(input.MachineCode));
            if (device == null || device.DeviceGoods == null || !device.DeviceGoods.Any())
                return new PagedResultDto<DeviceGoodsListDto>(0, new List<DeviceGoodsListDto>());

            var count = device.DeviceGoods.Count;
            var temp = device.DeviceGoods.OrderBy(c => c.CreationTime).Skip(input.SkipCount)
                .Take(input.MaxResultCount).ToList();
            var result = temp.MapTo<List<DeviceGoodsListDto>>();
            return new PagedResultDto<DeviceGoodsListDto>(count, result);
        }
        /// <summary>
        /// 心跳程序 检查设备状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task Beat(DeviceStateInput input)
        {
            var device = await _deviceRepository.FirstOrDefaultAsync(c => c.DeviceNum == input.MachineCode);
            if (device == null) throw new UserFriendlyException("该设备不存在");
            device.State = input.State;
        }
        /// <summary>
        /// 出货上报
        /// </summary>
        /// <returns></returns>
        public async Task Order(DealOrderInput input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(c => c.OrderNum == input.OrderId);
            if (order == null) throw new UserFriendlyException("该订单不存在");
            if (input.OrderStatus.Equals("4"))
            {
                order.DeliveryTime = DateTime.Now;
            }
            order.Status = input.OrderStatus;

        }
        /// <summary>
        /// Vmc系统状态报告
        /// </summary>
        /// <returns></returns>
        public async Task StatusReport(StateReportInput input)
        {
            var channel = await _channelRepository.FirstOrDefaultAsync(c =>
                c.Machine_Code == input.MachineCode && c.Site == input.BillStatus);
            channel.State = input.Cointype0Lack;
        }
        /// <summary>
        /// 余量调整
        /// </summary>
        /// <returns></returns>
        public async Task ChannelStockReport(ChannelStockReportInput input)
        {
            var channel = await _channelRepository.FirstOrDefaultAsync(c =>
                c.Machine_Code == input.MachineCode && c.Site == input.BillStatus);
            channel.Quantity = input.Quantity;
        }
        /// <summary>
        /// 状态调整
        /// </summary>
        /// <returns></returns>
        public async Task ChannelStatusReport(StateReportInput input)
        {
            var channel = await _channelRepository.FirstOrDefaultAsync(c =>
                c.Machine_Code == input.MachineCode && c.Site == input.BillStatus);
            channel.State = input.Cointype0Lack;
        }
        /// <summary>
        /// 取货码验证
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckPickCode(CheckPickCodeInput input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(c => c.Merchant_Name == input.MachineCode);
            if(order==null) throw new UserFriendlyException("该订单不存在");
            if(order.Status.Equals("0")) throw new UserFriendlyException("该订单未支付");
            if(order.Status.Equals("4")) throw new UserFriendlyException("该订单已出货");
            if(!order.PickupCode.Equals(input.PickCode)) throw new UserFriendlyException("提货码错误");
            return true;
        }
        /// <summary>
        /// 更新货道
        /// </summary>
        /// <returns></returns>
        public async Task UpGxvmChannel(CreateOrUpdateChannelInput input)
        {
            await _channelRepository.DeleteAsync(c => true);
            foreach (var c in input.Gx_vm_channel)
            {
                await _channelRepository.InsertAsync(new Channel()
                {
                    Goods_Id = c.Goods_id,
                    Isdelete = false,
                    Machine_Code = c.MachineCode,
                    Quantity = c.Quantity,
                    QuantityLine = c.QuantityLine,
                    Site = c.Site,
                    State = c.State
                });
            }
        }
        /// <summary>
        /// 更新货柜机展示
        /// </summary>
        /// <returns></returns>
        public async Task UpGxvmShow(CreateOrUpdateShowInput input)
        {
            await _showRepository.DeleteAsync(c => true);
            foreach (var c in input.Gx_vm_Show)
            {
                await _showRepository.InsertAsync(new Show()
                {
                    Machine_Code = c.MachineCode,
                    Goods_id = c.Goods_id,
                    Site = c.Site
                });
            }
        }
        /// <summary>
        /// 更新货柜机展示
        /// </summary>
        /// <returns></returns>
        public async Task UpGxvmShow2Channel(CreateOrUpdateShowChannelInput input)
        {
            await _channelShowRepository.DeleteAsync(c => true);
            foreach (var c in input.Gx_vm_Show2Channel)
            {
                await _channelShowRepository.InsertAsync(new ChannelShow()
                {
                    Machine_Code = c.MachineCode,
                    ChannelSite = c.ChannelSite,
                    ShowSite = c.ShowSite
                });
            }
        }
        private async Task<ListResultDto<dsc_Goods>> GetGoodsFromCache()
        {
            var current =await AbpSession.CurrentAsync();
            if(!current.KeyId.HasValue)throw new UserFriendlyException("当前用户不是超管用户");
            var res = await DapperHelper.GetResult<dsc_Goods>($" where user_id={current.KeyId} ");
            return res;
        }
    }
}

