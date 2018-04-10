using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using School.Authorization;
using School.OperatorTrees.Dtos;
using School.OperatorTrees.DomainServices;
using School.Models;

namespace School.OperatorTrees
{
    /// <summary>
    /// OperatorTree应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages_Operator_Orgs)]
    public class OperatorTreeAppService : SchoolAppServiceBase, IOperatorTreeAppService
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<OperatorTree, int> _operatortreeRepository;
        private readonly IOperatorTreeManager _operatortreeManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OperatorTreeAppService(IRepository<OperatorTree, int> operatortreeRepository
      , IOperatorTreeManager operatortreeManager
        )
        {
            _operatortreeRepository = operatortreeRepository;
            _operatortreeManager = operatortreeManager;
        }

        /// <summary>
        /// 获取OperatorTree的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<OperatorTreeListDto>> GetPagedOperatorTrees(GetOperatorTreesInput input)
        {

            var query = _operatortreeRepository.GetAll();
            //TODO:根据传入的参数添加过滤条件
            var operatortreeCount = await query.CountAsync();

            var operatortrees = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var operatortreeListDtos = ObjectMapper.Map<List <OperatorTreeListDto>>(operatortrees);
            var operatortreeListDtos = operatortrees.MapTo<List<OperatorTreeListDto>>();

            return new PagedResultDto<OperatorTreeListDto>(
                operatortreeCount,
                operatortreeListDtos
                );

        }

        /// <summary>
        /// 通过指定id获取OperatorTreeListDto信息
        /// </summary>
        public async Task<OperatorTreeListDto> GetOperatorTreeByIdAsync(EntityDto<int> input)
        {
            var entity = await _operatortreeRepository.GetAsync(input.Id);

            return entity.MapTo<OperatorTreeListDto>();
        }

        /// <summary>
        /// 导出OperatorTree为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetOperatorTreesToExcel(){
        //var users = await UserManager.Users.ToListAsync();
        //var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //await FillRoleNames(userListDtos);
        //return _userListExcelExporter.ExportToFile(userListDtos);
        //}
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetOperatorTreeForEditOutput> GetOperatorTreeForEdit(NullableIdDto<int> input)
        {
            var output = new GetOperatorTreeForEditOutput();
            OperatorTreeEditDto operatortreeEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _operatortreeRepository.GetAsync(input.Id.Value);

                operatortreeEditDto = entity.MapTo<OperatorTreeEditDto>();

                //operatortreeEditDto = ObjectMapper.Map<List <operatortreeEditDto>>(entity);
            }
            else
            {
                operatortreeEditDto = new OperatorTreeEditDto();
            }

            output.OperatorTree = operatortreeEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改OperatorTree的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateOperatorTree(CreateOrUpdateOperatorTreeInput input)
        {

            if (input.OperatorTree.Id.HasValue)
            {
                await UpdateOperatorTreeAsync(input.OperatorTree);
            }
            else
            {
                await CreateOperatorTreeAsync(input.OperatorTree);
            }
        }

        /// <summary>
        /// 新增OperatorTree
        /// </summary>
        [AbpAuthorize(AppPermissions.Pages_Operator_Orgs_Create)]
        protected virtual async Task<OperatorTreeEditDto> CreateOperatorTreeAsync(OperatorTreeEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<OperatorTree>(input);

            entity = await _operatortreeRepository.InsertAsync(entity);
            return entity.MapTo<OperatorTreeEditDto>();
        }

        /// <summary>
        /// 编辑OperatorTree
        /// </summary>
        [AbpAuthorize(AppPermissions.Pages_Operator_Orgs_Edit)]
        protected virtual async Task UpdateOperatorTreeAsync(OperatorTreeEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _operatortreeRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _operatortreeRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除OperatorTree信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Operator_Orgs_Delete)]
        public async Task DeleteOperatorTree(EntityDto<int> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _operatortreeRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除OperatorTree的方法
        /// </summary>
        [AbpAuthorize(AppPermissions.Pages_Operator_Orgs_Delete)]
        public async Task BatchDeleteOperatorTreesAsync(List<int> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _operatortreeRepository.DeleteAsync(s => input.Contains(s.Id));
        }

    }
}

