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
            <Page :total="totalCount" class="margin-top-10"
             @on-change="pageChange"
              @on-page-size-change="pagesizeChange"
               :page-size="pageSize"
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
      if (data.length == 1) {
        this.parent = {
          parentId: data[0].id,
          parentName: data[0].title
        };
        await this.getpage();
      } else {
        this.parent = null;
      }
    },
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
    orgs() {
      let orgs = this.$store.state.org.orgs;
      return this.$tree(orgs, null, "parentId");
    },
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
    this.init();
  }
};
</script>



