<script setup>
import { ref, watch } from 'vue'
import { useRouter } from 'vue-router'
import defaultAvatar from '@/assets/image.png'
import { employeeService } from '@/api/services/employeeService'
import { getFullPath } from '@/utils/UrlUtils'

const previewImage = ref(null)
const fileInputRef = ref(null)
const props = defineProps({
  employee: { type: Object, required: true },
})
const emit = defineEmits(['update', 'create-account'])

const router = useRouter()
const form = ref({ ...props.employee })

watch(
  () => props.employee,
  (val) => {
    if (val) {
      form.value = { ...val }
      form.value.dateOfBirth = toDateInputString(val.dateOfBirth)
      form.value.startDate = toDateInputString(val.startDate)
      previewImage.value = val.avatar || null
    }
  },
  { immediate: true },
)

function onFileChange(event) {
  const file = event.target.files[0]
  if (file) {
    form.value.formFile = file
    const reader = new FileReader()
    reader.onload = (e) => {
      previewImage.value = e.target.result
    }
    reader.readAsDataURL(file)
  }
}

async function removeImage() {
  if (!confirm('Bạn có chắc chắn muốn xóa ảnh đại diện này không?')) {
    return
  }
  if (form.value.avatar) {
    try {
      await employeeService.deleteAvatar(form.value.employeeId)
      form.value.avatar = null
      previewImage.value = null
      form.value.FormFile = null
      if (fileInputRef.value) fileInputRef.value.value = ''
    } catch (error) {
      alert('Xóa ảnh thất bại!')
    }
  } else {
    form.value.FormFile = null
    previewImage.value = null
    if (fileInputRef.value) fileInputRef.value.value = ''
  }
}

form.value.FormFile = null
previewImage.value = null
if (fileInputRef.value) fileInputRef.value.value = ''

function triggerFileInput() {
  fileInputRef.value?.click()
}

function onSubmit() {
  emit('update', form.value)
}

function onCreateAccount() {
  emit('create-account', form.value)
}
function onEditAccount(){
    
}

function onCancel() {
  router.back()
}

const statusOptions = {
  0: 'Đang làm',
  1: 'Xin nghỉ',
  2: 'Đã nghỉ',
}

function toDateInputString(dateStr) {
  if (!dateStr) return ''
  return dateStr.split('T')[0]
}
</script>

