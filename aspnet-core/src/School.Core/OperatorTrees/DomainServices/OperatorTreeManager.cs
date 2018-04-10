using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using School.Models;

namespace School.OperatorTrees.DomainServices
{
    /// <summary>
    /// OperatorTree领域层的业务管理
    /// </summary>
    public class OperatorTreeManager : SchoolDomainServiceBase, IOperatorTreeManager
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<OperatorTree, int> _operatortreeRepository;
        /// <summary>
        /// OperatorTree的构造方法
        /// </summary>
        public OperatorTreeManager(IRepository<OperatorTree, int> operatortreeRepository)
        {
            _operatortreeRepository = operatortreeRepository;
        }

        //TODO:编写领域业务代码
        /// <summary>
        ///     初始化
        /// </summary>
        public void InitOperatorTree()
        {
            throw new NotImplementedException();
        }

    }

}
