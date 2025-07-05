<script setup>
const props = defineProps({
  roles: {
    type: Array,
    required: true,
  },
  form: {
    type: Object,
    required: true,
  },
})
const emit = defineEmits(['submit'])
function onSubmit() {
  emit('submit', props.form)
}
</script>
<template>
  <h1>Tạo tài khoản người dùng</h1>
  <form @submit.prevent="onSubmit">
    <div>
      <label>UserName:</label>
      <input v-model="form.userName" type="text" placeholder="Nhập UserName" />
    </div>
    <div>
      <option value="">Chọn quyền hạn</option>
      <select v-model="form.roleId">
        <option v-for="result in roles" :key="result.id" :value="result.roleName">
          {{ result.roleName }}
        </option>
      </select>
    </div>
    <div>
      <label>Password:</label>
      <input v-model="form.passWord" type="password" placeholder="Nhập mật khẩu" />
    </div>
    <div>
        <label>Xác nhận mật khẩu</label>
        <input v-model="form.confirmPassword" type="password" placeholder="xác nhận mật khẩu"/>
    </div>
    <div>
        <label>Số điện thoại: </label>
        <input v-model="form.phoneNumber" type="text" placeholder="Nhập số điện thoại"/>
    </div>
    <div>
        <label>Email: </label>
        <input v-model="form.email" type="email" placeholder="Nhập email"/>
    </div>
    <button type="submit">Tạo tài khoản</button>
  </form>
</template>
<style></style>
