import Cookies from 'js-cookie';
import Util from '@/libs/util'
import appconst from '@/libs/appconst'
const point = {
    namespaced: true,
    state: {
        devices: [],
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
        async batchDelete({
            state
        }, payload) {
            await Util.ajax.post('/api/services/app/Device/DeleteDevice' + payload.data);
        },
        async createOrUpdate({
            state
        }, payload) {
            await Util.ajax.post('/api/services/app/Device/BatchDeleteDevicesAsync', payload.data);
        }
    }
};
export default point;