import Cookies from 'js-cookie';
import Util from '@/libs/util'
import appconst from '@/libs/appconst'
const user = {
    namespaced: true,
    state: {
        roles: [],
        role: null,
        permissions: []
    },
    mutations: {},
    actions: {
        async getAll({
            state
        }, payload) {
            let rep = await Util.ajax.get('/api/services/app/Role/GetRoles');
            state.roles = [];
            state.roles.push(...rep.data.result.items);
        },
        async getRole({
            state
        }, payload) {
            var params = ""
            if (payload.data) {
                params = "?Id=" + payload.data;
            }
            let rep = await Util.ajax.get('/api/services/app/Role/GetRoleForEdit' + params);
            state.role = rep.data.result;
        },
        async delete({
            state
        }, payload) {
            await Util.ajax.delete('/api/services/app/Role/DeleteRole?Id=' + payload.data);
        },
        async createOrUpdate({
            state
        }, payload) {
            await Util.ajax.post('/api/services/app/Role/CreateOrUpdateRole', payload.data);
        },
        async getAllPermissions({
            state
        }) {
            let rep = await Util.ajax.get('/api/services/app/Permission/GetAllPermissions');
            state.permissions = [];
            state.permissions.push(...rep.data.result.items)
        }
    }
};

export default user;