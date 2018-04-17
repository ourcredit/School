using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using School.Models;

namespace School.OperatorTrees.DomainServices
{
    public interface IOperatorTreeManager : IDomainService
    {

       

        void GenderAdmins();

    }
}
