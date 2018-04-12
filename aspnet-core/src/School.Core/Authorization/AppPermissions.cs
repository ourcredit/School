using MyCompanyName.AbpZeroTemplate.Authorization;

namespace School.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        /// <summary>
        /// 权限
        /// </summary>
        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";

        /// <summary>
        /// 日志
        /// </summary>
        public const string Pages_AuditLogs = "Pages.AuditLogs";
        public const string Pages_AuditLogs_Logs = "Pages.AuditLogs.Logs";

        public const string Pages_AuditLogs_Warns = "Pages.AuditLogs.Warns";

        /// <summary>
        /// 运营商
        /// </summary>
        public const string Pages_Operator = "Pages.Operator";

        public const string Pages_Operator_Orgs = "Pages.Operator.Orgs";
        public const string Pages_Operator_Boxs = "Pages.Operator.Boxs";

        public const string Pages_Operator_Orgs_Create = "Pages.Operator.Orgs.Create";
        public const string Pages_Operator_Orgs_Edit = "Pages.Operator.Orgs.Edit";
        public const string Pages_Operator_Orgs_Delete = "Pages.Operator.Orgs.Delete";
        public const string Pages_Operator_Orgs_BindPrice = "Pages.Operator.Orgs.BindPrice";
        public const string Pages_Operator_Orgs_BindProduct = "Pages.Operator.Orgs.BindProduct";

        public const string Pages_Operator_Product_Create = "Pages.Operator.Product.Create";
        public const string Pages_Operator_Product_Edit = "Pages.Operator.Product.Edit";
        public const string Pages_Operator_Product_Delete = "Pages.Operator.Product.Delete";


        /// <summary>
        /// 订单
        /// </summary>
        public const string Pages_Orders = "Pages.Orders";
        public const string Pages_Orders_OrderList = "Pages.Orders.OrderList";


        /// <summary>
        /// 设备
        /// </summary>
        public const string Pages_Device = "Pages.Device";

        public const string Pages_Device_Manage = "Pages.Device.Manage";

        public const string Pages_Device_Manage_Create = "Pages.Device.Manage.Create";
        public const string Pages_Device_Manage_Edit = "Pages.Device.Manage.Edit";
        public const string Pages_Device_Manage_Delete = "Pages.Device.Manage.Delete";

        /// <summary>
        /// 点位
        /// </summary>
        public const string Pages_Point = "Pages.Point";

        public const string Pages_Point_Manage = "Pages.Point.Manage";

        public const string Pages_Point_Manage_Create = "Pages.Point.Manage.Create";
        public const string Pages_Point_Manage_Edit = "Pages.Point.Manage.Edit";
        public const string Pages_Point_Manage_Delete = "Pages.Point.Manage.Delete";

        public const string Pages_Point_View = "Pages.Point.View";

    }
}