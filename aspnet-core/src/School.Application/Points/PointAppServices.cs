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
using School.Points.Dtos;
using School.Points.DomainServices;
using School.Models;

namespace School.Points
{
    /// <summary>
    /// Point应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages_Point_Manage)]
    public class PointAppService : SchoolAppServiceBase, IPointAppService
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<Point, int> _pointRepository;
        private readonly IPointManager _pointManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PointAppService(IRepository<Point, int> pointRepository
      , IPointManager pointManager
        )
        {
            _pointRepository = pointRepository;
            _pointManager = pointManager;
        }

        /// <summary>
        /// 获取Point的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<PointListDto>> GetPagedPoints(GetPointsInput input)
        {

            var query = _pointRepository.GetAll();
            //TODO:根据传入的参数添加过滤条件
            var pointCount = await query.CountAsync();

            var points = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var pointListDtos = ObjectMapper.Map<List <PointListDto>>(points);
            var pointListDtos = points.MapTo<List<PointListDto>>();

            return new PagedResultDto<PointListDto>(
                pointCount,
                pointListDtos
                );

        }

        /// <summary>
        /// 通过指定id获取PointListDto信息
        /// </summary>
        public async Task<PointListDto> GetPointByIdAsync(EntityDto<int> input)
        {
            var entity = await _pointRepository.GetAsync(input.Id);

            return entity.MapTo<PointListDto>();
        }

        /// <summary>
        /// 导出Point为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetPointsToExcel(){
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
        public async Task<GetPointForEditOutput> GetPointForEdit(NullableIdDto<int> input)
        {
            var output = new GetPointForEditOutput();
            PointEditDto pointEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _pointRepository.GetAsync(input.Id.Value);

                pointEditDto = entity.MapTo<PointEditDto>();

                //pointEditDto = ObjectMapper.Map<List <pointEditDto>>(entity);
            }
            else
            {
                pointEditDto = new PointEditDto();
            }

            output.Point = pointEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改Point的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdatePoint(CreateOrUpdatePointInput input)
        {
            if (input.Point.Id.HasValue)
            {
                await UpdatePointAsync(input.Point);
            }
            else
            {
                await CreatePointAsync(input.Point);
            }
        }

        /// <summary>
        /// 新增Point
        /// </summary>
        [AbpAuthorize(AppPermissions.Pages_Point_Manage_Create)]
        protected virtual async Task<PointEditDto> CreatePointAsync(PointEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<Point>(input);

            entity = await _pointRepository.InsertAsync(entity);
            return entity.MapTo<PointEditDto>();
        }

        /// <summary>
        /// 编辑Point
        /// </summary>
        [AbpAuthorize(AppPermissions.Pages_Point_Manage_Edit)]
        protected virtual async Task UpdatePointAsync(PointEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _pointRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _pointRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除Point信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Point_Manage_Delete)]
        public async Task DeletePoint(EntityDto<int> input)
        {

            //TODO:删除前的逻辑判断，是否允许删除
            await _pointRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除Point的方法
        /// </summary>
        [AbpAuthorize(AppPermissions.Pages_Point_Manage_Delete)]
        public async Task BatchDeletePointsAsync(List<int> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _pointRepository.DeleteAsync(s => input.Contains(s.Id));
        }

    }
}

