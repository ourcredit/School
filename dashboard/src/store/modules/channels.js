import Cookies from 'js-cookie';
import Util from '@/libs/util'
import appconst from '@/libs/appconst'
const point = {
    namespaced: true,
    state: {
        channels: [],
        shows: [],
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
        async getChannels({
            state
        }, payload) {
            let page = {
                maxResultCount: state.pageSize,
                skipCount: (state.currentPage - 1) * state.pageSize,
                machineCode: payload.filter,
                MachineCode: payload.data
            }
            let rep = await Util.ajax.get('/api/services/app/Other/GetPagedChannels', {
                params: page
            });
            state.channels = [];
            state.channels.push(...rep.data.result.items);
            state.totalCount = rep.data.result.totalCount;
        },
        async getShows({
            state
        }, payload) {
            let page = {
                maxResultCount: state.pageSize,
                skipCount: (state.currentPage - 1) * state.pageSize,
                MachineCode: payload.data
            }
            let rep = await Util.ajax.get('/api/services/app/Other/GetPagedBoxs', {
                params: page
            });
            state.shows = [];
            state.shows.push(...rep.data.result.items);
            state.totalCount = rep.data.result.totalCount;
        }

    }
};
export default point;