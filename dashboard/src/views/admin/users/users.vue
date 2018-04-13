<template>
    <div>
        <Card>
            <p slot="title">用户信息</p>
            <Row slot="extra">
                <i-col span="12">
                </i-col>
                <i-col span="12">
                    <i-button @click="create" type="primary">添加</i-button>
                </i-col>
            </Row>
            <Table :columns="columns" border :data="users"></Table>
            <Page :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize"
                :current="currentPage"></Page>
        </Card>
        <Modal v-model="showModal" title="添加用户" @on-ok="save" okText="保存" cancelText="关闭">
            <div>
                <Form ref="userForm" label-position="top" :rules="newUserRule" :model="editUser">
                    <Tabs value="detail">
                        <TabPane label="用户信息" name="detail">
                            <FormItem label="用户名" prop="userName">
                                <Input v-model="editUser.userName" :maxlength="32" :minlength="2"></Input>
                            </FormItem>
                            <FormItem label="姓名" prop="name">
                                <Input v-model="editUser.name" :maxlength="32"></Input>
                            </FormItem>
                            <FormItem v-if="!isedit" label="密码" prop="password">
                                <Input v-model="editUser.password" type="password" :maxlength="32"></Input>
                            </FormItem>
                            <FormItem v-if="!isedit" label="确认密码" prop="confirmPassword">
                                <Input v-model="editUser.confirmPassword" type="password" :maxlength="32"></Input>
                            </FormItem>
                            <FormItem>
                                <Checkbox v-model="editUser.isActive">是否启用</Checkbox>
                            </FormItem>
                        </TabPane>
                        <TabPane label="用户角色" name="roles">
                             <CheckboxGroup >
                                <Checkbox v-model="role.isAssigned" :label="role.roleDisplayName" v-for="role in userRoles" :key="role.id">
                                    <span>{{role.roleDisplayName}}</span>
                                </Checkbox>
                            </CheckboxGroup>
                        </TabPane>
                            <TabPane label="用户机构" name="orgs">
                              <Tree @on-select-change="change" :data="orgs"></Tree>
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
    async change(data) {
      this.editUser.treeCode = data[0].treeCode;
    },
    async create() {
      this.isedit = false;
      await this.$store.dispatch({
        type: "user/getUser"
      });
      this.editUser = this.user.user;
      this.userRoles = this.user.roles;
      this.showModal = true;
    },
    async save() {
      this.$refs.userForm.validate(async val => {
        if (val) {
          let arr = this.userRoles
            .filter(c => c.isAssigned)
            .map(w => w.roleName);
          await this.$store.dispatch({
            type: "user/createOrUpdate",
            data: { user: this.editUser, assignedRoleNames: arr }
          });
          this.showModal = false;
          await this.getpage();
        }
      });
    },
    pageChange(page) {
      this.$store.commit("user/setCurrentPage", page);
      this.getpage();
    },
    pagesizeChange(pagesize) {
      this.$store.commit("user/setPageSize", pagesize);
      this.getpage();
    },
    async getpage() {
      await this.$store.dispatch({
        type: "user/getAll"
      });
    }
  },
  data() {
    const validatePassCheck = (rule, value, callback) => {
      if (!value) {
        callback(new Error("请确认你的密码"));
      } else if (value !== this.editUser.password) {
        callback(new Error("两次密码不一致"));
      } else {
        callback();
      }
    };
    return {
      editUser: {},
      isedit: false,
      showModal: false,
      newUserRule: {
        userName: [
          {
            required: true,
            message: "用户名必填",
            trigger: "blur"
          }
        ],
        name: [
          {
            required: true,
            message: "姓名必填",
            trigger: "blur"
          }
        ],
        password: [
          {
            required: true,
            message: "密码必填",
            trigger: "blur"
          }
        ],
        confirmPassword: {
          validator: validatePassCheck,
          trigger: "blur"
        }
      },
      userRoles: [],
      columns: [
        {
          title: "用户名",
          key: "userName"
        },
        {
          title: "姓名",
          key: "name"
        },
        {
          title: "角色",
          key: "name",
          render: (h, p) => {
            if (p.row.roles) {
              var t = "";
              p.row.roles.forEach(w => {
                t += w.roleName + " ";
              });
              return t;
            }
          }
        },
        {
          title: "是否启用",
          render: (h, params) => {
            return h("Checkbox", {
              props: {
                value: this.users[params.index].isActive,
                disabled: true
              }
            });
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
                    click: async () => {
                      await this.$store.dispatch({
                        type: "user/getUser",
                        data: params.row.id
                      });
                      this.editUser = this.user.user;
                      this.userRoles = this.user.roles;
                      this.isedit = true;
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
                        content: "删除用户",
                        okText: "是",
                        cancelText: "否",
                        onOk: async () => {
                          await this.$store.dispatch({
                            type: "user/delete",
                            data: this.users[params.index]
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
    users() {
      return this.$store.state.user.users;
    },
    user() {
      return this.$store.state.user.user;
    },
    totalCount() {
      return this.$store.state.user.totalCount;
    },
    currentPage() {
      return this.$store.state.user.currentPage;
    },
    pageSize() {
      return this.$store.state.user.pageSize;
    }
  },
  async created() {
    this.getpage();
    await this.$store.dispatch({
      type: "user/getRoles"
    });
    await this.$store.dispatch({
      type: "org/getAll"
    });
  }
};
</script>