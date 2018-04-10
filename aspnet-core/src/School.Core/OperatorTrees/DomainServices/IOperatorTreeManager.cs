using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using School.Models;

namespace School.OperatorTrees.DomainServices
{
    public interface IOperatorTreeManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        /// </summary>
        void InitOperatorTree();

    }
}