<template>
  <div class="employee-edit-page">
    <!-- Header -->
    <div class="page-header">
      <div class="header-content">
        <div class="breadcrumb">
          <span class="breadcrumb-item" @click="router.push('/admin/employees')">
            <i class="icon">👥</i>
            Quản lý nhân viên
          </span>
          <i class="breadcrumb-separator">❯</i>
          <span class="breadcrumb-current">Chỉnh sửa nhân viên</span>
        </div>
        <h1 class="page-title">
          <i class="title-icon">✏️</i>
          Chỉnh sửa thông tin nhân viên
        </h1>
        <p class="page-subtitle">Cập nhật thông tin chi tiết của nhân viên</p>
      </div>
    </div>

    <!-- Form -->
    <div class="form-container">
      <form class="employee-form" @submit.prevent="onSubmit">
        <!-- Avatar Section -->
        <div class="avatar-section">
          <div class="avatar-wrapper">
            <div class="avatar-container">
              <div class="avatar-preview">
                <img v-if="previewImage" :src="previewImage" alt="Preview" class="avatar-img" />
                <img
                  v-else-if="form.avatar"
                  :src="getFullPath(form.avatar)"
                  alt="Current Avatar"
                  class="avatar-img"
                />
                <div v-else class="avatar-placeholder">
                  <i class="avatar-icon">📷</i>
                  <span>Chọn ảnh</span>
                </div>
              </div>
              <div class="avatar-overlay" @click="triggerFileInput">
                <i class="overlay-icon">📁</i>
                <span>Thay đổi</span>
              </div>
            </div>
            <div class="avatar-actions">
              <button type="button" class="btn btn-upload" @click="triggerFileInput">
                <i class="btn-icon">📁</i>
                Chọn ảnh mới
              </button>
              <button
                v-if="previewImage || form.avatar"
                type="button"
                class="btn btn-remove"
                @click="removeImage"
              >
                <i class="btn-icon">🗑️</i>
                Xóa ảnh
              </button>
            </div>
            <input
              ref="fileInputRef"
              type="file"
              accept="image/*"
              @change="onFileChange"
              class="file-input"
              hidden
            />
          </div>
        </div>

        <!-- Form Content -->
        <div class="form-content">
          <!-- Personal Info Section -->
          <div class="form-section">
            <h3 class="section-title">
              <i class="section-icon">👤</i>
              Thông tin cá nhân
            </h3>
            <div class="form-grid">
              <div class="form-group">
                <label class="form-label">
                  <i class="label-icon">📝</i>
                  Họ và tên
                  <span class="required">*</span>
                </label>
                <input
                  v-model="form.fullName"
                  type="text"
                  class="form-input"
                  placeholder="Nhập họ và tên đầy đủ"
                  required
                />
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="label-icon">🎂</i>
                  Ngày sinh
                  <span class="required">*</span>
                </label>
                <input v-model="form.dateOfBirth" type="date" class="form-input" required />
              </div>

              <div class="form-group full-width">
                <label class="form-label">
                  <i class="label-icon">📍</i>
                  Địa chỉ
                  <span class="required">*</span>
                </label>
                <input
                  v-model="form.address"
                  type="text"
                  class="form-input"
                  placeholder="Nhập địa chỉ đầy đủ"
                  required
                />
              </div>
            </div>
          </div>

          <!-- Work Info Section -->
          <div class="form-section">
            <h3 class="section-title">
              <i class="section-icon">💼</i>
              Thông tin công việc
            </h3>
            <div class="form-grid">
              <div class="form-group">
                <label class="form-label">
                  <i class="label-icon">📅</i>
                  Ngày vào làm
                  <span class="required">*</span>
                </label>
                <input v-model="form.startDate" type="date" class="form-input" required />
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="label-icon">💰</i>
                  Lương (VNĐ)
                  <span class="required">*</span>
                </label>
                <input
                  v-model="form.salary"
                  type="number"
                  min="0"
                  step="100000"
                  class="form-input"
                  placeholder="0"
                  required
                />
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="label-icon">📊</i>
                  Trạng thái
                  <span class="required">*</span>
                </label>
                <div class="select-wrapper">
                  <select v-model="form.status" class="form-select" required>
                    <option
                      v-for="(label, code) in statusOptions"
                      :key="code"
                      :value="Number(code)"
                    >
                      {{ label }}
                    </option>
                  </select>
                  <i class="select-arrow">⌄</i>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Form Actions -->
        <div class="form-actions">
          <div class="action-left">
            <button
              v-if="!form.roleName"
              type="button"
              class="btn btn-account"
              @click="onCreateAccount"
            >
              <i class="btn-icon">👤</i>
              Tạo tài khoản
            </button>
            
            <!-- Nút chỉnh sửa tài khoản nếu đã có role -->
            <button
              v-else
              type="button"
              class="btn btn-edit-account"
              @click="onEditAccount"
            >
              <i class="btn-icon">⚙️</i>
              Chỉnh sửa tài khoản
            </button>
            
          </div>
          <div class="action-right">
            <button type="button" class="btn btn-secondary" @click="onCancel">
              <i class="btn-icon">❌</i>
              Hủy bỏ
            </button>
            <button type="submit" class="btn btn-primary">
              <i class="btn-icon">💾</i>
              Lưu thay đổi
            </button>
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.employee-edit-page {
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
}

.breadcrumb {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 1rem;
  font-size: 0.875rem;
}

.breadcrumb-item {
  display: flex;
  align-items: center;
  gap: 0.25rem;
  color: #6366f1;
  cursor: pointer;
  transition: color 0.2s ease;
}

.breadcrumb-item:hover {
  color: #4f46e5;
}

.breadcrumb-separator {
  color: #9ca3af;
  font-size: 0.75rem;
}

