﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using School.Devices.Dtos;
using School.Models;

namespace School.Devices
{
    /// <summary>
    /// Device应用层服务的接口方法
    /// </summary>
    public interface IDeviceAppService : IApplicationService
    {
        /// <summary>
        /// 获取Device的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceListDto>> GetPagedDevices(GetDevicesInput input);

        /// <summary>
        /// 获取未绑定设备信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceListDto>> GetPagedUnBindDevices(GetDevicesInput input);
        /// <summary>
        /// 通过指定id获取DeviceListDto信息
        /// </summary>
        Task<DeviceListDto> GetDeviceByIdAsync(EntityDto<int> input);

        /// <summary>
        /// 删除Device信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteRelationDevice(EntityDto<int> input);
        /// <summary>
        /// 绑定设备和价格
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BindDeviceGoods(BindGoodsInput input);
        /// <summary>
        /// 获取机构树下的设备
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DeviceListDto>> GetOperatorTreeDevices(GetOrgsDevicesInput input);

        /// <summary>
        /// 设备和 机构绑定
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BindOrgAndDevices(BindDevicesInput input);
        /// <summary>
        /// 设备和 机构解绑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UnBindOrgAndDevices(List<int> input);
        /// <summary>
        /// 获取所有点位信息
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<KeyValuePair<int, string>>> GetAllPoints();
        /// <summary>
        /// 导出Device为excel表
        /// </summary>
        /// <returns></returns>
        //  Task<FileDto> GetDevicesToExcel();
        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetDeviceForEditOutput> GetDeviceForEdit(NullableIdDto<int> input);
        /// <summary>
        /// 添加或者修改Device的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateDevice(CreateOrUpdateDeviceInput input);

        /// <summary>
        /// 删除Device信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteDevice(EntityDto<int> input);

        /// <summary>
        /// 批量删除Device
        /// </summary>
        Task BatchDeleteDevicesAsync(List<int> input);
    }
}
