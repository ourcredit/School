using System.Collections.Generic;
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
        /// 通过指定id获取DeviceListDto信息
        /// </summary>
        Task<DeviceListDto> GetDeviceByIdAsync(EntityDto<int> input);

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

        //todo:缺少Dto的生成GetDeviceForEditOutput
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
