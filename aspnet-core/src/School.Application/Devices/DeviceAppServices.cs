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
using School.Devices.Dtos;
using School.Devices.DomainServices;
using School.Models;

namespace School.Devices
{
    /// <summary>
    /// Device应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages_Device_Manage)]
    public class DeviceAppService : SchoolAppServiceBase, IDeviceAppService
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<Device, int> _deviceRepository;
        private readonly IRepository<Point, int> _pointRepository;
        private readonly IRepository<OperatorDevice, int> _operatorDeviceRepository;
        private readonly IRepository<DeviceGood> _goodsRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DeviceAppService(IRepository<Device, int> deviceRepository,
            IRepository<Point, int> pointRepository, 
            IRepository<OperatorDevice, int> operatorDeviceRepository,
            IRepository<DeviceGood> goodsRepository)
        {
            _deviceRepository = deviceRepository;
            _pointRepository = pointRepository;
            _operatorDeviceRepository = operatorDeviceRepository;
            _goodsRepository = goodsRepository;
        }
        /// <summary>
        /// 获取Device的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceListDto>> GetPagedDevices(GetDevicesInput input)
        {
            var query = _deviceRepository.GetAllIncluding(c => c.Point);
            var deviceCount = await query.CountAsync();
            var devices = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();
            //var deviceListDtos = ObjectMapper.Map<List <DeviceListDto>>(devices);
            var deviceListDtos = devices.MapTo<List<DeviceListDto>>();
            return new PagedResultDto<DeviceListDto>(
                deviceCount,
                deviceListDtos
                );
        }

        /// <summary>
        /// 获取未绑定设备信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceListDto>> GetPagedUnBindDevices(GetDevicesInput input)
        {
            var query = _deviceRepository.GetAllIncluding(c => c.Point);
            var bindDevice =await _operatorDeviceRepository.GetAllListAsync();
            var result = from c in query
                join d in bindDevice on c.Id equals d.DeviceId into temp
                from tt in temp.DefaultIfEmpty()
                where tt == null
                select c;
            var deviceCount = await result.CountAsync();
            var devices = await result
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();
            //var deviceListDtos = ObjectMapper.Map<List <DeviceListDto>>(devices);
            var deviceListDtos = devices.MapTo<List<DeviceListDto>>();
            return new PagedResultDto<DeviceListDto>(
                deviceCount,
                deviceListDtos
            );
        }
        /// <summary>
        /// 获取机构树下的设备
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<DeviceListDto>> GetOperatorTreeDevices(GetOrgsDevicesInput input)
        {
            var query = _operatorDeviceRepository.GetAllIncluding(c => c.Device,c=>c.Device.Point);
            query = query.Where(c => c.OperatorId == input.OrgId);
            var deviceCount = await query.CountAsync();
            var devices = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();
            var result=new List<DeviceListDto>();
            foreach (var r in devices)
            {
                var mo = r.Device.MapTo<DeviceListDto>();
                mo.Id = r.Id;
                result.Add(mo);
            }
            return new PagedResultDto<DeviceListDto>(
                deviceCount,
                result
            );
        }
        /// <summary>
        /// 设备和 机构绑定
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BindOrgAndDevices(BindDevicesInput input)
        {
            var devices = await _operatorDeviceRepository.GetAllListAsync(c => c.OperatorId == input.OrgId);
            foreach (var i in input.Devices)
            {
                if(devices.Any(w=>w.DeviceId==i))continue;
                await _operatorDeviceRepository.InsertAsync(new OperatorDevice(input.OrgId, i));
            }
        }
        /// <summary>
        /// 绑定设备和价格
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BindDeviceGoods(BindGoodsInput input)
        {
            var device = await _deviceRepository.FirstOrDefaultAsync(input.DeviceId);
            if (device != null)
            {
                if (device.DeviceGoods!=null&&device.DeviceGoods.Any())
                {
                    await _goodsRepository.DeleteAsync(c => c.DeviceId == device.Id);
                }
                foreach (var i in input.Goods)
                {
                    await _goodsRepository.InsertAsync(
                        new DeviceGood(device.Id, i.GoodId, i.GoodName, i.Price));
                }
            }
        }
        /// <summary>
        /// 设备和 机构解绑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UnBindOrgAndDevices(List<int> input)
        {
             await _operatorDeviceRepository.DeleteAsync(c=>input.Any(w=>w==c.DeviceId));
        }
        /// <summary>
        /// 获取所有点位信息
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<KeyValuePair<int,string>>> GetAllPoints()
        {
            var query = _pointRepository.GetAll();
            var points = await query
                .ToListAsync();

            //var deviceListDtos = ObjectMapper.Map<List <DeviceListDto>>(devices);
            var deviceListDtos = points.Select(c=>new KeyValuePair<int, string>(c.Id,c.PointName)).ToList();
            return new ListResultDto<KeyValuePair<int, string>>(
                deviceListDtos
            );

        }
        /// <summary>
        /// 通过指定id获取DeviceListDto信息
        /// </summary>
        public async Task<DeviceListDto> GetDeviceByIdAsync(EntityDto<int> input)
        {
            var entity = await _deviceRepository.GetAsync(input.Id);

            return entity.MapTo<DeviceListDto>();
        }

        /// <summary>
        /// 导出Device为excel表
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetDevicesToExcel(){
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
        public async Task<GetDeviceForEditOutput> GetDeviceForEdit(NullableIdDto<int> input)
        {
            var output = new GetDeviceForEditOutput();
            DeviceEditDto deviceEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _deviceRepository.GetAsync(input.Id.Value);

                deviceEditDto = entity.MapTo<DeviceEditDto>();

                //deviceEditDto = ObjectMapper.Map<List <deviceEditDto>>(entity);
            }
            else
            {
                deviceEditDto = new DeviceEditDto();
            }

            output.Device = deviceEditDto;
            return output;

        }

        /// <summary>
        /// 添加或者修改Device的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateDevice(CreateOrUpdateDeviceInput input)
        {

            if (input.Device.Id.HasValue)
            {
                await UpdateDeviceAsync(input.Device);
            }
            else
            {
                await CreateDeviceAsync(input.Device);
            }
        }

        /// <summary>
        /// 新增Device
        /// </summary>
        [AbpAuthorize(AppPermissions.Pages_Device_Manage_Create)]
        protected virtual async Task<DeviceEditDto> CreateDeviceAsync(DeviceEditDto input)
        {
            var entity = ObjectMapper.Map<Device>(input);

            entity = await _deviceRepository.InsertAsync(entity);
            return entity.MapTo<DeviceEditDto>();
        }

        /// <summary>
        /// 编辑Device
        /// </summary>
        [AbpAuthorize(AppPermissions.Pages_Device_Manage_Edit)]
        protected virtual async Task UpdateDeviceAsync(DeviceEditDto input)
        {
            var entity = await _deviceRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _deviceRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除Device信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Device_Manage_Delete)]
        public async Task DeleteDevice(EntityDto<int> input)
        {

            await _deviceRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除Device的方法
        /// </summary>
        [AbpAuthorize(AppPermissions.Pages_Device_Manage_Delete)]
        public async Task BatchDeleteDevicesAsync(List<int> input)
        {
            await _deviceRepository.DeleteAsync(s => input.Contains(s.Id));
        }

    }
}

