import Vue from 'vue';
import iView from 'iview';
import {
    router
} from './router/index';
import {
    appRouter
} from './router/router';
import store from './store';
import App from './app.vue';
import 'iview/dist/styles/iview.css';
import util from './libs/util';
import AppConsts from './libs/appconst'
import BaiduMap from 'vue-baidu-map'
util.ajax.get('/AbpUserConfiguration/GetAll').then(result => {
    Vue.use(iView);
    Vue.use(BaiduMap, {
        ak: "pYjoSR2GThuatLt06MlaKzRgSWy4Zztq"
    });
    window.abp = $.extend(true, abp, result.data.result);
    Vue.prototype.L = function (text, ...args) {
        let localizedText = window.abp.localization.localize(text, AppConsts.localization.defaultLocalizationSourceName);
        if (!localizedText) {
            localizedText = text;
        }
        if (!args || !args.length) {
            return localizedText;
        }
        args.unshift(localizedText);
        return abp.utils.formatString.apply(this, args)
    }

    Vue.filter('l', function (value) {
        if (!value) return ''
        return window.abp.localization.localize(value, AppConsts.localization.defaultLocalizationSourceName);
    });
    Vue.prototype.formatter = function (value) {
        if (!value) return ''
        var d = new Date(value);
        var year = d.getFullYear();
        var month = d.getMonth() + 1;
        var day = d.getDate() < 10 ? '0' + d.getDate() : '' + d.getDate();
        var hour = d.getHours();
        var minutes = d.getMinutes();
        var seconds = d.getSeconds();
        return year + '-' + month + '-' + day + ' ' + hour + ':' + minutes + ':' + seconds;
    }
    new Vue({
        el: '#app',
        router: router,
        store: store,
        render: h => h(App),
        data: {
            currentPageName: ''
        },
        mounted() {
            this.currentPageName = this.$route.name;
            // Display a list of open pages
            this.$store.commit('setOpenedList');
            this.$store.commit('initCachepage');

            //Filtering admin menu
            this.$store.commit('updateMenulist');
        },
        created() {
            let tagsList = [];
            appRouter.map((item) => {
                if (item.children.length <= 1) {
                    tagsList.push(item.children[0]);
                } else {
                    tagsList.push(...item.children);
                }
            });

            this.$store.commit('setTagsList', tagsList);
            abp.message.info = (message, title) => {
                this.$Modal.info({
                    title: title,
                    content: message
                })
            };

            abp.message.success = (message, title) => {
                this.$Modal.success({
                    title: title,
                    content: message
                })
            };

            abp.message.warn = (message, title) => {
                this.$Modal.warning({
                    title: title,
                    content: message
                })
            };

            abp.message.error = (message, title) => {
                this.$Modal.error({
                    title: title,
                    content: message
                })
            };

            abp.message.confirm = (message, titleOrCallback, callback) => {
                var userOpts = {
                    content: message
                };
                if ($.isFunction(titleOrCallback)) {
                    callback = titleOrCallback;
                } else if (titleOrCallback) {
                    userOpts.title = titleOrCallback;
                };
                userOpts.onOk = callback || function () {};
                this.$Modal.confirm(userOpts);
            }

            abp.notify.success = (message, title, options) => {
                this.$Notice.success({
                    title: title,
                    desc: message
                })
            };

            abp.notify.info = (message, title, options) => {
                this.$Notice.info({
                    title: title,
                    desc: message
                })
            };

            abp.notify.warn = (message, title, options) => {
                this.$Notice.warning({
                    title: title,
                    desc: message
                })
            };

            abp.notify.error = (message, title, options) => {
                this.$Notice.error({
                    title: title,
                    desc: message
                })
            };
        }
    });
})