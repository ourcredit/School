import Cookies from 'js-cookie';
import Util from '@/libs/util'
import appconst from '@/libs/appconst'
const point = {
    namespaced: true,
    state: {
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
                filter: payload.data
            }
            let rep = await Util.ajax.get('/api/services/app/Point/GetPagedPoints', {
                params: page
            });
            state.points = [];
            state.points.push(...rep.data.result.items);
            state.totalCount = rep.data.result.totalCount;
        },
        async delete({
            state
        }, payload) {
            await Util.ajax.delete('/api/services/app/Point/DeletePoint?Id=' + payload.data.id);
        },
        async batchDelete({
            state
        }, payload) {
            await Util.ajax.post('/api/services/app/Point/BatchDeletePointsAsync' + payload.data);
        },
        async createOrUpdate({
            state
        }, payload) {
            await Util.ajax.post('/api/services/app/Point/CreateOrUpdatePoint', payload.data);
        }
    }
};
export default point;