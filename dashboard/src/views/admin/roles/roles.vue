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
    </Card>

    <Modal v-model="showModal" title="添加角色" @on-ok="save" okText="保存" cancelText="关闭">
      <div>
        <Form ref="newRoleForm" label-position="top" :rules="RoleRule" :model="editRole">
          <Tabs value="detail">
            <TabPane label="角色信息" name="detail">
              <FormItem label="显示名" prop="displayName">
                <Input v-model="editRole.displayName" :maxlength="32" :minlength="2"></Input>
              </FormItem>
              <FormItem label="角色描述" prop="description">
                <Input v-model="editRole.description"></Input>
              </FormItem>
            </TabPane>
            <TabPane label="权限信息" name="roles">
               <Tree ref="tree" multiple show-checkbox :data="tree"></Tree>
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
    async create() {
      await this.$store.dispatch({
        type: "role/getRole"
      });
      this.editRole = this.role.role;
      this.permissions = this.role.permissions;
      this.tree = this.$permissions(
        this.role.permissions,
        null,
        "parentName",
        this.role.grantedPermissionNames
      );
      this.showModal = true;
    },
    async save() {
      var nodes = this.$refs.tree.getCheckedNodes();
      var result = [];
      nodes.forEach(c => {
        this.$depthNode(this.permissions, c, result);
      });
      this.$refs.newRoleForm.validate(async val => {
        if (val) {
          await this.$store.dispatch({
            type: "role/createOrUpdate",
            data: {
              role: this.editRole,
              grantedPermissionNames: new Set(result)
            }
          });
          this.showModal = false;
          await this.getpage();
        }
      });
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
      permissions: [],
      tree: [],
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
                    click: async () => {
                      await this.$store.dispatch({
                        type: "role/getRole",
                        data: params.row.id
                      });
                      this.editRole = this.role.role;
                      this.permissions = this.role.permissions;
                      this.tree = this.$permissions(
                        this.role.permissions,
                        null,
                        "parentName",
                        this.role.grantedPermissionNames
                      );
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
                            data: params.row.id
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
    role() {
      return this.$store.state.role.role;
    }
  },
  async created() {
    this.getpage();
  }
};
</script>