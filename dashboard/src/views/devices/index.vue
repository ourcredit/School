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
        <i-col span="3">
          <i-button @click="create" type="primary">添加</i-button>
        </i-col>
      </Row>
      <Table :columns="columns" border :data="devices"></Table>
      <Page :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize"
        :current="currentPage"></Page>
    </Card>
    <Modal v-model="showEditModal" title="编辑售货机" @on-ok="save" okText="保存" cancelText="关闭">
      <div>
        <Form inline ref="roleForm" label-position="top" :rules="rule" :model="device">
          <FormItem label="售货机名称" prop="deviceName">
            <Input v-model="device.deviceName" :maxlength="120" :minlength="1"></Input>
          </FormItem>
          <FormItem label="售货机编号" prop="deviceNum">
            <Input v-model="device.deviceNum" :maxlength="120" :minlength="1"></Input>
          </FormItem>
          <FormItem label="售货机类型" prop="deviceType">
            <Select style="width:162px" v-model="device.deviceType" placeholder="请选择">
              <Option value="格子机">格子机</Option>
              <Option value="无人销售机">无人销售机</Option>
              <Option value="咖啡机">咖啡机</Option>
              <Option value="饮料机">饮料机</Option>
            </Select>
          </FormItem>
          <FormItem label="所属点位" prop="pointId">
            <Select style="width:162px" v-model="device.pointId" placeholder="请选择">
              <Option :key="index" v-for="item,index in points" :value="item.key">{{ item.value }}</Option>
            </Select>
          </FormItem>
        </Form>
      </div>
      <div slot="footer">
        <Button @click="showEditModal=false">关闭</Button>
        <Button @click="save" type="primary">保存</Button>
      </div>
    </Modal>
  </div>
</template>
<script>
export default {
  methods: {
    create() {
      this.device = {
        pointId: null,
        deviceType: ""
      };
      this.showEditModal = true;
    },
    async save() {
      this.$refs.roleForm.validate(async val => {
        if (val) {
          await this.$store.dispatch({
            type: "device/createOrUpdate",
            data: {
              device: this.device
            }
          });
          this.showEditModal = false;
          await this.getpage();
        }
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
      showEditModal: false,
      rule: {
        deviceName: [
          {
            required: true,
            message: "售货机名称必填",
            trigger: "blur"
          }
        ],
        deviceNum: [
          {
            required: true,
            message: "售货机编号必填",
            trigger: "blur"
          }
        ]
      },
      columns: [
        {
          type: "selection",
          width: 60,
          align: "center"
        },
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
        {
          title: "操作",
          key: "action",
          width: 150,
          render: (h, params) => {
            return h("div", [
              h(
                "Button",
                {
                  props: {
                    type: "primary",
                    size: "small"
                  },
                  style: {
                    marginRight: "5px"
                  },
                  on: {
                    click: () => {
                      this.device = this.devices[params.index];
                      this.showEditModal = true;
                    }
                  }
                },
                "编辑"
              ),
              h(
                "Button",
                {
                  props: {
                    type: "error",
                    size: "small"
                  },
                  on: {
                    click: async () => {
                      this.$Modal.confirm({
                        title: "",
                        content: "删除售货机信息",
                        okText: "是",
                        cancelText: "否",
                        onOk: async () => {
                          await this.$store.dispatch({
                            type: "device/delete",
                            data: this.devices[params.index]
                          });
                          await this.getpage();
                        }
                      });
                    }
                  }
                },
                "删除"
              )
            ]);
          }
        }
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