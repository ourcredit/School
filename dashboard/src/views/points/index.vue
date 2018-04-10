<template>
  <div>
    <Card>
      <p slot="title">点位信息</p>
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
    <Modal v-model="showEditModal" title="编辑点位" @on-ok="save" okText="保存" cancelText="关闭">
      <div>
        <Form inline ref="roleForm" label-position="top" :rules="roleRule" :model="editRole">
          <FormItem label="点位名称" prop="pointName">
            <Input v-model="editRole.pointName" :maxlength="120" :minlength="1"></Input>
          </FormItem>
          <FormItem label="地址" prop="pointAddress">
            <Input v-model="editRole.pointAddress" :maxlength="120" :minlength="1"></Input>
          </FormItem>
           <FormItem label="描述" prop="pointDescription">
            <Input v-model="editRole.pointDescription" :maxlength="120" :minlength="1"></Input>
          </FormItem>
        </Form>
        <baidu-map :center="center" :zoom="15" :scroll-wheel-zoom="true" @ready="handler" class="bm-view">
          <bm-marker @mouseup="showWindows" :position="center" :dragging="true" animation="BMAP_ANIMATION_BOUNCE">
          </bm-marker>
        </baidu-map>
        </baidu-map>
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
      this.editRole = { isActive: true };
      this.showEditModal = true;
    },
    showWindows(e) {
      console.log(e);
      this.editRole.longitude = e.point.lng;
      this.editRole.latitide = e.point.lat;
    },
    async save() {
      this.$refs.roleForm.validate(async val => {
        if (val) {
          await this.$store.dispatch({
            type: "point/createOrUpdate",
            data: { point: this.editRole }
          });
          this.showEditModal = false;
          await this.getpage();
        }
      });
    },
    pageChange(page) {
      this.$store.commit("point/setCurrentPage", page);
      this.getpage();
    },
    pagesizeChange(pagesize) {
      this.$store.commit("point/setPageSize", pagesize);
      this.getpage();
    },
    async getpage() {
      await this.$store.dispatch({
        type: "point/getAll"
      });
    },
    handler({ BMap, map }) {}
  },
  data() {
    return {
      center: { lng: 116.404, lat: 39.915 },
      editRole: {
        isActive: true
      },
      showEditModal: false,
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
          key: "pointName"
        },
        {
          title: "地址",
          key: "pointAddress"
        },
        {
          title: "描述",
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
    roles() {
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
  }
};
</script>
<style lang="less" scoped>
.bm-view {
  width: 100%;
  height: 300px;
}
</style>