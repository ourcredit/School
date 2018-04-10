using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using School.OperatorTrees.Dtos;
using School.Models;

namespace School.OperatorTrees
{
    /// <summary>
    /// OperatorTree应用层服务的接口方法
    /// </summary>
    public interface IOperatorTreeAppService : IApplicationService
    {
        /// <summary>
        /// 获取OperatorTree的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<OperatorTreeListDto>> GetPagedOperatorTrees(GetOperatorTreesInput input);

        /// <summary>
        /// 通过指定id获取OperatorTreeListDto信息
        /// </summary>
        Task<OperatorTreeListDto> GetOperatorTreeByIdAsync(EntityDto<int> input);

        /// <summary>
        /// 导出OperatorTree为excel表
        /// </summary>
        /// <returns></returns>
        //  Task<FileDto> GetOperatorTreesToExcel();
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetOperatorTreeForEditOutput> GetOperatorTreeForEdit(NullableIdDto<int> input);

        //todo:缺少Dto的生成GetOperatorTreeForEditOutput
        /// <summary>
        /// 添加或者修改OperatorTree的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateOperatorTree(CreateOrUpdateOperatorTreeInput input);

        /// <summary>
        /// 删除OperatorTree信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteOperatorTree(EntityDto<int> input);

        /// <summary>
        /// 批量删除OperatorTree
        /// </summary>
        Task BatchDeleteOperatorTreesAsync(List<int> input);
    }
}
