using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using School.Authorization;
using School.Models;
using School.Others.Dtos;
using School.Points;
using School.Points.DomainServices;
using School.Points.Dtos;

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
        private readonly IRepository<OperatorDevice> _odeviceRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OtherAppService(ICacheManager cacheManager, IRepository<OperatorDevice> odeviceRepository)
        {
            _cacheManager = cacheManager;
            _odeviceRepository = odeviceRepository;
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<dsc_Goods>> GetPagedGoods(GetGoodsInput input)
        {
            var ht = await _odeviceRepository.GetAllIncluding(c => c.DeviceGoodses)
                .FirstOrDefaultAsync(c => c.Id == input.DeviceId);

            var result = await _cacheManager.GetCache(SchoolCache.GoodsCache)
                .GetAsync(SchoolCache.GoodsCache, GetGoodsFromCache);
            var count = result.Items.Count;
            var temp = result.Items.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                c => c.goods_name.Contains(input.Filter));
            var list = temp.OrderBy(c => c.goods_name).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            if (ht == null || !ht.DeviceGoodses.Any()) return new PagedResultDto<dsc_Goods>(count, list);

            var arr = ht.DeviceGoodses.ToList();
            foreach (var goodse in list)
            {
                var mo = arr.FirstOrDefault(c => c.GoodsId == goodse.goods_id);
                if (mo != null)
                {
                    goodse.IsSeal = true;
                    goodse.Price = mo.Price;
                }
            }
            return new PagedResultDto<dsc_Goods>(count, list);
        }

        private async Task<ListResultDto<dsc_Goods>> GetGoodsFromCache()
        {
            var res = await DapperHelper.GetResult<dsc_Goods>();
            return res;
        }
    }
}

