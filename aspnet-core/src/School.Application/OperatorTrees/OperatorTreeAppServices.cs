using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using System.Linq.Dynamic.Core;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using School.Authorization;
using School.Dto;
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
        public async Task<ListResultDto<OperatorTreeListDto>> GetOperatorTrees(FilterInputDto input)
        {
            var current =await AbpSession.CurrentAsync();
            var query = _operatortreeRepository.GetAll();
            query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(), c => c.TreeCode.Contains(input.Filter))
                .WhereIf(!current.IsAdmin && !current.TreeCode.IsNullOrWhiteSpace(),
                    c => c.TreeCode.Contains(current.TreeCode));
            var operatortrees = await query
                .ToListAsync();

            //var operatortreeListDtos = ObjectMapper.Map<List <OperatorTreeListDto>>(operatortrees);
            var operatortreeListDtos = operatortrees.MapTo<List<OperatorTreeListDto>>();

            return new ListResultDto<OperatorTreeListDto>(
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
            var entity = ObjectMapper.Map<OperatorTree>(input);
            if (input.ParentId.HasValue)
            {
                var parent = await _operatortreeRepository.FirstOrDefaultAsync(input.ParentId.Value);
                if (parent != null)
                {
                    entity.TreeCode = GenderCode(parent.TreeCode);
                    entity.TreeLength = parent.TreeLength + 1;
                }
            }
            else
            {
                entity.TreeCode = GenderCode("");
                entity.TreeLength = 1;
            }
            entity = await _operatortreeRepository.InsertAsync(entity);
            return entity.MapTo<OperatorTreeEditDto>();
        }

        private string GenderCode(string parent)
        {
            var code = Guid.NewGuid().ToString("D").Split('-').Last();
            return parent.IsNullOrWhiteSpace() ? code : $"{parent}.{code}";
        }
        /// <summary>
        /// 编辑OperatorTree
        /// </summary>
        [AbpAuthorize(AppPermissions.Pages_Operator_Orgs_Edit)]
        protected virtual async Task UpdateOperatorTreeAsync(OperatorTreeEditDto input)
        {
           
            var entity = await _operatortreeRepository.GetAsync(input.Id.Value);
            if (!entity.ParentId.HasValue || !input.ParentId.HasValue)
            {
                input.MapTo(entity);
            }
            else if(entity.ParentId.Value!=input.ParentId.Value)
            {
                input.MapTo(entity);
                var parent = await _operatortreeRepository.FirstOrDefaultAsync(input.ParentId.Value);
                if (parent != null)
                {
                    entity.TreeCode = GenderCode(parent.TreeCode);
                    entity.TreeLength = parent.TreeLength + 1;
                }
            }

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

