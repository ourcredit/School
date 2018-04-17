﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using School.Authorization.Roles;
using School.Authorization.Users;
using School.Models;

namespace School.Workers
{
   public class GenderAdminWorker: PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<OperatorTree> _orgRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        public GenderAdminWorker(AbpTimer timer, IRepository<Role> roleRepository, IRepository<User, long> userRepository, IRepository<OperatorTree> orgRepository, IRepository<UserRole, long> userRoleRepository) : base(timer)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _orgRepository = orgRepository;
            _userRoleRepository = userRoleRepository;
            Timer.Period = 1000*60*50*24;
        }
        [UnitOfWork]
        protected override void DoWork()
        {
            var sql = @"SELECT
	a.shop_name,
	b.user_id,
	b.email,
	b.user_name,
	b.`password`,ec_salt
	
FROM
	dsc_drp_shop a
	LEFT JOIN dsc_users b ON a.user_id = b.user_id";
            var result = DapperHelper.GetSqlResult<dsc_drp_shop>(sql);
            var adminRole = _roleRepository.FirstOrDefaultAsync(c => c.Name == StaticRoleNames.Tenants.Admin);
            foreach (var item in result.Items)
            {

                var family = _orgRepository.FirstOrDefault(c => c.TreeName == item.shop_name);
                if (family == null)
                {
                    family = new OperatorTree()
                    {
                        TreeLength = 1,
                        TreeName = item.shop_name,
                        TreeCode = Guid.NewGuid().ToString("D").Split('-').Last()
                    };

                }
                else
                {
                    family.TreeName = item.shop_name;
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
                    temp.TreeCode = family.TreeCode;
                    temp.Salt = item.ec_salt;
                    temp.KeyId = item.user_id;
                    temp = _userRepository.Insert(temp);
                    _userRoleRepository.InsertAsync(new UserRole(1, temp.Id, adminRole.Id));
                }
                else
                {
                    temp.Password = item.password;
                    temp.Salt = item.ec_salt;
                    temp.KeyId = item.user_id;
                    temp.UserName = item.user_name;
                    temp = _userRepository.Update(temp);
                }
            }
        }
    }
}
