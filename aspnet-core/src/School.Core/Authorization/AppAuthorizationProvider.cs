using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace School.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("页面"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("权限管理"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("角色"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("创建角色"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("编辑角色"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("删除角色"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("用户"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("创建用户"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("编辑用户"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("删除用户"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("修改权限"));


            var auditlogs = pages.CreateChildPermission(AppPermissions.Pages_AuditLogs, L("审计日志"));
            auditlogs.CreateChildPermission(AppPermissions.Pages_AuditLogs_Logs, L("日志信息"));
            auditlogs.CreateChildPermission(AppPermissions.Pages_AuditLogs_Warns, L("报警信息"));

            var operators = pages.CreateChildPermission(AppPermissions.Pages_Operator, L("运营商管理"));
            var tree = operators.CreateChildPermission(AppPermissions.Pages_Operator_Orgs, L("组织机构树"));
                 tree.CreateChildPermission(AppPermissions.Pages_Operator_Orgs_Create, L("创建树"));
                 tree.CreateChildPermission(AppPermissions.Pages_Operator_Orgs_Edit, L("编辑树"));
                 tree.CreateChildPermission(AppPermissions.Pages_Operator_Orgs_Delete, L("删除树"));
                 tree.CreateChildPermission(AppPermissions.Pages_Operator_Orgs_BindPrice, L("绑定价格"));
                 tree.CreateChildPermission(AppPermissions.Pages_Operator_Orgs_BindProduct, L("绑定产品"));

           operators.CreateChildPermission(AppPermissions.Pages_Operator_Orgs, L("货道信息"));


            var orders = pages.CreateChildPermission(AppPermissions.Pages_Orders, L("订单信息"));
            orders.CreateChildPermission(AppPermissions.Pages_Orders_OrderList, L("订单列表"));

            var devices = pages.CreateChildPermission(AppPermissions.Pages_Device, L("设备信息"));
            var manage = devices.CreateChildPermission(AppPermissions.Pages_Device_Manage, L("设备管理"));
            manage.CreateChildPermission(AppPermissions.Pages_Device_Manage_Create, L("创建设备"));
            manage.CreateChildPermission(AppPermissions.Pages_Device_Manage_Edit, L("编辑设备"));
            manage.CreateChildPermission(AppPermissions.Pages_Device_Manage_Delete, L("删除设备"));

            var points = pages.CreateChildPermission(AppPermissions.Pages_Point, L("点位信息"));
            var pmanage = points.CreateChildPermission(AppPermissions.Pages_Point_Manage, L("点位管理"));
            var view = points.CreateChildPermission(AppPermissions.Pages_Point_Manage, L("点位查看"));
            pmanage.CreateChildPermission(AppPermissions.Pages_Point_Manage_Create, L("创建点位"));
            pmanage.CreateChildPermission(AppPermissions.Pages_Point_Manage_Edit, L("编辑点位"));
            pmanage.CreateChildPermission(AppPermissions.Pages_Point_Manage_Delete, L("删除点位"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SchoolConsts.LocalizationSourceName);
        }
    }
}
