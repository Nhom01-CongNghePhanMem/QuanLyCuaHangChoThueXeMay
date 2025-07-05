<script setup>
import { ref, watch } from 'vue'
import { useRouter } from 'vue-router'
import defaultAvatar from '@/assets/image.png'

const props = defineProps({
  form: { type: Object, required: true },
  roles: { type: Array, default: () => [] },
  isLoading: { type: Boolean, default: false }
})
const emit = defineEmits(['submit', 'cancel'])
const router = useRouter()

const previewImage = ref(null)
const fileInputRef = ref(null)

watch(() => props.form.FormFile, (file) => {
  if (file) {
    const reader = new FileReader()
    reader.onload = (e) => {
      previewImage.value = e.target.result
    }
    reader.readAsDataURL(file)
  } else {
    previewImage.value = null
  }
})

function onFileChange(event) {
  const file = event.target.files[0]
  if (file) {
    props.form.FormFile = file
  }
}

function removeImage() {
  props.form.FormFile = null
  previewImage.value = null
  if (fileInputRef.value) {
    fileInputRef.value.value = ''
  }
}

function triggerFileInput() {
  fileInputRef.value?.click()
}

function onSubmit() {
  emit('submit', props.form)
}

function onCancel() {
  router.push('/admin/employees')
}

const statusOptions = {
  0: 'Đang làm',
  1: 'Xin nghỉ',
  2: 'Đã nghỉ',
}
</script>

<template>
  <div class="employee-create-page">
    <div class="page-header">
      <div class="header-content">
        <div class="breadcrumb">
          <span class="breadcrumb-item" @click="router.push('/admin/employees')">Quản lý nhân viên</span>
          <i class="breadcrumb-separator">❯</i>
          <span class="breadcrumb-current">Thêm nhân viên mới</span>
        </div>
        <h1 class="page-title">
          <i class="title-icon">👤</i>
          Thêm nhân viên mới
        </h1>
        <p class="page-subtitle">Điền thông tin chi tiết để tạo nhân viên mới</p>
      </div>
    </div>

    <div class="form-container">
      <form class="employee-form" @submit.prevent="onSubmit">
        <!-- Avatar Section -->
        <div class="avatar-section">
          <div class="avatar-wrapper">
            <div class="avatar-preview">
              <img
                v-if="previewImage"
                :src="previewImage"
                alt="Preview"
                class="avatar-img"
              />
              <img
                v-else-if="form.Avatar"
                :src="form.Avatar"
                alt="Current Avatar"
                class="avatar-img"
              />
              <div v-else class="avatar-placeholder">
                <i class="avatar-icon">📷</i>
                <span>Chọn ảnh</span>
              </div>
            </div>
            <div class="avatar-controls">
              <button type="button" class="btn btn-upload" @click="triggerFileInput">
                <i class="btn-icon">📁</i>
                Chọn ảnh
              </button>
              <button 
                v-if="previewImage || form.FormFile" 
                type="button" 
                class="btn btn-remove" 
                @click="removeImage"
              >
                <i class="btn-icon">🗑️</i>
                Xóa
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

        <!-- Form Fields -->
        <div class="form-content">
          <div class="form-section">
            <h3 class="section-title">
              <i class="section-icon">📋</i>
              Thông tin cá nhân
            </h3>
            <div class="form-grid">
              <div class="form-group">
                <label class="form-label">
                  <i class="label-icon">👤</i>
                  Họ và tên
                  <span class="required">*</span>
                </label>
                <input 
                  v-model="form.FullName" 
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
                <input 
                  v-model="form.DateOfBirth" 
                  type="date" 
                  class="form-input"
                  required 
                />
              </div>

              <div class="form-group full-width">
                <label class="form-label">
                  <i class="label-icon">📍</i>
                  Địa chỉ
                  <span class="required">*</span>
                </label>
                <input 
                  v-model="form.Address" 
                  type="text" 
                  class="form-input"
                  placeholder="Nhập địa chỉ đầy đủ"
                  required 
                />
              </div>
            </div>
          </div>

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
                <input 
                  v-model="form.StartDate" 
                  type="date" 
                  class="form-input"
                  required 
                />
              </div>

              <div class="form-group">
                <label class="form-label">
                  <i class="label-icon">💰</i>
                  Lương (VNĐ)
                  <span class="required">*</span>
                </label>
                <input 
                  v-model="form.Salary" 
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
                  <select v-model="form.Status" class="form-select" required>
                    <option v-for="(label, code) in statusOptions" :key="code" :value="code">
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
          <button type="button" class="btn btn-secondary" @click="onCancel">
            <i class="btn-icon">❌</i>
            Hủy bỏ
          </button>
          <button type="submit" class="btn btn-primary" :disabled="isLoading">
            <i class="btn-icon">{{ isLoading ? '⏳' : '💾' }}</i>
            {{ isLoading ? 'Đang lưu...' : 'Lưu nhân viên' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.employee-create-page {
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
  max-width: 800px;
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
  padding: 2rem;
  text-align: center;
}

.avatar-wrapper {
  display: inline-block;
}

.avatar-preview {
  width: 120px;
  height: 120px;
  margin: 0 auto 1rem;
  position: relative;
}

.avatar-img {
  width: 100%;
  height: 100%;
  border-radius: 50%;
  object-fit: cover;
  border: 4px solid white;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);
}

.avatar-placeholder {
  width: 100%;
  height: 100%;
  border-radius: 50%;
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
  font-size: 2rem;
  margin-bottom: 0.5rem;
}

.avatar-controls {
  display: flex;
  gap: 0.75rem;
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
  padding: 2rem;
}

.form-section {
  margin-bottom: 2rem;
}

.form-section:last-child {
  margin-bottom: 0;
}

.section-title {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 1.25rem;
  font-weight: 600;
  color: #374151;
  margin: 0 0 1.5rem 0;
  padding-bottom: 0.75rem;
  border-bottom: 2px solid #f3f4f6;
}

.section-icon {
  font-size: 1.5rem;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1.5rem;
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
  font-size: 0.875rem;
}

.label-icon {
  font-size: 1rem;
}

.required {
  color: #ef4444;
  font-weight: 700;
}

.form-input {
  width: 100%;
  padding: 0.875rem 1rem;
  border: 2px solid #e5e7eb;
  border-radius: 12px;
  font-size: 0.875rem;
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
  padding: 0.875rem 2.5rem 0.875rem 1rem;
  border: 2px solid #e5e7eb;
  border-radius: 12px;
  font-size: 0.875rem;
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
  right: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: #9ca3af;
  pointer-events: none;
  font-size: 1.25rem;
}

/* Form Actions */
.form-actions {
  background: #f8fafc;
  padding: 1.5rem 2rem;
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  border-top: 1px solid #e2e8f0;
}

/* Buttons */
.btn {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1.5rem;
  border-radius: 12px;
  font-weight: 600;
  font-size: 0.875rem;
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
  text-decoration: none;
}

.btn-icon {
  font-size: 1rem;
}

.btn-primary {
  background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%);
  color: white;
  box-shadow: 0 4px 12px rgba(99, 102, 241, 0.3);
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(99, 102, 241, 0.4);
}

.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
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
  .employee-create-page {
    padding: 1rem;
  }
  
  .form-grid {
    grid-template-columns: 1fr;
  }
  
  .form-group.full-width {
    grid-column: 1;
  }
  
  .form-actions {
    flex-direction: column-reverse;
  }
  
  .avatar-controls {
    flex-direction: column;
  }
}
</style>