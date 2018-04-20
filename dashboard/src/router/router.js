import Main from '@/views/Main.vue';
import util from '@/libs/util.js';

//title properties are localization keys.

export const loginRouter = {
    path: '/login',
    name: 'login',
    meta: {
        title: '登陆页'
    },
    component: () =>
        import ('@/views/login.vue')
};

export const page404 = {
    path: '/*',
    name: 'error-404',
    meta: {
        title: '404-未找到页面'
    },
    component: () =>
        import ('@/views/error-page/404.vue')
};

export const page403 = {
    path: '/403',
    meta: {
        title: '403 - 未授权'
    },
    name: 'error-403',
    component: () =>
        import ('@//views/error-page/403.vue')
};

export const page500 = {
    path: '/500',
    meta: {
        title: '500 - 服务器内部错误'
    },
    name: 'error-500',
    component: () =>
        import ('@/views/error-page/500.vue')
};
export const locking = {
    path: '/locking',
    name: 'locking',
    component: () =>
        import ('@/views/main-components/lockscreen/components/locking-page.vue')
};

// A route which is not displayed in the left menu
export const otherRouter = {
    path: '/',
    name: 'otherRouter',
    component: Main,
    children: [{
        path: 'setting',
        title: '商品配置',
        name: 'settings',
        permission: 'Pages.Device.Manage.Create',
        component: () =>
            import ('@/views/operators/setting.vue')
    }, {
        path: 'box',
        title: '货道管理',
        name: 'boxs',
        permission: 'Pages.Operator.Boxs',
        component: () =>
            import ('@/views/operators/box.vue')
    }]
};

// Left menu items
export const appRouter = [{
    path: '/point',
    icon: 'pinpoint',
    title: '点位信息',
    name: 'point',
    component: Main,
    children: [{
        path: 'manage',
        title: '点位管理',
        name: 'pointmanage',
        permission: 'Pages.Point.Manage',
        component: () =>
            import ('@/views/points/index.vue')
    }, {
        path: 'view',
        title: '点位展示',
        name: 'pointview',
        permission: 'Pages.Point.View',
        component: () =>
            import ('@/views/points/view.vue')
    }]
}, {
    path: '/device',
    icon: 'magnet',
    title: '售货机信息',
    name: 'device',
    component: Main,
    children: [{
        path: 'manage',
        title: '售货机管理',
        name: 'devicemanage',
        permission: 'Pages.Device.Manage',
        component: () =>
            import ('@/views/devices/index.vue')
    }]
}, {
    path: '/orders',
    icon: 'record',
    title: '订单信息',
    name: 'orders',
    component: Main,
    children: [{
        path: 'list',
        title: '订单管理',
        name: 'order',
        permission: 'Pages.Orders.OrderList',
        component: () =>
            import ('@/views/orders/index.vue')
    }]
}, {
    path: '/audits',
    icon: 'android-list',
    title: '日志管理',
    name: 'audit',
    component: Main,
    children: [{
        path: 'logs',
        title: '日志信息',
        name: 'logs',
        permission: 'Pages.AuditLogs.Logs',
        component: () =>
            import ('@/views/auditlogs/logs.vue')
    }, {
        path: 'warns',
        title: '报警信息',
        name: 'warns',
        permission: 'Pages.AuditLogs.Warns',
        component: () =>
            import ('@/views/auditlogs/warns.vue')
    }]
}, {
    path: '/operator',
    icon: 'icecream',
    title: '运营商信息',
    name: 'operator',
    component: Main,
    children: [{
        path: 'manage',
        title: '机构树',
        name: 'operatormanage',
        permission: 'Pages.Operator.Orgs',
        component: () =>
            import ('@/views/operators/index.vue')
    }]
}, {
    path: '/admin',
    icon: 'gear-a',
    title: '系统管理',
    name: 'administration',
    component: Main,
    children: [{
            path: 'users',
            title: '用户',
            name: 'users',
            permission: 'Pages.Administration.Users',
            component: () =>
                import ('@/views/admin/users/users.vue')
        },
        {
            path: 'roles',
            title: '角色',
            name: 'roles',
            permission: 'Pages.Administration.Roles',
            component: () =>
                import ('@/views/admin/roles/roles.vue')
        }
    ]
}];
// All the routes defined above should be written in the routers below
export const routers = [
    loginRouter,
    otherRouter,
    locking,
    ...appRouter,
    page500,
    page403,
    page404
];