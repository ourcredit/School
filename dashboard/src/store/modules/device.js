import Cookies from 'js-cookie';
import Util from '@/libs/util'
import appconst from '@/libs/appconst'
const point = {
    namespaced: true,
    state: {
        devices: [],
        orgdevices: [],
        goods: [],
        current: null,
        points: [],
        totalCount: 0,
        pageSize: 10,
        currentPage: 1
    },
    mutations: {
        setPageSize(state, size) {
            state.pageSize = size;
        },
        setCurrentPage(state, page) {
            state.currentPage = page;
        },
        setCurrent(state, model) {
            state.current = model;
        }
    },
    actions: {
        async getAll({
            state
        }, payload) {
            let page = {
                maxResultCount: state.pageSize,
                skipCount: (state.currentPage - 1) * state.pageSize,
                filter: payload.filter
            }
            let rep = await Util.ajax.get('/api/services/app/Device/GetPagedDevices', {
                params: page
            });
            state.devices = [];
            state.devices.push(...rep.data.result.items);
            state.totalCount = rep.data.result.totalCount;
        },
        async getUnbind({
            state
        }, payload) {
            let page = {
                maxResultCount: state.pageSize,
                skipCount: (state.currentPage - 1) * state.pageSize
            }
            let rep = await Util.ajax.get('/api/services/app/Device/GetPagedUnBindDevices', {
                params: page
            });
            state.devices = [];
            state.devices.push(...rep.data.result.items);
            state.totalCount = rep.data.result.totalCount;
        },

        async getOrgDevices({
            state
        }, payload) {
            let page = {
                maxResultCount: state.pageSize,
                skipCount: (state.currentPage - 1) * state.pageSize,
                OrgId: payload.parentId
            }
            let rep = await Util.ajax.get('/api/services/app/Device/GetOperatorTreeDevices', {
                params: page
            });
            state.orgdevices = [];
            state.orgdevices.push(...rep.data.result.items);
            state.totalCount = rep.data.result.totalCount;
        },
        async getGoods({
            state
        }, payload) {
            let page = {
                maxResultCount: state.pageSize,
                skipCount: (state.currentPage - 1) * state.pageSize,
                DeviceId: state.current
            }
            let rep = await Util.ajax.get('/api/services/app/Other/GetPagedGoods', {
                params: page
            });
            state.goods = [];
            state.goods.push(...rep.data.result.items);
            state.totalCount = rep.data.result.totalCount;
        },
        async getAllPoints({
            state
        }, payload) {
            let rep = await Util.ajax.get('/api/services/app/Device/GetAllPoints');
            state.points = [];
            state.points.push(...rep.data.result.items);
        },
        async delete({
            state
        }, payload) {
            await Util.ajax.delete('/api/services/app/Device/DeleteDevice?Id=' + payload.data.id);
        },
        async bindDevice({
            state
        }, payload) {
            await Util.ajax.post('/api/services/app/Device/BindOrgAndDevices', payload.data);
        },
        async bindGoods({
            state
        }, payload) {
            await Util.ajax.post('/api/services/app/Device/BindDeviceGoods', payload.data);
        },

        async unBindDevice({
            state
        }, payload) {
            await Util.ajax.post('/api/services/app/Device/UnBindOrgAndDevices', payload.data);
        },
        async batchDelete({
            state
        }, payload) {
            await Util.ajax.post('/api/services/app/Device/DeleteDevice' + payload.data);
        },
        async createOrUpdate({
            state
        }, payload) {
            await Util.ajax.post('/api/services/app/Device/CreateOrUpdateDevice', payload.data);
        }
    }
};
export default point;