using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using School.Models;

namespace School.Devices.DomainServices
{
    public interface IDeviceManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        /// </summary>
        void InitDevice();

    }
}
