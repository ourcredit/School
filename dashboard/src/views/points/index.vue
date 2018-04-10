<template>
  <div>
    <Card>
      <p slot="title">点位信息</p>
      <Row slot="extra">
        <i-col span="12">
        </i-col>
        <i-col span="12">
         <i-button @click="create"  type="primary">添加</i-button>
        </i-col>
      </Row>
      <Table :columns="columns" border :data="roles"></Table>
      <Page :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize"
        :current="currentPage"></Page>
    </Card>
    <Modal v-model="showModal" title="添加点位" @on-ok="save" okText="保存" cancelText="关闭">
      <div>
        <Form  inline ref="newRoleForm" label-position="top" :rules="newRoleRule" :model="editRole">
          <FormItem label="点为名称" prop="pointName">
            <Input v-model="editRole.pointName" :maxlength="120" :minlength="1"></Input>
          </FormItem>
          <FormItem label="描述" prop="pointDescription">
            <Input v-model="editRole.pointDescription" :maxlength="120" :minlength="2"></Input>
          </FormItem>
        </Form>
      </div>
      <div slot="footer">
        <Button @click="showModal=false">关闭</Button>
        <Button @click="save" type="primary">保存</Button>
      </div>
    </Modal>
    <Modal v-model="showEditModal" title="编辑点位" @on-ok="save" okText="保存" cancelText="关闭">
      <div>
        <Form ref="roleForm" label-position="top" :rules="roleRule" :model="editRole">
          <FormItem label="点位名称" prop="name">
            <Input v-model="editRole.name" :maxlength="120" :minlength="1"></Input>
          </FormItem>
          <FormItem label="描述" prop="displayName">
            <Input v-model="editRole.displayName" :maxlength="120" :minlength="1"></Input>
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
      this.editRole = {
        isActive: true
      };
      this.showModal = true;
    },
    async save() {
      if (!!this.editRole.id) {
        this.$refs.roleForm.validate(async val => {
          if (val) {
            await this.$store.dispatch({
              type: "role/update",
              data: this.editRole
            });
            this.showEditModal = false;
            await this.getpage();
          }
        });
      } else {
        this.$refs.newRoleForm.validate(async val => {
          if (val) {
            await this.$store.dispatch({
              type: "role/create",
              data: this.editRole
            });
            this.showModal = false;
            await this.getpage();
          }
        });
      }
    },
    pageChange(page) {
      this.$store.commit("role/setCurrentPage", page);
      this.getpage();
    },
    pagesizeChange(pagesize) {
      this.$store.commit("role/setPageSize", pagesize);
      this.getpage();
    },
    async getpage() {
      await this.$store.dispatch({
        type: "role/getAll"
      });
    }
  },
  data() {
    return {
      editRole: {},
      showModal: false,
      showEditModal: false,
      newRoleRule: {
        pointName: [
          {
            required: true,
            message: "点位名称必填",
            trigger: "blur"
          }
        ]
      },
      roleRule: {
        pointName: [
          {
            required: true,
            message: "点位名称必填",
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
          title: "点位名称",
          key: "name"
        },
        {
          title: "地址",
          key: "displayName"
        },
        {
          title: "描述",
          key: "displayName"
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
                        title: this.L(""),
                        content: this.L("Delete role"),
                        okText: this.L("Yes"),
                        cancelText: this.L("No"),
                        onOk: async () => {
                          await this.$store.dispatch({
                            type: "role/delete",
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
    roles() {
      return this.$store.state.role.roles;
    },
    permissions() {
      return this.$store.state.role.permissions;
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
    await this.$store.dispatch({
      type: "role/getAllPermissions"
    });
  }
};
</script>