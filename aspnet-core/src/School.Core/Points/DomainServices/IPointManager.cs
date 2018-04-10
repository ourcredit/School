using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using School.Models;

namespace School.Points.DomainServices
{
    public interface IPointManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        /// </summary>
        void InitPoint();

    }
}
