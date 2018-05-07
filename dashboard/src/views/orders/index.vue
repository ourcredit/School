<template>
  <div>
    <Row :gutter="16">
      <Col span="5">
      <Card>
        <p slot="title">机构信息</p>
        <Tree @on-select-change="change" :data="orgs"></Tree>
      </Card>
      </Col>
      <Col span="19">
      <Card>
        <p slot="title">订单信息</p>
        <Table :columns="columns" border :data="orders"></Table>
        <Page :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize"
          :current="currentPage"></Page>
      </Card>
      </Col>
    </Row>
  </div>
</template>
<script>
  export default {
    methods: {
      async change(data) {
        this.params.treeCode = data[0].treeCode;
        this.getpage();
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
        if (!this.params.treeCode) return;
        await this.$store.dispatch({
          type: "device/getOrders",
          data: this.params
        });
      },
      async init() {
        await this.$store.dispatch({
          type: "org/getAll"
        });
      },
      detail() {}
    },
    data() {
      return {
        order: {},
        params: {
          treeCode: "",
          orderNum: "",
          startTime: "",
          endTime: null,
          deviceNum: null
        },
        showModal: false,
        columns: [{
            title: "订单编号",
            key: "order_id"
          },
          {
            title: "商品名称",
            key: "goods_name"
          },
          {
            title: "售货机编号",
            key: "vmid"
          },
          {
            title: "订单金额",
            key: "pay_price"
          },
          {
            title: "订单状态",
            key: "status"
          },
          {
            title: "订单时间",
            key: "created_time"
          }
        ]
      };
    },
    computed: {
      orgs() {
        let orgs = this.$store.state.org.orgs;
        return this.$tree(orgs, null, "parentId");
      },
      orders() {
        return this.$store.state.device.orders;
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
      this.init();
    }
  };
</script>