import Cookies from 'js-cookie';
import Util from '@/libs/util'
import appconst from '@/libs/appconst'
const point = {
    namespaced: true,
    state: {
        devices: [],
        orgdevices: [],
        unbindDevices: [],
        goods: [],
        current: null,
        currentCode: "",
        orders:[],
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
        },
        setCurrentCode(state, code) {
            state.currentCode = code;
        }
    },
    actions: {
        async getAll({
            state
        }, payload) {
            let page = {
                maxResultCount: state.pageSize,
                skipCount: (state.currentPage - 1) * state.pageSize,
                name: payload.data.name,
                num: payload.data.num,
                cate: payload.data.cate
            }
            let rep = await Util.ajax.get('/api/services/app/Device/GetPagedDevices', {
                params: page
            });
            state.devices = [];
            state.devices.push(...rep.data.result.items);
            state.totalCount = rep.data.result.totalCount;
        },
        async getOrders({
            state
        }, payload) {
            let page = {
                maxResultCount: state.pageSize,
                skipCount: (state.currentPage - 1) * state.pageSize,
                TreeCode: payload.data.treeCode,
                OrderNum: payload.data.orderNum,
                StartTime: payload.data.startTime,
                EndTime: payload.data.endTime,
                DeviceNum: payload.data.deviceNum
            }
            let rep = await Util.ajax.get('/api/services/app/Other/GetOrdersAsync', {
                params: page
            });
            state.orders = [];
            state.orders.push(...rep.data.result.items);
            state.totalCount = rep.data.result.totalCount;
        },
        async getUnbind({
            state
        }, payload) {
            let page = {
                maxResultCount: state.pageSize,
                skipCount: (state.currentPage - 1) * state.pageSize,
                name: payload.data.name,
                num: payload.data.num
            }
            let rep = await Util.ajax.get('/api/services/app/Device/GetPagedUnBindDevices', {
                params: page
            });
            state.unbindDevices = [];
            state.unbindDevices.push(...rep.data.result.items);
            state.totalCount = rep.data.result.totalCount;
        },

        async getOrgDevices({
            state
        }, payload) {
            let page = {
                maxResultCount: state.pageSize,
                skipCount: (state.currentPage - 1) * state.pageSize,
                OrgId: payload.parentId,
                name: payload.name,
                num: payload.num
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
                DeviceId: state.current,
                name: payload.name,
                sn: payload.sn,
                cate: payload.cate
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
        async deleteRelation({
            state
        }, payload) {
            await Util.ajax.delete('/api/services/app/Device/DeleteRelationDevice?Id=' + payload.data.id);
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