using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using School.Models;

namespace School.Devices.DomainServices
{
    /// <summary>
    /// Device领域层的业务管理
    /// </summary>
    public class DeviceManager : SchoolDomainServiceBase, IDeviceManager
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<Device, int> _deviceRepository;
        /// <summary>
        /// Device的构造方法
        /// </summary>
        public DeviceManager(IRepository<Device, int> deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        //TODO:编写领域业务代码
        /// <summary>
        ///     初始化
        /// </summary>
        public void InitDevice()
        {
            throw new NotImplementedException();
        }

    }

}
