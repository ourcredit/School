<template>
    <div>
   <Card>
      <p slot="title">售货机管理</p>
      <Row :gutter="8" slot="extra">
        <i-col span="6">
          <Input placeholder="售货机名" v-model="params.name">
          </Input>
        </i-col>
        <i-col span="6">
          <Input placeholder="售货机编号" v-model="params.num">
          </Input>
        </i-col>
        <i-col span="3">
          <i-button @click="getpage" type="primary">查询</i-button>
        </i-col>
      </Row>
      <Table @on-selection-change="select" :columns="columns" border :data="devices"></Table>
        <Page :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize"
            :current="currentPage"></Page>
    </Card>
      
      
    </div>
</template>
<script>
export default {
  methods: {
    select(temp) {
      this.$emit("cc", temp);
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
        type: "device/getUnbind",
        data: this.params
      });
    }
  },
  data() {
    return {
      params: {
        name: "",
        num: ""
      },
      columns: [
        {
          type: "selection",
          width: 60,
          align: "center"
        },
        {
          title: "售货机名",
          key: "deviceName"
        },
        {
          title: "售货机编号",
          key: "deviceNum"
        },
        {
          title: "售货机类型",
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
      return this.$store.state.device.unbindDevices;
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
    this.getpage();
  },
  activated() {
    this.getpage();
  }
};
</script>