<template>
  <div>
    <Row :gutter="16">
      <Col span="8">
      <Card>
        <p slot="title">机构信息</p>
        <Row slot="extra">
          <i-col span="12">
            <Button @click="create" type="primary" shape="circle" icon="plus"></Button>
          </i-col>
          <i-col span="12">
            <Button @click="remove" type="primary" shape="circle" icon="close"></Button>
          </i-col>
        </Row>
        <Tree @on-select-change="change" :data="orgs"></Tree>
      </Card>
      </Col>
      <Col span="15">
      <Card>
        <p slot="title">售货机信息</p>
        <Row :gutter="8" slot="extra">
          <i-col span="8">
            <Input placeholder="售货机名" v-model="params.name">
            </Input>
          </i-col>
            <i-col span="8">
            <Input placeholder="售货机编号" v-model="params.num">
            </Input>
          </i-col>
          <i-col span="3">
            <i-button @click="getpage" type="primary">查询</i-button>
          </i-col>
          <i-col span="3">
            <i-button v-if="this.parent!=null&&this.parent.length==6" @click="bind" type="primary">绑定</i-button>
          </i-col>
        </Row>
        <Table :columns="columns" border :data="devices"></Table>
        <Page :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize"
          :current="currentPage"></Page>
      </Card>
      </Col>
    </Row>
    <Modal v-model="showModal" title="添加机构" @on-ok="save" okText="保存" cancelText="关闭">
      <div>
        <Form ref="orgForm" label-position="top" :rules="orgRule" :model="org">
          <FormItem label="上级机构">
            <Input v-model="org.parentName" disabled></Input>
          </FormItem>
          <FormItem label="机构名" prop="treeName">
            <Input v-model="org.treeName" :maxlength="32" :minlength="1"></Input>
          </FormItem>
        </Form>
      </div>
      <div slot="footer">
        <Button @click="showModal=false">关闭</Button>
        <Button @click="save" type="primary">保存</Button>
      </div>
    </Modal>

    <Modal v-model="bindModal" title="绑定售货机" @on-ok="bindDevice" okText="保存" cancelText="关闭">
      <div>
        <bind-form @cc.sync="setValues"></bind-form>
      </div>
      <div slot="footer">
        <Button @click="bindModal=false">关闭</Button>
        <Button @click="bindDevice" type="primary">保存</Button>
      </div>
    </Modal>
  </div>
</template>
<script>
import BindForm from "./bind";
export default {
  components: {
    BindForm
  },
  methods: {
    async change(data) {
      if (data.length == 1) {
        this.parent = {
          parentId: data[0].id,
          parentName: data[0].title,
          length: data[0].treeLength
        };
        this.org.parentName = this.parent.parentName;
        await this.getpage();
      } else {
        this.parent = null;
        delete this.org.parentName;
      }
    },
    remove() {
      if (!this.parent) {
        this.$Message.warning("请选择机构进行操作");
        return;
      }
      if (this.parent.length == 1) {
        this.$Message.warning("根节点不可删除");
        return;
      }
      this.$Modal.confirm({
        title: this.parent.parentName,
        content: "确定要删除么",
        okText: "是",
        cancelText: "否",
        onOk: async () => {
          await this.$store.dispatch({
            type: "org/delete",
            data: this.parent.parentId
          });
          this.parent = null;
          await this.init();
        }
      });
    },
    create() {
      const Id =
        this.parent && this.parent.parentId ? this.parent.parentId : null;
      const Name =
        this.parent && this.parent.parentName ? this.parent.parentName : "";
      const length = this.parent && this.parent.length ? this.parent.length : 0;
      if (length >= 6) {
        this.$Message.warning("层级过多");
        return;
      }
      this.org = {
        parentId: Id,
        parentName: Name
      };
      this.showModal = true;
    },
    async save() {
      this.$refs.orgForm.validate(async val => {
        if (val) {
          await this.$store.dispatch({
            type: "org/createOrUpdate",
            data: {
              operatorTree: this.org
            }
          });
          this.showModal = false;
          this.parent = null;
          await this.init();
        }
      });
    },
    async init() {
      await this.$store.dispatch({
        type: "org/getAll"
      });
    },
    setValues(data) {
      this.selectModels = data;
    },
    bind() {
      this.bindModal = true;
    },
    async bindDevice() {
      if (!this.parent || !this.parent.parentId) {
        abp.message.warn("请先选择机构");
        this.bindModal = false;
        return;
      }
      let des = Array.from(this.selectModels, c => c.id);
      if (!des || des.length <= 0) {
        abp.message.warn("请选择售货机");
        return;
      }
      const params = {
        orgId: this.parent.parentId,
        devices: des
      };
      await this.$store.dispatch({
        type: "device/bindDevice",
        data: params
      });
      this.bindModal = false;
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
      if (!this.parent || !this.parent.parentId) return;
      await this.$store.dispatch({
        type: "device/getOrgDevices",
        parentId: this.parent.parentId,
        name: this.params.name,
        num: this.params.num
      });
    }
  },
  data() {
    return {
      params: {
        name: "",
        num: ""
      },
      selectModels: [],
      bindModal: false,
      parent: null,
      org: {
        parentId: null
      },
      showModal: false,
      orgRule: {
        treeName: [
          {
            required: true,
            message: "机构名必填",
            trigger: "blur"
          }
        ]
      },
      columns: [
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
                      this.$store.commit(
                        "device/setCurrentCode",
                        params.row.deviceNum
                      );
                      this.$router.push({
                        name: "boxs"
                      });
                    }
                  }
                },
                "详情"
              ),
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
                      this.$store.commit("device/setCurrent", params.row.id);
                      //  this.editRole = this.devices[params.index];
                      this.$router.push({
                        name: "settings"
                      });
                    }
                  }
                },
                "配置"
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
                            type: "device/deleteRelation",
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
    orgs() {
      let orgs = this.$store.state.org.orgs;
      return this.$tree(orgs, null, "parentId");
    },
    devices() {
      return this.$store.state.device.orgdevices;
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