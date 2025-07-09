<script setup>
import { ref } from 'vue'

const props = defineProps({
  userCredentials: {
    type: Object,
    required: true,
  },
  roles: {
    type: Array,
    required: true,
  },
})

const editing = ref({
  userName: false,
  email: false,
  phoneNumber: false,
  password: false,
  role: false,
})

const temp = ref({
  userName: props.userCredentials.userName,
  email: props.userCredentials.email,
  phoneNumber: props.userCredentials.phoneNumber,
  newPassword: '',
  confirmPassword: '',
  roleId: props.userCredentials.roleId,
})

const emit = defineEmits(['reset-username', 'reset-email', 'reset-phone-number', 'reset-password', 'reset-role', 'delete-user'])

function startEdit(field) {
  editing.value[field] = true
  temp.value[field] = props.userCredentials[field] || ''
  if (field === 'role') {
    temp.value.roleId = props.userCredentials.roleId
  }
}

function cancelEdit(field) {
  editing.value[field] = false
  temp.value[field] = props.userCredentials[field] || ''
  if (field === 'password') {
    temp.value.password = ''
    temp.value.newPassword = ''
    temp.value.confirmPassword = ''
  }
  if (field === 'role') {
    temp.value.roleId = props.userCredentials.roleId
  }
}

function deleteUser() {
  if (confirm('Bạn có chắc chắn muốn xóa tài khoản này không? Hành động này không thể hoàn tác!')) {
    emit('delete-user')
  }
}

function saveUserName() {
  emit('reset-username', { userName: temp.value.userName })
  editing.value.userName = false
}

function saveEmail() {
  emit('reset-email', { email: temp.value.email })
  editing.value.email = false
}

function savePhoneNumber() {
  emit('reset-phone-number', { phoneNumber: temp.value.phoneNumber })
  editing.value.phoneNumber = false
}

function savePassword() {
  if (!temp.value.password || !temp.value.confirmPassword) {
    alert('Vui lòng nhập đầy đủ thông tin!')
    return
  }
  if (temp.value.password !== temp.value.confirmPassword) {
    alert('Mật khẩu mới không khớp!')
    return
  }
  emit('reset-password', {
    password: temp.value.password,
    confirmPassword: temp.value.confirmPassword,
  })
  editing.value.password = false
  temp.value.password = ''
  temp.value.newPassword = ''
  temp.value.confirmPassword = ''
}

function saveRole() {
  emit('reset-role', { roleId: temp.value.roleId })
  editing.value.role = false
}
</script>

<template>
  <div class="edit-user-container">
    <div class="header">
      <h2>Chỉnh sửa tài khoản nhân viên</h2>
    </div>
    
    <div class="form-container">
      <!-- UserName -->
      <div class="form-group">
        <label class="form-label">Tên đăng nhập</label>
        <div class="form-field">
          <template v-if="editing.userName">
            <div class="edit-mode">
              <input 
                v-model="temp.userName" 
                class="form-input"
                placeholder="Nhập tên đăng nhập"
              />
              <div class="button-group">
                <button @click="saveUserName" class="btn btn-save">Lưu</button>
                <button @click="cancelEdit('userName')" class="btn btn-cancel">Hủy</button>
              </div>
            </div>
          </template>
          <template v-else>
            <div class="view-mode">
              <span class="field-value">{{ props.userCredentials.userName }}</span>
              <button @click="startEdit('userName')" class="btn btn-edit">Chỉnh sửa</button>
            </div>
          </template>
        </div>
      </div>

      <!-- Email -->
      <div class="form-group">
        <label class="form-label">Email</label>
        <div class="form-field">
          <template v-if="editing.email">
            <div class="edit-mode">
              <input 
                v-model="temp.email" 
                type="email" 
                class="form-input"
                placeholder="Nhập email"
              />
              <div class="button-group">
                <button @click="saveEmail" class="btn btn-save">Lưu</button>
                <button @click="cancelEdit('email')" class="btn btn-cancel">Hủy</button>
              </div>
            </div>
          </template>
          <template v-else>
            <div class="view-mode">
              <span class="field-value">{{ props.userCredentials.email }}</span>
              <button @click="startEdit('email')" class="btn btn-edit">Chỉnh sửa</button>
            </div>
          </template>
        </div>
      </div>

      <!-- Phone Number -->
      <div class="form-group">
        <label class="form-label">Số điện thoại</label>
        <div class="form-field">
          <template v-if="editing.phoneNumber">
            <div class="edit-mode">
              <input 
                v-model="temp.phoneNumber" 
                type="tel" 
                class="form-input"
                placeholder="Nhập số điện thoại"
              />
              <div class="button-group">
                <button @click="savePhoneNumber" class="btn btn-save">Lưu</button>
                <button @click="cancelEdit('phoneNumber')" class="btn btn-cancel">Hủy</button>
              </div>
            </div>
          </template>
          <template v-else>
            <div class="view-mode">
              <span class="field-value">{{ props.userCredentials.phoneNumber }}</span>
              <button @click="startEdit('phoneNumber')" class="btn btn-edit">Chỉnh sửa</button>
            </div>
          </template>
        </div>
      </div>

      <!-- Role -->
      <div class="form-group">
        <label class="form-label">Vai trò</label>
        <div class="form-field">
          <template v-if="editing.role">
            <div class="edit-mode">
              <select 
                v-model="temp.roleId" 
                class="form-input"
              >
                <option v-for="role in props.roles" :key="role.id" :value="role.id">
                  {{ role.roleName }} - {{ role.description }}
                </option>
              </select>
              <div class="button-group">
                <button @click="saveRole" class="btn btn-save">Lưu</button>
                <button @click="cancelEdit('role')" class="btn btn-cancel">Hủy</button>
              </div>
            </div>
          </template>
          <template v-else>
            <div class="view-mode">
              <span class="field-value">{{ props.userCredentials.roleName }}</span>
              <button @click="startEdit('role')" class="btn btn-edit">Chỉnh sửa</button>
            </div>
          </template>
        </div>
      </div>

      <!-- Password -->
      <div class="form-group">
        <label class="form-label">Mật khẩu</label>
        <div class="form-field">
          <template v-if="editing.password">
            <div class="edit-mode">
              <div class="password-inputs">
                <input 
                  v-model="temp.password" 
                  type="password" 
                  class="form-input"
                  placeholder="Mật khẩu mới" 
                />
                <input 
                  v-model="temp.confirmPassword" 
                  type="password" 
                  class="form-input"
                  placeholder="Xác nhận mật khẩu mới" 
                />
              </div>
              <div class="button-group">
                <button @click="savePassword" class="btn btn-save">Lưu</button>
                <button @click="cancelEdit('password')" class="btn btn-cancel">Hủy</button>
              </div>
            </div>
          </template>
          <template v-else>
            <div class="view-mode">
              <span class="field-value">********</span>
              <button @click="startEdit('password')" class="btn btn-edit">Đổi mật khẩu</button>
            </div>
          </template>
        </div>
      </div>
    </div>

    <div class="danger-zone">
      <h3>Vùng nguy hiểm</h3>
      <p>Xóa tài khoản này sẽ không thể hoàn tác</p>
      <button @click="deleteUser" class="btn btn-danger">Xóa tài khoản</button>
    </div>
  </div>
