<template>
  <div>
    <Row :gutter="16">
      <Col span="8">
      <Card>
        <p slot="title">机构信息</p>
        <Row slot="extra">
          <i-col  span="12">
            <Button @click="create" type="primary" shape="circle" icon="plus"></Button>
          </i-col>
          <i-col  span="12">
              <Button @click="remove" type="primary" shape="circle" icon="close"></Button>
          </i-col>
        </Row>
        <Tree @on-select-change="change" :data="baseData"></Tree>
      </Card>
      </Col>
      <Col span="15">
      <Card>
        <p slot="title">角色信息</p>
        <Dropdown slot="extra" @on-click="handleClickActionsDropdown">
          <a href="javascript:void(0)">
            操作
            <Icon type="android-more-vertical"></Icon>
          </a>
          <DropdownMenu slot="list">
            <DropdownItem name='Refresh'>刷新</DropdownItem>
            <DropdownItem name='Create'>创建</DropdownItem>
          </DropdownMenu>
        </Dropdown>
        <Table :columns="columns" border :data="roles"></Table>
        <Page :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize"
          :current="currentPage"></Page>
      </Card>
      </Col>
    </Row>
    <Modal v-model="showModal" title="添加机构" @on-ok="save" okText="保存" cancelText="关闭">
      <div>
        <Form ref="newRoleForm" label-position="top" :rules="orgRule" :model="org">
          <FormItem label="上级机构" >
            <Input v-model="org.parentName" disabled ></Input>
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
    
  </div>
</template>
<script>
export default {
  methods: {
    change(data) {
      if (data.length == 1) {
        this.parent = { parentId: data[0].id, parentName: data[0].title };
        this.org.parentName = this.parent.parentName;
      } else {
        this.parent = null;
        delete this.org.parentName;
      }
    },
    remove() {
      if (!this.parent) return;
      this.$Modal.confirm({
        title: this.parent.parentName,
        content: "确定要删除么",
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
    },
    create() {
      const p =
        this.parent && this.parent.parentId ? this.parent.parentId : null;
      this.org = {
        parentId: p
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
      this.$store.commit("org/setCurrentPage", page);
      this.getpage();
    },
    pagesizeChange(pagesize) {
      this.$store.commit("org/setPageSize", pagesize);
      this.getpage();
    },
    async getpage() {
      await this.$store.dispatch({
        type: "org/getAll"
      });
    },
    handleClickActionsDropdown(name) {
      if (name === "Create") {
        this.create();
      } else if (name === "Refresh") {
        this.getpage();
      }
    }
  },
  data() {
    return {
      parent: null,
      baseData: [
        {
          id: 1,
          title: "parent 1",
          children: [
            {
              title: "parent 1-0",
              children: [
                {
                  title: "leaf"
                },
                {
                  title: "leaf"
                }
              ]
            },
            {
              title: "parent 1-1",
              children: [
                {
                  title: '<span style="color: red">leaf</span>'
                }
              ]
            }
          ]
        }
      ],
      org: { parentId: null },
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
    orgs() {
      return this.$store.state.org.orgs;
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
      type: "org/getAll"
    });
  }
};
</script>