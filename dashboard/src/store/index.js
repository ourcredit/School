import Vue from 'vue';
import Vuex from 'vuex';

import app from './modules/app';
import user from './modules/user';
import session from './modules/session'
import account from './modules/account'
import role from './modules/role'
import point from './modules/point'
import device from './modules/device'
import org from './modules/org'
import channels from './modules/channels'

Vue.use(Vuex);

const store = new Vuex.Store({
    state: {
        //
    },
    mutations: {
        //
    },
    actions: {

    },
    modules: {
        app,
        user,
        session,
        account,
        role,
        point,
        device,
        org,
        channels
    }
});

export default store;