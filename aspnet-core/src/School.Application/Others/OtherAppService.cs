using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Caching;
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

        /// <summary>
        /// 构造函数
        /// </summary>
        public OtherAppService(ICacheManager cacheManager,
            IRepository<Orders> orderRepository, IRepository<Device> deviceRepository)
        {
            _cacheManager = cacheManager;
            _orderRepository = orderRepository;
            _deviceRepository = deviceRepository;
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

            var arr = ht == null || !ht.DeviceGoods.Any()?new List<DeviceGood>() : ht.DeviceGoods.ToList();
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
        /// 出货上报
        /// </summary>
        /// <returns></returns>
        public async Task Order(DealOrderInput input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(c => c.OrderNum == input.OrderId);
        }


        private async Task<ListResultDto<dsc_Goods>> GetGoodsFromCache()
        {
            var res = await DapperHelper.GetResult<dsc_Goods>();
            return res;
        }
    }
}

