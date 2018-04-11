<template>
    <div>
        <Table @on-selection-change="select" :columns="columns" border :data="devices"></Table>
        <Page :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize"
            :current="currentPage"></Page>
    </div>
</template>
<script>
export default {
  methods: {
    select(temp) {
      this.models = temp;
      this.$emit("cc", this.models);
    },
    pageChange(page) {
      this.$store.commit("device/setCurrentPage", page);
      this.getpage();
    },
    pagesizeChange(pagesize) {
      this.$store.commit("device/setPageSize", pagesize);
      this.getpage();
    },
    async getpage() {
      await this.$store.dispatch({
        type: "device/getAll"
      });
    }
  },
  data() {
    return {
      models: [],
      columns: [
        {
          type: "selection",
          width: 60,
          align: "center"
        },
        {
          title: "设备名",
          key: "deviceName"
        },
        {
          title: "设备编号",
          key: "deviceNum"
        },
        {
          title: "设备类型",
          key: "deviceType"
        },
        {
          title: "所属点位",
          key: "pointName"
        }
      ]
    };
  },
  computed: {
    devices() {
      return this.$store.state.device.devices;
    },
    totalCount() {
      return this.$store.state.device.totalCount;
    },
    currentPage() {
      return this.$store.state.device.currentPage;
    },
    pageSize() {
      return this.$store.state.device.pageSize;
    }
  },
  async created() {
    this.models = [];
    this.getpage();
  }
};
</script>