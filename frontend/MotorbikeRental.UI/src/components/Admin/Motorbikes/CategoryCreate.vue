<script setup>
import { ref, reactive } from 'vue'

const emit = defineEmits(['submit'])

// Form data
const form = reactive({
  categoryName: '',
  depositAmount: ''
})

// Validation
const errors = ref({})
const isSubmitting = ref(false)

function validateForm() {
  errors.value = {}
  
  if (!form.categoryName.trim()) {
    errors.value.categoryName = 'Tên loại xe là bắt buộc'
  }
  
  if (!form.depositAmount || form.depositAmount <= 0) {
    errors.value.depositAmount = 'Tiền cọc phải lớn hơn 0'
  }
  
  return Object.keys(errors.value).length === 0
}

function handleSubmit() {
  if (!validateForm()) return
  
  isSubmitting.value = true
  emit('submit', {
    categoryName: form.categoryName,
    depositAmount: parseFloat(form.depositAmount)
  })
  
  setTimeout(() => {
    isSubmitting.value = false
  }, 1000)
}

function resetForm() {
  form.categoryName = ''
  form.depositAmount = ''
  errors.value = {}
}
</script>

<template>
  <div class="category-create-container">
    <!-- Header -->
    <div class="form-header">
      <h1 class="form-title">Thêm loại xe mới</h1>
      <p class="form-subtitle">Nhập thông tin loại xe để thêm vào hệ thống</p>
    </div>

    <!-- Form -->
    <div class="form-content">
      <form @submit.prevent="handleSubmit" class="category-form">
        <div class="form-group">
          <label class="form-label" for="categoryName">
            Tên loại xe <span class="required">*</span>
          </label>
          <input
            id="categoryName"
            v-model="form.categoryName"
            type="text"
            class="form-input"
            :class="{ 'error': errors.categoryName }"
            placeholder="Ví dụ: Xe số, Xe ga, Xe côn..."
          />
          <span v-if="errors.categoryName" class="error-message">{{ errors.categoryName }}</span>
        </div>

        <div class="form-group">
          <label class="form-label" for="depositAmount">
            Tiền cọc (VND) <span class="required">*</span>
          </label>
          <input
            id="depositAmount"
            v-model="form.depositAmount"
            type="number"
            class="form-input"
            :class="{ 'error': errors.depositAmount }"
            placeholder="Ví dụ: 500000"
            min="0"
            step="1000"
          />
          <span v-if="errors.depositAmount" class="error-message">{{ errors.depositAmount }}</span>
        </div>

        <!-- Form Actions -->
        <div class="form-actions">
          <button
            type="button"
            @click="resetForm"
            class="btn btn-secondary"
            :disabled="isSubmitting"
          >
            Làm mới
          </button>
          <button
            type="submit"
            class="btn btn-primary"
            :disabled="isSubmitting"
          >
            <span v-if="isSubmitting">Đang thêm...</span>
            <span v-else>Thêm loại xe</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.category-create-container {
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  margin: 20px;
  overflow: hidden;
  max-width: 600px;
  margin: 20px auto;
}

.form-header {
  padding: 32px 32px 24px 32px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  text-align: center;
}

.form-title {
  font-size: 24px;
  font-weight: 700;
  margin: 0 0 8px 0;
}

.form-subtitle {
  font-size: 14px;
  margin: 0;
  opacity: 0.9;
}

.form-content {
  padding: 32px;
}

.category-form {
  display: flex;
  flex-direction: column;
  gap: 24px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-label {
  font-size: 14px;
  font-weight: 600;
  color: #374151;
}

.required {
  color: #ef4444;
}

.form-input {
  padding: 12px 16px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  font-size: 16px;
  transition: all 0.2s;
}

.form-input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.form-input.error {
  border-color: #ef4444;
}

.error-message {
  font-size: 12px;
  color: #ef4444;
}

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
  border-radius: 8px;
  font-size: 16px;
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

@media (max-width: 768px) {
  .category-create-container {
    margin: 10px;
  }

  .form-header {
    padding: 24px 20px;
  }

  .form-content {
    padding: 24px 20px;
  }

  .form-actions {
    flex-direction: column;
  }

  .btn {
    width: 100%;
  }
}
</style>