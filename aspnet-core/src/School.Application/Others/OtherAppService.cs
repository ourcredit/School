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
        private readonly IRepository<OperatorTree> _treeRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OtherAppService(ICacheManager cacheManager,
            IRepository<Orders> orderRepository, IRepository<Device> deviceRepository, IRepository<Show> showRepository, IRepository<Channel> channelRepository, IRepository<ChannelShow> channelShowRepository, IRepository<OperatorTree> treeRepository)
        {
            _cacheManager = cacheManager;
            _orderRepository = orderRepository;
            _deviceRepository = deviceRepository;
            _showRepository = showRepository;
            _channelRepository = channelRepository;
            _channelShowRepository = channelShowRepository;
            _treeRepository = treeRepository;
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<dsc_Goods>> GetPagedGoods(GetGoodsInput input)
        {
            var current =await AbpSession.CurrentAsync();
            var otre = await _treeRepository.FirstOrDefaultAsync(c => c.TreeCode.Equals(current.TreeCode));
            var ht = await _deviceRepository.GetAllIncluding(c => c.DeviceGoods)
                .FirstOrDefaultAsync(c => c.Id == input.DeviceId);
          
            var result = await _cacheManager.GetCache(SchoolCache.GoodsCache)
                .GetAsync(SchoolCache.GoodsCache, GetGoodsFromCache);
            var temp = result.Items
                .Where(c=>c.user_id==otre.ShopId)
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), c => c.goods_name.Contains(input.Name))
                .WhereIf(!input.Sn.IsNullOrWhiteSpace(), c => c.goods_sn.Contains(input.Sn))
                .WhereIf(!input.Cate.IsNullOrWhiteSpace(), c => c.cat_name.Contains(input.Cate));
            var arr = ht == null || !ht.DeviceGoods.Any() ? new List<DeviceGood>() : ht.DeviceGoods.ToList();
            var t = from c in temp
                    join d in arr on c.goods_id equals d.GoodsId into h
                    from tt in h.DefaultIfEmpty()
                    select new { c, tt };
            t = t.WhereIf(input.IsSeal.HasValue && input.IsSeal.Value, c => c.tt != null)
                .WhereIf(input.IsSeal.HasValue && !input.IsSeal.Value, c => c.tt == null);
            var count = t.Count();

            var list = t.OrderBy(c => c.c.goods_name).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var res = new List<dsc_Goods>();
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
        public async Task<PagedResultDto<ProductListDto>> Products(GetProductsInput input)
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

            var list = ht.DeviceGoods;
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
                    model.img_url = "http://image.ishenran.cn/" + mo.goods_img;
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
        /// 更新售货机工控编号接口
        /// </summary>
        /// <returns></returns>
        public async Task UpdateDeviceControlCode(MachineCodeInput input)
        {
            var device = await _deviceRepository.FirstOrDefaultAsync(c => c.DeviceNum == input.Machine_Code);
            device.ControlNum = input.Control_Code;
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
            device.SaleStatus = input.status_code;
        }
        /// <summary>
        /// 线上出货上报
        /// </summary>
        /// <returns></returns>
        public async Task OnlineOrder(DealOrderInput input)
        {
            var orders = await _orderRepository.GetAllListAsync(c => input.orders.Any(w => w.orderId == c.order_id));
            foreach (var order in orders)
            {
                var state = input.orders.FirstOrDefault(c => c.orderId == order.order_id);
                order.status = state.orderStatus;
            }

        }
        /// <summary>
        /// 现金出货上报
        /// </summary>
        /// <returns></returns>
        public async Task CachOrder(CashOrderInput input)
        {
            foreach (var order in input.orders)
            {
                var model = new Orders()
                {
                    created_time = DateTime.Now,
                    delivery_time = order.venDoutDate,
                    goods_id = order.productId,
                    order_id = Guid.NewGuid().ToString("N"),
                    pay_price = order.price,
                    status = order.orderStatus,
                    vmid = input.MachineCode,pay_time=DateTime.Now
                };
                await _orderRepository.InsertAsync(model);
            }
        }
        /// <summary>
        /// Vmc系统状态报告
        /// </summary>
        /// <returns></returns>
        public async Task StatusReport(StateReportInput input)
        {
            var device = await _deviceRepository.FirstOrDefaultAsync(c =>
                c.DeviceNum == input.MachineCode);
            if (device == null) throw new UserFriendlyException("该设备不存在");
            device.SaleStatus = input.SaleStatus;
            device.WorkPattern = input.WorkPattern;
            device.DoorSw = input.DoorSw;
            device.CoinConnection = input.CoinConnection;
            device.BillConnection = input.BillConnection;
            device.Cointype0Lack = input.Cointype0Lack;
            device.Cointype1Lack = input.Cointype1Lack;
            device.BillStatus = input.BillStatus;
        }
        /// <summary>
        /// 余量调整
        /// </summary>
        /// <returns></returns>
        public async Task ChannelStockReport(ChannelStockReportInput input)
        {
            var channels = await _channelRepository.GetAllListAsync(c =>
                c.Machine_Code == input.MachineCode);
            foreach (var statu in input.Status)
            {
                var model = channels.FirstOrDefault(c => c.Site == statu.column);
                if (model != null)
                {
                    model.Quantity = statu.quantity;
                }
            }
        }
        /// <summary>
        /// 状态调整
        /// </summary>
        /// <returns></returns>
        public async Task ChannelStatusReport(ChannelStatusReportInput input)
        {
            var channels = await _channelRepository.GetAllListAsync(c =>
                c.Machine_Code == input.MachineCode);
            foreach (var statu in input.Status)
            {
                var model = channels.FirstOrDefault(c => c.Site == statu.column);
                if (model != null)
                {
                    model.State = statu.state;
                }
            }
        }
        /// <summary>
        /// 取货码验证
        /// </summary>
        /// <returns></returns>
        public async Task<CheckPickCodeResult> CheckPickCode(CheckPickCodeInput input)
        {
            var result = new CheckPickCodeResult();
            var orders = await _orderRepository.GetAllListAsync(c => c.vmid == input.MachineCode);
            if (orders == null || !orders.Any()) result.IsTrue = false;
            var order = orders.FirstOrDefault(c => c.pickup_code == input.PickCode);
            if (order == null) result.IsTrue = false;
            if (!order.status.IsNullOrWhiteSpace()||order.status.Equals("10004")) result.IsTrue = false;
            if (!order.pickup_code.Equals(input.PickCode)) result.IsTrue = false;
            result.IsTrue = true;
            result.ProductId = order.goods_id;
            return result;
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
        //private async Task<ListResultDto<dsc_Goods>> GetGoodsFromCacheByAuth()
        //{
        //    var current = await AbpSession.CurrentAsync();
        //    if (!current.KeyId.HasValue) throw new UserFriendlyException("当前用户不是超管用户");
        //    if (!current.ShopId.HasValue) throw new UserFriendlyException("未找到丛属店铺信息");
        //    var res = await DapperHelper.GetGoodsResult<dsc_Goods>($" where a.user_id={current.ShopId.Value} ");
        //    return res;
        //}
        private async Task<ListResultDto<dsc_Goods>> GetGoodsFromCache()
        {
            var res = await DapperHelper.GetGoodsResult<dsc_Goods>();
            return res;
        }
    }
}

