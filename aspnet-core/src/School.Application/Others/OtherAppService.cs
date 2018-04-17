using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
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
                .GetAsync(SchoolCache.GoodsCache, GetGoodsFromCacheByAuth);
            var temp = result.Items.WhereIf(!input.Name.IsNullOrWhiteSpace(), c => c.goods_name.Contains(input.Name))
                .WhereIf(!input.Sn.IsNullOrWhiteSpace(), c => c.goods_sn.Contains(input.Sn))
                .WhereIf(!input.Cate.IsNullOrWhiteSpace(), c => c.cat_name.Contains(input.Cate));
            var arr = ht == null || !ht.DeviceGoods.Any() ? new List<DeviceGood>() : ht.DeviceGoods.ToList();
            var t = from c in temp
                join d in arr on c.goods_id equals d.GoodsId into h
                from tt in h.DefaultIfEmpty()
                select new {c, tt};
            t = t.WhereIf(input.IsSeal.HasValue && input.IsSeal.Value, c => c.tt != null)
                .WhereIf(input.IsSeal.HasValue && !input.IsSeal.Value, c => c.tt == null);
            var count =t.Count();
          
            var list = t.OrderBy(c => c.c.goods_name).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var res=new List<dsc_Goods>();
            foreach (var goodse in list)
            {
                var te = goodse.c;
                te.IsSeal = goodse.tt != null;
                te.Price = goodse.tt != null ? goodse.tt.Price : 0;
                res.Add(te);
            }
            return new PagedResultDto<dsc_Goods>(count, res);
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductListDto>> GetProducts(GetDeviceGoodsInput input)
        {
            var ht = await _deviceRepository.GetAllIncluding(c => c.DeviceGoods)
                .FirstOrDefaultAsync(c => c.DeviceNum == input.MachineCode);
            if (ht == null || ht.DeviceGoods == null || !ht.DeviceGoods.Any())
            {
                return new PagedResultDto<ProductListDto>();
            }
            var temp = await _cacheManager.GetCache(SchoolCache.GoodsCache)
                .GetAsync(SchoolCache.GoodsCache, GetGoodsFromCache);
            var count = ht.DeviceGoods.Count;

            var list = ht.DeviceGoods.OrderBy(c => c.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new List<ProductListDto>();
            foreach (var g in list)
            {
                var model = new ProductListDto()
                {
                    price = g.Price,
                    product_id = g.GoodsId,
                    product_name = g.GoodsName
                };
                var mo = temp.Items.FirstOrDefault(c => c.goods_id == g.GoodsId);
                if (mo != null)
                {
                    model.img_url = "http://image.ishenran.cn/"+ mo.goods_img;
                }
               result.Add(model);
            }
            return new PagedResultDto<ProductListDto>(count, result);
        }

        /// <summary>
        /// 获取货道列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ChannelListDto>> GetPagedChannels(GetDeviceGoodsInput input)
        {
            var hts = _channelRepository.GetAll();
            hts = hts.WhereIf(!input.MachineCode.IsNullOrWhiteSpace(), c => c.Machine_Code.Equals(input.MachineCode));
            var deviceCount = await hts.CountAsync();
            var devices = await hts
                .OrderByDescending(c => c.CreateTime)
                .PageBy(input)
                .ToListAsync();

            var result = await _cacheManager.GetCache(SchoolCache.GoodsCache)
                .GetAsync(SchoolCache.GoodsCache, GetGoodsFromCache);

            var tr = new List<ChannelListDto>();
            foreach (var goodse in devices)
            {
                var model = goodse.MapTo<ChannelListDto>();
                var mo = result.Items.FirstOrDefault(c => c.goods_id == goodse.Goods_Id);
                if (mo != null)
                {
                    model.Goods_Name = mo.goods_name;
                }
                tr.Add(model);
            }
            return new PagedResultDto<ChannelListDto>(deviceCount, tr);
        }
        /// <summary>
        /// 获取展示位列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ShowListDto>> GetPagedBoxs(GetDeviceGoodsInput input)
        {
            var hts = _showRepository.GetAll();
            hts = hts.WhereIf(!input.MachineCode.IsNullOrWhiteSpace(), c => c.Machine_Code.Equals(input.MachineCode));
            var deviceCount = await hts.CountAsync();
            var devices = await hts
                .OrderByDescending(c => c.CreateTime)
                .PageBy(input)
                .ToListAsync();

            var result = await _cacheManager.GetCache(SchoolCache.GoodsCache)
                .GetAsync(SchoolCache.GoodsCache, GetGoodsFromCache);

            var tr = new List<ShowListDto>();
            foreach (var goodse in devices)
            {
                var model = goodse.MapTo<ShowListDto>();
                var mo = result.Items.FirstOrDefault(c => c.goods_id == goodse.Goods_id);
                if (mo != null)
                {
                    model.Goods_Name = mo.goods_name;
                }
                tr.Add(model);
            }
            return new PagedResultDto<ShowListDto>(deviceCount, tr);
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
            if (order == null) throw new UserFriendlyException("该订单不存在");
            if (order.Status.Equals("0")) throw new UserFriendlyException("该订单未支付");
            if (order.Status.Equals("4")) throw new UserFriendlyException("该订单已出货");
            if (!order.PickupCode.Equals(input.PickCode)) throw new UserFriendlyException("提货码错误");
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
                    State = c.State,
                    CreateTime = DateTime.Now
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
                    Site = c.Site,
                    CreateTime = DateTime.Now

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
                    ShowSite = c.ShowSite,
                    CreateTime = DateTime.Now
                });
            }
        }
        private async Task<ListResultDto<dsc_Goods>> GetGoodsFromCacheByAuth()
        {
            var current = await AbpSession.CurrentAsync();
            if (!current.KeyId.HasValue) throw new UserFriendlyException("当前用户不是超管用户");
            var res = await DapperHelper.GetGoodsResult<dsc_Goods>($" where a.user_id={current.KeyId} ");
            return res;
        }
        private async Task<ListResultDto<dsc_Goods>> GetGoodsFromCache()
        {
            var res = await DapperHelper.GetGoodsResult<dsc_Goods>();
            return res;
        }
    }
}

