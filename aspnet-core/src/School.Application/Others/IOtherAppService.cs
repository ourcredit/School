using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using School.Models;
using School.Others.Dtos;
using School.Points.Dtos;

namespace School.Others
{
    /// <summary>
    /// Point应用层服务的接口方法
    /// </summary>
    public interface IOtherAppService : IApplicationService
    {
        /// <summary>
        /// 获取Point的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<dsc_Goods>> GetPagedGoods(GetGoodsInput input);
       
    }
}
