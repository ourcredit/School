using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using School.Points.Dtos;
using School.Models;

namespace School.Points
{
    /// <summary>
    /// Point应用层服务的接口方法
    /// </summary>
    public interface IPointAppService : IApplicationService
    {
        /// <summary>
        /// 获取Point的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<PointListDto>> GetPagedPoints(GetPointsInput input);

        /// <summary>
        /// 通过指定id获取PointListDto信息
        /// </summary>
        Task<PointListDto> GetPointByIdAsync(EntityDto<int> input);

        /// <summary>
        /// 导出Point为excel表
        /// </summary>
        /// <returns></returns>
        //  Task<FileDto> GetPointsToExcel();
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetPointForEditOutput> GetPointForEdit(NullableIdDto<int> input);

        //todo:缺少Dto的生成GetPointForEditOutput
        /// <summary>
        /// 添加或者修改Point的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdatePoint(CreateOrUpdatePointInput input);

        /// <summary>
        /// 删除Point信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeletePoint(EntityDto<int> input);

        /// <summary>
        /// 批量删除Point
        /// </summary>
        Task BatchDeletePointsAsync(List<int> input);
    }
}
