<style lang="less">
  @import "./login.less";
</style>

<template>
  <div class="login" @keydown.enter="handleSubmit">
    <div class="login-con">
      <Card :bordered="false">

        <div class="form-con">
          <Form ref="loginForm" :model="form" :rules="rules">
            <FormItem prop="userNameOrEmailAddress">
              <Input v-model="form.userNameOrEmailAddress" placeholder="用户名或邮箱">
              <span slot="prepend">
                <Icon :size="16" type="person"></Icon>
              </span>
              </Input>
            </FormItem>
            <FormItem prop="password">
              <Input type="password" v-model="form.password" placeholder="密码">
              <span slot="prepend">
                <Icon :size="14" type="locked"></Icon>
              </span>
              </Input>
            </FormItem>
            <div style="margin-bottom:10px">
              <Checkbox v-model="form.isAdmin">超管登陆</Checkbox>
            </div>
            <FormItem>
              <Button @click="handleSubmit" type="primary" long>登入</Button>
            </FormItem>
          </Form>
        </div>
      </Card>
    </div>
  </div>
</template>

<script>
  import Cookies from "js-cookie";
  export default {
    data() {
      return {
        languages: [],
        currentLanguage: {},
        form: {
          userNameOrEmailAddress: "",
          password: "",
          isAdmin: false
        },
        rules: {
          userNameOrEmailAddress: [{
            required: true,
            message: "字段必填",
            trigger: "blur"
          }],
          password: [{
            required: true,
            message: "字段必填",
            trigger: "blur"
          }]
        }
      };
    },
    methods: {
      async handleSubmit() {
        this.$refs.loginForm.validate(async valid => {
          if (valid) {
            this.$Message.loading({
              content: "请稍等",
              duration: 0
            });

            let self = this;
            var type = self.form.isAdmin ? "user/superlogin" : "user/login";
            await this.$store
              .dispatch({
                type: type,
                data: self.form
              })
              .then(
                response => {
                  Cookies.set(
                    "userNameOrEmailAddress",
                    self.form.userNameOrEmailAddress
                  );
                  location.reload();
                },
                error => {
                  this.$Modal.error({
                    title: "",
                    content: "登陆失败 !"
                  });
                  this.$Message.destroy();
                }
              );
          }
        });
      }
    },
    created() {},
    computed: {
      tenant() {
        return this.$store.state.session.tenant;
      }
    }
  };
</script>

<style>
  .language-ul {
    text-align: center;
  }

  .language-ul li {
    display: inline;
    margin: 2px;
  }

  .famfamfam-flags {
    display: inline-block;
  }
</style>