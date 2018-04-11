import Cookies from 'js-cookie';
import Util from '@/libs/util'
import appconst from '@/libs/appconst'
const org = {
    namespaced: true,
    state: {
        orgs: [],
    },
    mutations: {},
    actions: {
        async getAll({
            state
        }, payload) {
            let page = {
                filter: payload.filter
            }
            let rep = await Util.ajax.get('/api/services/app/OperatorTree/GetOperatorTrees', {
                params: page
            });
            state.orgs = [];
            state.orgs.push(...rep.data.result.items);
        },
        async delete({
            state
        }, payload) {
            await Util.ajax.delete('/api/services/app/OperatorTree/DeleteOperatorTree?Id=' + payload.data);
        },
        async batchDelete({
            state
        }, payload) {
            await Util.ajax.post('/api/services/app/OperatorTree/BatchDeleteOperatorTreesAsync' + payload.data);
        },
        async createOrUpdate({
            state
        }, payload) {
            await Util.ajax.post('/api/services/app/OperatorTree/CreateOrUpdateOperatorTree', payload.data);
        }
    }
};
export default org;