.breadcrumb-current {
  color: #6b7280;
}

.page-title {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 2rem;
  font-weight: 700;
  color: #1e293b;
  margin: 0 0 0.5rem 0;
}

.title-icon {
  font-size: 2.5rem;
}

.page-subtitle {
  color: #64748b;
  font-size: 1.125rem;
  margin: 0;
}

/* Form Container */
.form-container {
  max-width: 900px;
  margin: 0 auto;
}

.employee-form {
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  overflow: hidden;
}

/* Avatar Section */
.avatar-section {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 3rem 2rem;
  text-align: center;
}

.avatar-wrapper {
  display: inline-block;
}

.avatar-container {
  position: relative;
  display: inline-block;
  margin-bottom: 1.5rem;
}

.avatar-preview {
  width: 140px;
  height: 140px;
  border-radius: 50%;
  overflow: hidden;
  border: 4px solid white;
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15);
}

.avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.avatar-placeholder {
  width: 100%;
  height: 100%;
  background: rgba(255, 255, 255, 0.2);
  border: 2px dashed rgba(255, 255, 255, 0.5);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 0.875rem;
}

.avatar-icon {
  font-size: 3rem;
  margin-bottom: 0.5rem;
}

.avatar-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.6);
  border-radius: 50%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  color: white;
  opacity: 0;
  transition: opacity 0.3s ease;
  cursor: pointer;
  font-size: 0.875rem;
}

.avatar-container:hover .avatar-overlay {
  opacity: 1;
}

.overlay-icon {
  font-size: 2rem;
  margin-bottom: 0.25rem;
}

.avatar-actions {
  display: flex;
  gap: 1rem;
  justify-content: center;
}

.btn-upload {
  background: rgba(255, 255, 255, 0.2);
  color: white;
  border: 2px solid rgba(255, 255, 255, 0.3);
}

.btn-upload:hover {
  background: rgba(255, 255, 255, 0.3);
  border-color: rgba(255, 255, 255, 0.5);
}

.btn-remove {
  background: rgba(239, 68, 68, 0.2);
  color: white;
  border: 2px solid rgba(239, 68, 68, 0.3);
}

.btn-remove:hover {
  background: rgba(239, 68, 68, 0.3);
  border-color: rgba(239, 68, 68, 0.5);
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

.form-group.full-width {
  grid-column: 1 / -1;
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
  justify-content: space-between;
  align-items: center;
  border-top: 1px solid #e2e8f0;
}

.action-left {
  flex: 1;
}

.action-right {
  display: flex;
  gap: 1rem;
}

/* Buttons */
.btn {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.875rem 1.75rem;
  border-radius: 12px;
  font-weight: 600;
  font-size: 0.9rem;
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
  text-decoration: none;
}

.btn-icon {
  font-size: 1.1rem;
}

.btn-primary {
  background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%);
  color: white;
  box-shadow: 0 4px 15px rgba(99, 102, 241, 0.3);
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 25px rgba(99, 102, 241, 0.4);
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

.btn-account {
  background: linear-gradient(135deg, #10b981 0%, #059669 100%);
  color: white;
  box-shadow: 0 4px 15px rgba(16, 185, 129, 0.3);
}

.btn-account:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 25px rgba(16, 185, 129, 0.4);
}

/* Responsive */
@media (max-width: 768px) {
  .employee-edit-page {
    padding: 1rem;
  }

  .form-grid {
    grid-template-columns: 1fr;
  }

  .form-group.full-width {
    grid-column: 1;
  }

  .form-actions {
    flex-direction: column;
    gap: 1rem;
  }

  .action-left,
  .action-right {
    width: 100%;
  }

  .action-right {
    flex-direction: column-reverse;
  }

  .avatar-actions {
    flex-direction: column;
  }
}

@media (max-width: 480px) {
  .avatar-preview {
    width: 100px;
    height: 100px;
  }

  .page-title {
    font-size: 1.5rem;
  }

  .section-title {
    font-size: 1.125rem;
  }
}
</style>
