import Main from '@/views/Main.vue';
import util from '@/libs/util.js';

//title properties are localization keys.

export const loginRouter = {
    path: '/login',
    name: 'login',
    meta: {
        title: 'LogIn'
    },
    component: () =>
        import ('@/views/login.vue')
};

export const page404 = {
    path: '/*',
    name: 'error-404',
    meta: {
        title: '404 - Page does not exist'
    },
    component: () =>
        import ('@/views/error-page/404.vue')
};

export const page403 = {
    path: '/403',
    meta: {
        title: '403 - You are not authorized'
    },
    name: 'error-403',
    component: () =>
        import ('@//views/error-page/403.vue')
};

export const page500 = {
    path: '/500',
    meta: {
        title: '500 - Server error'
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
    redirect: '/home',
    component: Main,
    children: [{
        path: 'home',
        title: 'HomePage',
        name: 'home_index',
        component: () =>
            import ('@/views/home/home.vue')
    }]
};

// Left menu items
export const appRouter = [{
    path: '/point',
    icon: 'settings',
    title: '点位信息',
    name: 'point',
    component: Main,
    children: [{
        path: 'manage',
        title: '点位管理',
        name: 'pointmanage',
        permission: 'Pages.Administration.Users',
        component: () =>
            import ('@/views/admin/tenants/tenants.vue')
    }]
}, {
    path: '/device',
    icon: 'settings',
    title: '设备信息',
    name: 'device',
    component: Main,
    children: [{
        path: 'manage',
        title: '设备管理',
        name: 'devicemanage',
        permission: 'Pages.Administration.Users',
        component: () =>
            import ('@/views/admin/tenants/tenants.vue')
    }]
}, {
    path: '/orders',
    icon: 'settings',
    title: '订单信息',
    name: 'orders',
    component: Main,
    children: [{
        path: 'list',
        title: '订单管理',
        name: 'order',
        permission: 'Pages.Administration.Users',
        component: () =>
            import ('@/views/admin/tenants/tenants.vue')
    }]
}, {
    path: '/audits',
    icon: 'settings',
    title: '日志管理',
    name: 'audit',
    component: Main,
    children: [{
        path: 'logs',
        title: '日志信息',
        name: 'logs',
        permission: 'Pages.Administration.Users',
        component: () =>
            import ('@/views/admin/tenants/tenants.vue')
    }, {
        path: 'warns',
        title: '报警信息',
        name: 'warns',
        permission: 'Pages.Administration.Users',
        component: () =>
            import ('@/views/admin/tenants/tenants.vue')
    }]
}, {
    path: '/operator',
    icon: 'settings',
    title: '运营商信息',
    name: 'operator',
    component: Main,
    children: [{
        path: 'manage',
        title: '机构树',
        name: 'operatormanage',
        permission: 'Pages.Administration.Users',
        component: () =>
            import ('@/views/admin/tenants/tenants.vue')
    }]
}, {
    path: '/admin',
    icon: 'settings',
    title: '系统管理',
    name: 'administration',
    component: Main,
    children: [{
            path: 'tenants',
            title: '租户',
            name: 'tenants',
            permission: 'Pages.Tenants',
            component: () =>
                import ('@/views/admin/tenants/tenants.vue')
        },
        {
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
        //,
        // {
        //     path: 'about',
        //     title: '关于',
        //     name: 'about',
        //     component: () =>
        //         import ('@/views/admin/about/about.vue')
        // }
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