import Cookies from 'js-cookie';
import Util from '@/libs/util'
import appconst from '@/libs/appconst'
const user = {
    namespaced: true,
    state: {
        users: [],
        user: null,
        totalCount: 0,
        pageSize: 10,
        currentPage: 1,
        roles: []
    },
    mutations: {
        logout() {
            abp.auth.clearToken();
            location.reload();
        },
        setPageSize(state, size) {
            state.pageSize = size;
        },
        setCurrentPage(state, page) {
            state.currentPage = page;
        }
    },
    actions: {
        async login({
            state
        }, payload) {
            let rep = await Util.ajax.post("/api/TokenAuth/Authenticate", payload.data);
            var tokenExpireDate = payload.data.rememberMe ? (new Date(new Date().getTime() + 1000 * rep.data.result.expireInSeconds)) : undefined;
            abp.auth.setToken(rep.data.result.accessToken, tokenExpireDate);
            abp.utils.setCookieValue(appconst.authorization.encrptedAuthTokenName, rep.data.result.encryptedAccessToken, tokenExpireDate, abp.appPath)
        },
        async getAll({
            state
        }, payload) {
            let page = {
                maxResultCount: state.pageSize,
                skipCount: (state.currentPage - 1) * state.pageSize
            }
            let rep = await Util.ajax.get('/api/services/app/User/GetUsers', {
                params: page
            });
            state.users = [];
            state.users.push(...rep.data.result.items);
            state.totalCount = rep.data.result.totalCount;
        },
        async delete({
            state
        }, payload) {
            await Util.ajax.delete('/api/services/app/User/DeleteUser?Id=' + payload.data.id);
        },
        async createOrUpdate({
            state
        }, payload) {
            await Util.ajax.post('/api/services/app/User/CreateOrUpdateUser', payload.data);
        },
        async getUser({
            state
        }, payload) {
            var params = ""
            if (payload.data) {
                params = "?Id=" + payload.data;
            }
            let rep = await Util.ajax.get('/api/services/app/User/GetUserForEdit' + params);
            state.user = null;
            state.user = rep.data.result;
        },
        async getRoles({
            state
        }) {
            let rep = await Util.ajax.get('/api/services/app/Role/GetRoles');
            state.roles = [];
            state.roles.push(...rep.data.result.items)
        }
    }
};

export default user;