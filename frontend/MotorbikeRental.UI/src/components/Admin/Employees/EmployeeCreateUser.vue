<script setup>
import { ref } from 'vue'

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
const showPassword = ref(false)
const showConfirmPassword = ref(false)

function onSubmit() {
  emit('submit', {
    ...props.form,
    roleId: Number(props.form.roleId),
  })
}

function togglePasswordVisibility() {
  showPassword.value = !showPassword.value
}

function toggleConfirmPasswordVisibility() {
  showConfirmPassword.value = !showConfirmPassword.value
}
</script>

<template>
  <div class="create-user-page">
    <!-- Header -->
    <div class="page-header">
      <div class="header-content">
        <h1 class="page-title">
          <i class="title-icon">👤</i>
          Tạo tài khoản người dùng
        </h1>
        <p class="page-subtitle">Tạo tài khoản đăng nhập cho nhân viên</p>
      </div>
    </div>

    <!-- Form -->
    <div class="form-container">
      <form class="user-form" @submit.prevent="onSubmit">
        <div class="form-content">
          <!-- Account Info Section -->
          <div class="form-section">
            <h3 class="section-title">
              <i class="section-icon">🔑</i>
              Thông tin tài khoản
            </h3>
            <div class="form-grid">
              <div class="form-group">
                <label class="form-label">
                  <i class="label-icon">👤</i>
                  Tên đăng nhập
                  <span class="required">*</span>
                </label>
                <input 
                  v-model="form.userName" 
                  type="text" 
                  class="form-input"
                  placeholder="Nhập tên đăng nhập"
                  required
                />
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="label-icon">🛡️</i>
                  Quyền hạn
                  <span class="required">*</span>
                </label>
                <div class="select-wrapper">
                  <select v-model="form.roleId" class="form-select" required>
                    <option value="">Chọn quyền hạn</option>
                    <option v-for="result in roles" :key="result.id" :value="result.id">
                      {{ result.roleName }}
                    </option>
                  </select>
                  <i class="select-arrow">⌄</i>
                </div>
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="label-icon">🔒</i>
                  Mật khẩu
                  <span class="required">*</span>
                </label>
                <div class="password-wrapper">
                  <input 
                    v-model="form.password" 
                    :type="showPassword ? 'text' : 'password'" 
                    class="form-input password-input"
                    placeholder="Nhập mật khẩu"
                    required
                  />
                  <button 
                    type="button" 
                    class="password-toggle"
                    @click="togglePasswordVisibility"
                  >
                    <i v-if="showPassword">🙈</i>
                    <i v-else>👁️</i>
                  </button>
                </div>
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="label-icon">🔐</i>
                  Xác nhận mật khẩu
                  <span class="required">*</span>
                </label>
                <div class="password-wrapper">
                  <input 
                    v-model="form.confirmPassword" 
                    :type="showConfirmPassword ? 'text' : 'password'" 
                    class="form-input password-input"
                    placeholder="Xác nhận mật khẩu"
                    required
                  />
                  <button 
                    type="button" 
                    class="password-toggle"
                    @click="toggleConfirmPasswordVisibility"
                  >
                    <i v-if="showConfirmPassword">🙈</i>
                    <i v-else>👁️</i>
                  </button>
                </div>
              </div>
            </div>
          </div>

          <!-- Contact Info Section -->
          <div class="form-section">
            <h3 class="section-title">
              <i class="section-icon">📞</i>
              Thông tin liên hệ
            </h3>
            <div class="form-grid">
              <div class="form-group">
                <label class="form-label">
                  <i class="label-icon">📱</i>
                  Số điện thoại
                  <span class="required">*</span>
                </label>
                <input 
                  v-model="form.phoneNumber" 
                  type="tel" 
                  class="form-input"
                  placeholder="Nhập số điện thoại"
                  required
                />
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="label-icon">📧</i>
                  Email
                  <span class="required">*</span>
                </label>
                <input 
                  v-model="form.email" 
                  type="email" 
                  class="form-input"
                  placeholder="Nhập địa chỉ email"
                  required
                />
              </div>
            </div>
          </div>
        </div>

        <!-- Form Actions -->
        <div class="form-actions">
          <button type="submit" class="btn btn-primary">
            <i class="btn-icon">✅</i>
            Tạo tài khoản
          </button>
          <button type="button" class="btn btn-secondary" @click="$router.back()">
            <i class="btn-icon">❌</i>
            Hủy bỏ
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.create-user-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%);
  padding: 2rem;
}

