<template>
  <div>
    <Card>
      <p slot="title">角色信息</p>
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

    <Modal v-model="showModal" title="添加角色" @on-ok="save" okText="保存" cancelText="关闭">
      <div>
        <Form ref="newRoleForm" label-position="top" :rules="RoleRule" :model="editRole">
          <Tabs value="detail">
            <TabPane label="角色信息" name="detail">
              <FormItem label="角色名称" prop="name">
                <Input v-model="editRole.name" :maxlength="32" :minlength="2"></Input>
              </FormItem>
              <FormItem label="显示名" prop="displayName">
                <Input v-model="editRole.displayName" :maxlength="32" :minlength="2"></Input>
              </FormItem>
              <FormItem label="角色描述" prop="description">
                <Input v-model="editRole.description"></Input>
              </FormItem>
            </TabPane>
            <TabPane label="权限信息" name="roles">
               <!-- <Tree show-checkbox :data="orgs"></Tree> -->
              <FormItem label="权限树">
                <CheckboxGroup v-model="editRole.permissions">
                  <Checkbox :label="permission.name" v-for="permission in permissions" :key="permission.name">
                    <span>{{permission.displayName}}</span>
                  </Checkbox>
                </CheckboxGroup>
              </FormItem>
            </TabPane>
          </Tabs>
        </Form>
      </div>
      <div slot="footer">
        <Button @click="showModal=false">关闭</Button>
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
      RoleRule: {
        name: [
          {
            required: true,
            message: "角色名必填",
            trigger: "blur"
          }
        ],
        displayName: [
          {
            required: true,
            message: "显示名必填",
            trigger: "blur"
          }
        ]
      },
      columns: [
        {
          title: "角色名",
          key: "name"
        },
        {
          title: "显示名",
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
                      console.log(this.editRole);
                      this.showModal = true;
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
                        content: "确定要删除角色么",
                        okText: "是",
                        cancelText: "否",
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