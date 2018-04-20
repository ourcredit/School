<template>
  <div>
    <Card>
      <p slot="title">货道信息</p>
      <Tabs value="detail">
        <TabPane label="货道" name="detail">
           <Table :columns="channelsC" border :data="channels"></Table>
            <Page :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize"
                :current="currentPage"></Page>
        </TabPane>
        <TabPane label="展示位" name="roles">
          <Table :columns="boxsC" border :data="boxs"></Table>
            <Page :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize"
                :current="currentPage"></Page>
        </TabPane>
     
      </Tabs>
    </Card>
    </Row>
  </div>
</template>
<script>
export default {
  methods: {
    async init() {
      await this.$store.dispatch({
        type: "channels/getShows",
        data: this.currentCode
      });
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
        type: "channels/getChannels",
        data: this.currentCode
      });
    }
  },
  data() {
    return {
      channelsC: [
        {
          title: "货道编号",
          key: "site"
        },
        {
          title: "商品原料",
          key: "goods_Name"
        },
        {
          title: "余量",
          key: "quantity"
        },
        {
          title: "状态",
          key: "state"
        }
      ],
      boxsC: [
        {
          title: "展示位编号",
          key: "site"
        },
        {
          title: "商品",
          key: "goods_Name"
        }
      ]
    };
  },
  computed: {
    channels() {
      return this.$store.state.channels.channels;
    },
    boxs() {
      return this.$store.state.channels.shows;
    },
    totalCount() {
      return this.$store.state.device.totalCount;
    },
    currentPage() {
      return this.$store.state.device.currentPage;
    },
    pageSize() {
      return this.$store.state.device.pageSize;
    },
    currentCode() {
      return this.$store.state.device.currentCode;
    }
  },
  async created() {
    this.init();
    this.getpage();
  }
};
</script>