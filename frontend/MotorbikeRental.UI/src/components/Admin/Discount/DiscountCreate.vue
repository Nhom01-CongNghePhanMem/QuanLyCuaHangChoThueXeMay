<script setup>
import { ref, reactive } from 'vue'

const props = defineProps({
  categories: {
    type: Array,
    required: true,
    default: () => []
  },
  isLoading: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['submit', 'back'])

// Form data
const form = reactive({
  name: '',
  categoryId: [],
  description: '',
  value: '',
  startDate: '',
  endDate: '',
  isActive: true
})

// Form validation
const errors = ref({})

function validateForm() {
  errors.value = {}
  
  if (!form.name.trim()) {
    errors.value.name = 'Tên mã giảm giá là bắt buộc'
  }
  
  if (form.categoryId.length === 0) {
    errors.value.categoryId = 'Vui lòng chọn ít nhất một loại xe'
  }
  
  if (!form.value || form.value <= 0 || form.value > 100) {
    errors.value.value = 'Giá trị giảm giá phải từ 1-100%'
  }
  
  if (!form.startDate) {
    errors.value.startDate = 'Ngày bắt đầu là bắt buộc'
  }
  
  if (!form.endDate) {
    errors.value.endDate = 'Ngày kết thúc là bắt buộc'
  }
  
  if (form.startDate && form.endDate && new Date(form.startDate) >= new Date(form.endDate)) {
    errors.value.endDate = 'Ngày kết thúc phải sau ngày bắt đầu'
  }
  
  return Object.keys(errors.value).length === 0
}

function handleSubmit() {
  if (!validateForm()) return
  
  const submitData = {
    name: form.name,
    categoryId: form.categoryId,
    description: form.description,
    value: parseFloat(form.value),
    startDate: form.startDate,
    endDate: form.endDate,
    isActive: form.isActive
  }
  
  emit('submit', submitData)
}

function handleBack() {
  emit('back')
}

function resetForm() {
  form.name = ''
  form.categoryId = []
  form.description = ''
  form.value = ''
  form.startDate = ''
  form.endDate = ''
  form.isActive = true
  errors.value = {}
}

function toggleCategory(categoryId) {
  const index = form.categoryId.indexOf(categoryId)
  if (index > -1) {
    form.categoryId.splice(index, 1)
  } else {
    form.categoryId.push(categoryId)
  }
}
</script>

<template>
  <div class="discount-create-container">
    <!-- Header -->
    <div class="header">
      <div class="header-content">
        <h1 class="title">Tạo mã giảm giá mới</h1>
        <button class="btn-back" @click="handleBack">
          ← Quay lại
        </button>
      </div>
    </div>

    <!-- Form -->
    <div class="form-container">
      <form @submit.prevent="handleSubmit" class="form">
        <div class="form-grid">
          <!-- Name -->
          <div class="form-group full-width">
            <label class="form-label">
              Tên mã giảm giá <span class="required">*</span>
            </label>
            <input
              v-model="form.name"
              type="text"
              class="form-input"
              :class="{ 'error': errors.name }"
              placeholder="Nhập tên mã giảm giá"
            />
            <span v-if="errors.name" class="error-message">{{ errors.name }}</span>
          </div>

          <!-- Categories -->
          <div class="form-group full-width">
            <label class="form-label">
              Loại xe áp dụng <span class="required">*</span>
            </label>
            <div class="category-list">
              <div
                v-for="category in categories"
                :key="category.categoryId"
                class="category-item"
                :class="{ 'selected': form.categoryId.includes(category.categoryId) }"
                @click="toggleCategory(category.categoryId)"
              >
                <input
                  type="checkbox"
                  :checked="form.categoryId.includes(category.categoryId)"
                  @change="toggleCategory(category.categoryId)"
                />
                <span class="category-name">{{ category.categoryName }}</span>
              </div>
            </div>
            <span v-if="errors.categoryId" class="error-message">{{ errors.categoryId }}</span>
          </div>

          <!-- Value -->
          <div class="form-group">
            <label class="form-label">
              Giá trị giảm giá (%) <span class="required">*</span>
            </label>
            <input
              v-model="form.value"
              type="number"
              class="form-input"
              :class="{ 'error': errors.value }"
              placeholder="Nhập % giảm giá"
              min="1"
              max="100"
            />
            <span v-if="errors.value" class="error-message">{{ errors.value }}</span>
          </div>

          <!-- Status -->
          <div class="form-group">
            <label class="form-label">Trạng thái</label>
            <select v-model="form.isActive" class="form-select">
              <option :value="true">Hoạt động</option>
              <option :value="false">Không hoạt động</option>
            </select>
          </div>

          <!-- Start Date -->
          <div class="form-group">
            <label class="form-label">
              Ngày bắt đầu <span class="required">*</span>
            </label>
            <input
              v-model="form.startDate"
              type="date"
              class="form-input"
              :class="{ 'error': errors.startDate }"
            />
            <span v-if="errors.startDate" class="error-message">{{ errors.startDate }}</span>
          </div>

          <!-- End Date -->
          <div class="form-group">
            <label class="form-label">
              Ngày kết thúc <span class="required">*</span>
            </label>
            <input
              v-model="form.endDate"
              type="date"
              class="form-input"
              :class="{ 'error': errors.endDate }"
            />
            <span v-if="errors.endDate" class="error-message">{{ errors.endDate }}</span>
          </div>

          <!-- Description -->
          <div class="form-group full-width">
            <label class="form-label">Mô tả</label>
            <textarea
              v-model="form.description"
              class="form-textarea"
              placeholder="Nhập mô tả mã giảm giá (không bắt buộc)"
              rows="3"
            ></textarea>
          </div>
        </div>

        <!-- Actions -->
        <div class="form-actions">
          <button
            type="button"
            @click="resetForm"
            class="btn btn-secondary"
            :disabled="isLoading"
          >
            Làm mới
          </button>
          <button
            type="submit"
            class="btn btn-primary"
            :disabled="isLoading"
          >
            <span v-if="isLoading">Đang tạo...</span>
            <span v-else>Tạo mã giảm giá</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.discount-create-container {
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  margin: 20px;
  overflow: hidden;
}

/* Header */
.header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 24px 32px;
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.title {
  font-size: 24px;
  font-weight: 700;
  margin: 0;
}

.btn-back {
  background: rgba(255, 255, 255, 0.2);
  border: 1px solid rgba(255, 255, 255, 0.3);
  color: white;
  padding: 8px 16px;
  border-radius: 6px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-back:hover {
  background: rgba(255, 255, 255, 0.3);
}

/* Form */
.form-container {
  padding: 32px;
}

.form {
  max-width: 800px;
  margin: 0 auto;
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 24px;
  margin-bottom: 32px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-group.full-width {
  grid-column: 1 / -1;
}

.form-label {
  font-size: 14px;
  font-weight: 600;
  color: #374151;
}

.required {
  color: #ef4444;
}

.form-input,
.form-select,
.form-textarea {
  padding: 12px 16px;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  font-size: 14px;
  transition: all 0.2s;
}

.form-input:focus,
.form-select:focus,
.form-textarea:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.form-input.error,
.form-select.error,
.form-textarea.error {
  border-color: #ef4444;
}

.form-textarea {
  resize: vertical;
  min-height: 80px;
}

.error-message {
  font-size: 12px;
  color: #ef4444;
}

/* Categories */
.category-list {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 12px;
  margin-top: 8px;
}

.category-item {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 12px;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.2s;
}

.category-item:hover {
  border-color: #667eea;
  background: #f8fafc;
}

.category-item.selected {
  border-color: #667eea;
  background: #f0f4ff;
}

.category-name {
  font-size: 14px;
  font-weight: 500;
  color: #374151;
}

/* Actions */
.form-actions {
  display: flex;
  justify-content: center;
  gap: 16px;
  padding-top: 24px;
  border-top: 1px solid #e5e7eb;
}

.btn {
  padding: 12px 24px;
  border: none;
  border-radius: 6px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
  min-width: 120px;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-primary {
  background: #667eea;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: #5a67d8;
}

.btn-secondary {
  background: #f3f4f6;
  color: #374151;
  border: 1px solid #d1d5db;
}

.btn-secondary:hover:not(:disabled) {
  background: #e5e7eb;
}

/* Responsive */
@media (max-width: 768px) {
  .discount-create-container {
    margin: 10px;
  }

  .header-content {
    flex-direction: column;
    gap: 16px;
    text-align: center;
  }

  .form-container {
    padding: 20px;
  }

  .form-grid {
    grid-template-columns: 1fr;
    gap: 20px;
  }

  .category-list {
    grid-template-columns: 1fr;
  }

  .form-actions {
    flex-direction: column;
  }

  .btn {
    width: 100%;
  }
}
</style>