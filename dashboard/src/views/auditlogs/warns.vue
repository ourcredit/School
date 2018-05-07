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
        <i-col span="6">
          <Select style="width:140px" v-model="params.cate" placeholder="请选择">
              <Option value="格子机">格子机</Option>
              <Option value="无人销售机">无人销售机</Option>
              <Option value="咖啡机">咖啡机</Option>
              <Option value="饮料机">饮料机</Option>
            </Select>
        </i-col>
        <i-col span="3">
          <i-button @click="getpage" type="primary">查询</i-button>
        </i-col>
      
      </Row>
      <Table :columns="columns" border :data="devices"></Table>
      <Page :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize"
        :current="currentPage"></Page>
    </Card>
  </div>
</template>
<script>
export default {
  methods: {
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
        type: "device/getAll",
        data: this.params
      });
    }
  },
  data() {
    return {
      params: {
        name: "",
        num: "",
        cate: ""
      },
      device: {
        pointId: null,
        deviceType: ""
      },
      columns: [
        {
          title: "售货机名称",
          key: "deviceName"
        },
        {
          title: "售货机编号",
          key: "deviceNum"
        },
          {
          title: "机控编号",
          key: "controlNum"
        },
        {
          title: "售货机类型",
          key: "deviceType"
        },
        {
          title: "所属点位",
          key: "pointName"
        },
        {
          title: "创建时间",
          key: "creationTime",
          render: (h, params) => {
            return this.formatter(params.row.creationTime);
          }
        },
      
      ]
    };
  },
  computed: {
    devices() {
      return this.$store.state.device.devices;
    },
    points() {
      return this.$store.state.device.points;
    },
    totalCount() {
      return this.$store.state.device.totalCount;
    },
    currentPage() {
      return this.$store.state.device.currentPage;
    },
    pageSize() {
      return this.$store.state.point.pageSize;
    }
  },
  async created() {
    this.getpage();
    await this.$store.dispatch({
      type: "device/getAllPoints"
    });
  }
};
</script>