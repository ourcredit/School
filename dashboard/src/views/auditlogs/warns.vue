<template>
    <div>
        <Card>
            <p slot="title">报警信息</p>
            <Table :columns="columns" border :data="orders"></Table>
            <Page :total="totalCount" class="margin-top-10"
             @on-change="pageChange"
              @on-page-size-change="pagesizeChange"
               :page-size="pageSize"
                :current="currentPage"></Page>
        </Card>
    </div>
</template>
<script>
export default {
  methods: {
    pageChange(page) {
      this.$store.commit("order/setCurrentPage", page);
      this.getpage();
    },
    pagesizeChange(pagesize) {
      this.$store.commit("order/setPageSize", pagesize);
      this.getpage();
    },
    async getpage() {
      await this.$store.dispatch({
        type: "order/getAll"
      });
    },
    detail() {}
  },
  data() {
    return {
      order: {},
      showModal: false,
      columns: [
        {
          title: "订单编号",
          key: "name"
        },
        {
          title: "商品名称",
          key: "displayName"
        },
        {
          title: "设备名称",
          key: "displayName"
        },
        {
          title: "设备编号",
          key: "displayName"
        },
        {
          title: "设备类型",
          key: "displayName"
        },
        {
          title: "所属点位",
          key: "displayName"
        },
        {
          title: "订单金额",
          key: "displayName"
        },
        {
          title: "订单状态",
          key: "displayName"
        },
        {
          title: "订单时间",
          key: "displayName"
        }
      ]
    };
  },
  computed: {
    orders() {
      return this.$store.state.role.roles;
    },
    totalCount() {
      return this.$store.state.role.totalCount;
    },
    currentPage() {
      return this.$store.state.role.currentPage;
    },
    pageSize() {
      return this.$store.state.role.pageSize;
    }
  },
  async created() {
    this.getpage();
  }
};
</script>