/* Page Header */
.page-header {
  margin-bottom: 2rem;
}

.header-content {
  background: white;
  padding: 2rem;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  text-align: center;
}

.page-title {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.75rem;
  font-size: 2rem;
  font-weight: 700;
  color: #1e293b;
  margin: 0 0 0.5rem 0;
}

.title-icon {
  font-size: 2.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.page-subtitle {
  color: #64748b;
  font-size: 1.125rem;
  margin: 0;
}

/* Form Container */
.form-container {
  max-width: 800px;
  margin: 0 auto;
}

.user-form {
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  overflow: hidden;
}

/* Form Content */
.form-content {
  padding: 2.5rem;
}

.form-section {
  margin-bottom: 2.5rem;
}

.form-section:last-child {
  margin-bottom: 0;
}

.section-title {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 1.375rem;
  font-weight: 600;
  color: #374151;
  margin: 0 0 1.5rem 0;
  padding-bottom: 0.75rem;
  border-bottom: 2px solid #f3f4f6;
}

.section-icon {
  font-size: 1.75rem;
  background: linear-gradient(135deg, #10b981 0%, #059669 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 2rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.form-label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 600;
  color: #374151;
  font-size: 0.9rem;
}

.label-icon {
  font-size: 1.1rem;
}

.required {
  color: #ef4444;
  font-weight: 700;
}

.form-input {
  width: 100%;
  padding: 1rem 1.25rem;
  border: 2px solid #e5e7eb;
  border-radius: 12px;
  font-size: 0.9rem;
  transition: all 0.2s ease;
  background: #fafafa;
}

.form-input:focus {
  outline: none;
  border-color: #6366f1;
  background: white;
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.1);
}

/* Password Input */
.password-wrapper {
  position: relative;
}

.password-input {
  padding-right: 3rem;
}

.password-toggle {
  position: absolute;
  right: 1rem;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.25rem;
  color: #9ca3af;
  transition: color 0.2s ease;
}

.password-toggle:hover {
  color: #6366f1;
}

/* Select */
.select-wrapper {
  position: relative;
}

.form-select {
  width: 100%;
  padding: 1rem 3rem 1rem 1.25rem;
  border: 2px solid #e5e7eb;
  border-radius: 12px;
  font-size: 0.9rem;
  background: #fafafa;
  cursor: pointer;
  transition: all 0.2s ease;
  appearance: none;
}

.form-select:focus {
  outline: none;
  border-color: #6366f1;
  background: white;
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.1);
}

.select-arrow {
  position: absolute;
  right: 1.25rem;
  top: 50%;
  transform: translateY(-50%);
  color: #9ca3af;
  pointer-events: none;
  font-size: 1.5rem;
}

/* Form Actions */
.form-actions {
  background: #f8fafc;
  padding: 2rem 2.5rem;
  display: flex;
  justify-content: center;
  gap: 1rem;
  border-top: 1px solid #e2e8f0;
}

/* Buttons */
.btn {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 1rem 2rem;
  border-radius: 12px;
  font-weight: 600;
  font-size: 0.9rem;
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
  text-decoration: none;
  min-width: 150px;
  justify-content: center;
}

.btn-icon {
  font-size: 1.1rem;
}

.btn-primary {
  background: linear-gradient(135deg, #10b981 0%, #059669 100%);
  color: white;
  box-shadow: 0 4px 15px rgba(16, 185, 129, 0.3);
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 25px rgba(16, 185, 129, 0.4);
}

.btn-secondary {
  background: white;
  color: #6b7280;
  border: 2px solid #e5e7eb;
}

.btn-secondary:hover {
  background: #f9fafb;
  border-color: #d1d5db;
}

/* Responsive */
@media (max-width: 768px) {
  .create-user-page {
    padding: 1rem;
  }
  
  .form-grid {
    grid-template-columns: 1fr;
  }
  
  .form-actions {
    flex-direction: column;
  }
  
  .page-title {
    font-size: 1.5rem;
  }
  
  .section-title {
    font-size: 1.125rem;
  }
}

@media (max-width: 480px) {
  .header-content {
    padding: 1.5rem;
  }
  
  .form-content {
    padding: 1.5rem;
  }
  
  .form-actions {
    padding: 1.5rem;
  }
}
</style>