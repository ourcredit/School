using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using School.Authorization.Roles;
using School.Authorization.Users;
using School.Models;

namespace School.OperatorTrees.DomainServices
{
    /// <summary>
    /// OperatorTree领域层的业务管理
    /// </summary>
    public class OperatorTreeManager : SchoolDomainServiceBase, IOperatorTreeManager
    {

        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User,long> _userRepository;
        private readonly IRepository<OperatorTree> _orgRepository;
        private readonly IRepository<UserRole,long> _userRoleRepository;
        /// <summary>
        /// OperatorTree的构造方法
        /// </summary>
        public OperatorTreeManager(
            IRepository<OperatorTree> orgRepository,
            IRepository<User, long> userRepository,
            IRepository<Role> roleRepository, IRepository<UserRole,long> userRoleRepository)
        {
            _orgRepository = orgRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public void GenderAdmins()
        {
            var sql = @"SELECT
	a.user_id,
	a.email,
	a.user_name,
	a.`password`,
	a.ec_salt,
	b.shop_name ,b.ru_id
FROM
	dsc_admin_user a
	LEFT JOIN dsc_seller_shopinfo b ON a.ru_id = b.ru_id where 1=1";
            var result = DapperHelper.GetSqlResult<dsc_drp_shop>(sql);
            var adminRole = _roleRepository.FirstOrDefault(c => c.Name == StaticRoleNames.Tenants.Admin);
            foreach (var item in result.Items)
            {
              
                var family = _orgRepository.FirstOrDefault(c => c.TreeName == item.shop_name);
                if (family == null)
                {
                    family = new OperatorTree()
                    {
                        TreeLength = 1,
                        ShopId = item.ru_id,
                        TreeName = item.shop_name,
                        TreeCode = Guid.NewGuid().ToString("D").Split('-').Last()
                    };
                  
                }
                else
                {
                    family.TreeName = item.shop_name;
                    family.ShopId = item.ru_id;
                }
                _orgRepository.InsertOrUpdate(family);
                var temp = _userRepository.FirstOrDefault(u =>
                    u.UserName == item.user_name);
                if (temp == null)
                {
                    temp = User.CreateTenantUser(1, item.user_name, item.user_name, item.email);
                    temp.Password = item.password;
                    temp.IsEmailConfirmed = true;
                    temp.IsActive = true;
                    temp.IsAdmin = true;
                    temp.ShopId = item.ru_id;
                    temp.TreeCode = family.TreeCode;
                    temp.Salt = item.ec_salt;
                    temp.EmailAddress = item.user_name + "@" + item.user_id + ".com";
                    temp.KeyId = item.user_id;
                  temp=  _userRepository.Insert(temp);
                    _userRoleRepository.Insert(new UserRole(1, temp.Id, adminRole.Id));
                }
                else
                {
                    temp.Password = item.password;
                    temp.Salt = item.ec_salt;
                    temp.KeyId = item.user_id;
                    temp.EmailAddress = item.user_name + "@" + item.user_id + ".com";
                    temp.ShopId = item.ru_id;
                    temp.UserName = item.user_name;
                    _userRepository.Update(temp);
                }
            }
        }
    }

}
