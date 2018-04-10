using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using School.Models;

namespace School.Points.DomainServices
{
    /// <summary>
    /// Point领域层的业务管理
    /// </summary>
    public class PointManager : SchoolDomainServiceBase, IPointManager
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<Point, int> _pointRepository;
        /// <summary>
        /// Point的构造方法
        /// </summary>
        public PointManager(IRepository<Point, int> pointRepository)
        {
            _pointRepository = pointRepository;
        }

        //TODO:编写领域业务代码
        /// <summary>
        ///     初始化
        /// </summary>
        public void InitPoint()
        {
            throw new NotImplementedException();
        }

    }

}