</template>

<style scoped>
.edit-user-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.header {
  text-align: center;
  margin-bottom: 30px;
  padding-bottom: 20px;
  border-bottom: 2px solid #e5e7eb;
}

.header h2 {
  color: #1f2937;
  font-size: 24px;
  font-weight: 600;
  margin: 0;
}

.form-container {
  margin-bottom: 40px;
}

.form-group {
  margin-bottom: 25px;
}

.form-label {
  display: block;
  font-weight: 600;
  color: #374151;
  margin-bottom: 8px;
  font-size: 14px;
}

.form-field {
  background: #f9fafb;
  border: 1px solid #e5e7eb;
  border-radius: 6px;
  padding: 15px;
}

.view-mode {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.field-value {
  font-size: 16px;
  color: #1f2937;
  font-weight: 500;
}

.edit-mode {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.password-inputs {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.form-input {
  padding: 10px 12px;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  font-size: 14px;
  transition: border-color 0.2s;
  width: 100%;
}

.form-input:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

select.form-input {
  appearance: none;
  background-image: url("data:image/svg+xml;charset=utf-8,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 24 24' stroke='%236b7280'%3E%3Cpath stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M19 9l-7 7-7-7'/%3E%3C/svg%3E");
  background-position: right 0.5rem center;
  background-repeat: no-repeat;
  background-size: 1.5em 1.5em;
  padding-right: 2.5rem;
}

.button-group {
  display: flex;
  gap: 10px;
}

.btn {
  padding: 8px 16px;
  border: none;
  border-radius: 6px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-edit {
  background: #3b82f6;
  color: white;
}

.btn-edit:hover {
  background: #2563eb;
}

.btn-save {
  background: #10b981;
  color: white;
}

.btn-save:hover {
  background: #059669;
}

.btn-cancel {
  background: #6b7280;
  color: white;
}

.btn-cancel:hover {
  background: #4b5563;
}

.danger-zone {
  background: #fef2f2;
  border: 1px solid #fecaca;
  border-radius: 8px;
  padding: 20px;
  text-align: center;
}

.danger-zone h3 {
  color: #dc2626;
  margin: 0 0 10px 0;
  font-size: 18px;
}

.danger-zone p {
  color: #6b7280;
  margin: 0 0 15px 0;
  font-size: 14px;
}

.btn-danger {
  background: #dc2626;
  color: white;
  padding: 10px 20px;
  font-size: 14px;
  font-weight: 600;
}

.btn-danger:hover {
  background: #b91c1c;
}

@media (max-width: 768px) {
  .edit-user-container {
    margin: 10px;
    padding: 15px;
  }
  
  .view-mode {
    flex-direction: column;
    align-items: flex-start;
    gap: 10px;
  }
  
  .button-group {
    flex-direction: column;
    width: 100%;
  }
  
  .btn {
    width: 100%;
  }
}
</style>