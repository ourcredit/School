<template>
  <div>
    <Card>
      <p slot="title">设备管理</p>
      <Row slot="extra">
        <i-col span="12">
        </i-col>
        <i-col span="12">
          <i-button @click="create" type="primary">添加</i-button>
        </i-col>
      </Row>
      <Table :columns="columns" border :data="roles"></Table>
      <Page :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize"
        :current="currentPage"></Page>
    </Card>
    <Modal v-model="showEditModal" title="编辑设备" @on-ok="save" okText="保存" cancelText="关闭">
      <div>
        <Form inline ref="roleForm" label-position="top" :rules="rule" :model="device">
          <FormItem label="设备名称" prop="deviceName">
            <Input v-model="device.deviceName" :maxlength="120" :minlength="1"></Input>
          </FormItem>
          <FormItem label="设备编号" prop="deviceNum">
            <Input v-model="device.deviceNum" :maxlength="120" :minlength="1"></Input>
          </FormItem>
           <FormItem label="设备类型" prop="deviceType">
              <Form-item label="选择器">
            <i-select :model.sync="device.deviceType" placeholder="请选择">
                <i-option value="beijing">A</i-option>
                <i-option value="shanghai">B</i-option>
                <i-option value="shenzhen">C</i-option>
                <i-option value="shenzhen">D</i-option>
            </i-select>
        </Form-item>
          </FormItem>
             <FormItem label="所属点位" prop="pointId">
             <i-select :model.sync="device.deviceType" placeholder="请选择">
                <i-option :key="index" v-for="item in points" :value="item.key">{{ item.value }}</i-option>
            </i-select>
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
      this.device = { isActive: true };
      this.showEditModal = true;
    },
    async save() {
      this.$refs.roleForm.validate(async val => {
        if (val) {
          await this.$store.dispatch({
            type: "device/createOrUpdate",
            data: { device: this.editRole }
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
        type: "device/getAll"
      });
    }
  },
  data() {
    return {
      device: {},
      showEditModal: false,
      rule: {
        deviceName: [
          {
            required: true,
            message: "设备名称必填",
            trigger: "blur"
          }
        ],
        deviceNum: [
          {
            required: true,
            message: "设备编号必填",
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
          title: "设备名称",
          key: "pointName"
        },
        {
          title: "设备编号",
          key: "pointAddress"
        },
        {
          title: "设备类型",
          key: "pointDescription"
        },
        {
          title: "所属点位",
          key: "pointDescription"
        },
        {
          title: "创建人",
          key: "pointDescription"
        },
        {
          title: "创建时间",
          key: "pointDescription"
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
                      this.editRole = this.roles[params.index];
                      this.center = {
                        lng: this.editRole.longitude,
                        lat: this.editRole.latitide
                      };
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
                        content: "删除角色",
                        okText: "是",
                        cancelText: "否",
                        onOk: async () => {
                          await this.$store.dispatch({
                            type: "point/delete",
                            data: this.roles[params.index]
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
    device() {
      return this.$store.state.point.devices;
    },
    points() {
      return this.$store.state.point.points;
    },
    totalCount() {
      return this.$store.state.point.totalCount;
    },
    currentPage() {
      return this.$store.state.point.currentPage;
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