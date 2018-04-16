<template>
    <div>
        <baidu-map :center="center" :zoom="15" :scroll-wheel-zoom="true" @ready="handler" class="bm-view">
            <bm-marker @click="showWindows(p)" :key="p.id" v-for="p in points" 
            :icon="{url: './src/images/poi24.png', size: {width: 200, height: 200}}" 
             :position="{lng:p.longitude,lat:p.latitide }" :dragging="false" >
            <!-- <bm-label :offset="100" :content="p.pointName" :position="{lng:p.longitude,lat:p.latitide }"
             :labelStyle="{ color: 'red', fontSize : '24px'}" title="Hover me"/> -->
            </bm-marker>
            
        </baidu-map>
    </div>
</template>
<script>
export default {
  methods: {
    closeWindows(p) {
      p.show = false;
    },
    showWindows(p) {
      p.show = true;
    },
    handler({ BMap, map }) {}
  },
  data() {
    return {
      center: {
        lng: 116.404,
        lat: 39.915
      }
    };
  },
  computed: {
    points() {
      return this.$store.state.point.points;
    }
  },
  async created() {
    this.$store.commit("point/setPageSize", 1000);
    await this.$store.dispatch({
      type: "point/getAll"
    });
  }
};
</script>
<style lang="less" scoped>
.bm-view {
  width: 99%;
  height: 450px;
}
</style>