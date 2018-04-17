<template>
  <div>
    <Card>
      <p slot="title">商品配置</p>
      <Row :gutter="8" slot="extra">
        <i-col span="5">
          <Input placeholder="商品名" v-model="params.name">
          </Input>
        </i-col>
        <i-col span="5">
          <Input placeholder="商品编号" v-model="params.sn">
          </Input>
        </i-col>
        <i-col span="4">
          <Input placeholder="商品类型" v-model="params.cate">
          </Input>
        </i-col>
        <i-col span="4">
          <Select style="width:100px" v-model="params.isSeal" placeholder="请选择">
            <Option value="null">全部</Option>
            <Option value="true">售卖</Option>
            <Option value="false">非售卖</Option>
          </Select>
        </i-col>
        <i-col span="2">
          <i-button @click="getpage" type="primary">查询</i-button>
        </i-col>
        <i-col span="2">
          <i-button @click="save" type="primary">保存</i-button>
        </i-col>
      </Row>
      <Table :columns="columns" border :data="goods"></Table>
      <Page :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize"
        :current="currentPage"></Page>
    </Card>

  </div>
</template>
<script>
export default {
  methods: {
    async save() {
      if (!this.tempArr) {
        abp.message.warn("请选择商品");
        return;
      }
      if (!this.current) {
        this.$router.push({
          name: "operatormanage"
        });
        return;
      }
      await this.$store.dispatch({
        type: "device/bindGoods",
        data: {
          deviceId: this.current,
          goods: this.tempArr
        }
      });
      this.$router.push({
        name: "operatormanage"
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
        type: "device/getGoods"
      });
    }
  },
  data() {
    return {
      params: {
        name: "",
        sn: "",
        cate: "",
        isSeal: null
      },
      tempArr: [],
      columns: [
        {
          title: "商品名称",
          key: "goods_name"
        },
        {
          title: "商品编号",
          key: "goods_sn"
        },
        {
          title: "商品类型",
          key: "cat_name"
        },
        {
          title: "是否售卖",
          key: "isSeal",
          render: (h, params) => {
            return h("Checkbox", {
              props: {
                value: this.goods[params.index].isSeal,
                disabled: false
              },
              on: {
                "on-change": e => {
                  this.goods[params.index].isSeal = e;
                  if (e) {
                    var model = this.tempArr.find(
                      c => c.goodId == params.row.goods_id
                    );
                    if (!model) {
                      this.tempArr.push({
                        goodId: params.row.goods_id,
                        goodName: params.row.goods_name,
                        price: 0
                      });
                    }
                  } else {
                    this.tempArr = this.tempArr.filter(
                      c => c.goodId != params.row.goods_id
                    );
                  }
                  console.log(this.tempArr);
                }
              }
            });
          }
        },
        {
          title: "制定价格",
          key: "price",
          render: (h, params) => {
            return h("Input", {
              props: {
                type: "text",
                value: this.goods[params.index].price,
                disabled: !this.goods[params.index].isSeal
              },
              on: {
                "on-blur": e => {
                  this.goods[params.index].price = e.target.value;
                  var model = this.tempArr.find(
                    c => c.goodId == params.row.goods_id
                  );
                  if (model) {
                    model.price = e.target.value;
                  }
                }
              }
            });
          }
        }
      ]
    };
  },
  computed: {
    goods() {
      return this.$store.state.device.goods;
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
    current() {
      return this.$store.state.device.current;
    }
  },
  async created() {
    if (!this.current) {
      this.$router.push({
        name: "operatormanage"
      });
    }
    this.getpage();
  }
};
</script>