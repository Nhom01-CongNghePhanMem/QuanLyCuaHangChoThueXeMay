<script setup>
import { ref, reactive, watch } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const props = defineProps({
  category: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['update', 'delete'])

// Form data
const form = reactive({
  categoryName: '',
  depositAmount: ''
})

// Edit mode
const isEditing = ref(false)
const errors = ref({})
const isSubmitting = ref(false)

// Watch for category changes
watch(() => props.category, (newCategory) => {
  if (newCategory) {
    form.categoryName = newCategory.categoryName
    form.depositAmount = newCategory.depositAmount
  }
}, { immediate: true })

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

function toggleEdit() {
  isEditing.value = !isEditing.value
  if (!isEditing.value) {
    // Reset form if canceling
    form.categoryName = props.category.categoryName
    form.depositAmount = props.category.depositAmount
    errors.value = {}
  }
}

function saveCategory() {
  if (!validateForm()) return
  
  isSubmitting.value = true
  emit('update', {
    categoryId: props.category.categoryId,
    categoryName: form.categoryName,
    depositAmount: parseFloat(form.depositAmount)
  })
  
  setTimeout(() => {
    isSubmitting.value = false
    isEditing.value = false
  }, 1000)
}

function deleteCategory() {
  if (confirm('Bạn có chắc chắn muốn xóa loại xe này không? Hành động này không thể hoàn tác!')) {
    emit('delete', props.category.categoryId)
  }
}

function formatCurrency(amount) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(amount)
}
</script>

<template>
  <div class="category-detail-container" v-if="category">
    <!-- Header -->
    <div class="detail-header">
      <h1 class="category-name">{{ category.categoryName }}</h1>
      <div class="header-actions">
        <button 
          v-if="!isEditing" 
          @click="toggleEdit" 
          class="btn btn-primary"
        >
          Chỉnh sửa
        </button>
        <button 
          v-if="isEditing" 
          @click="saveCategory" 
          class="btn btn-success"
          :disabled="isSubmitting"
        >
          <span v-if="isSubmitting">Đang lưu...</span>
          <span v-else>Lưu</span>
        </button>
        <button 
          v-if="isEditing" 
          @click="toggleEdit" 
          class="btn btn-secondary"
        >
          Hủy
        </button>
        <button 
          @click="deleteCategory" 
          class="btn btn-danger"
        >
          Xóa
        </button>
      </div>
    </div>

    <!-- Content -->
    <div class="detail-content">
      <div class="info-section">
        <h2 class="section-title">Thông tin loại xe</h2>
        <div class="info-form">
          <!-- Category Name -->
          <div class="form-group">
            <label class="form-label">Tên loại xe</label>
            <input 
              v-if="isEditing"
              v-model="form.categoryName"
              type="text"
              class="form-input"
              :class="{ 'error': errors.categoryName }"
              placeholder="Nhập tên loại xe"
            />
            <span v-else class="info-value">{{ category.categoryName }}</span>
            <span v-if="errors.categoryName" class="error-message">{{ errors.categoryName }}</span>
          </div>

          <!-- Deposit Amount -->
          <div class="form-group">
            <label class="form-label">Tiền cọc</label>
            <input 
              v-if="isEditing"
              v-model="form.depositAmount"
              type="number"
              class="form-input"
              :class="{ 'error': errors.depositAmount }"
              placeholder="Nhập tiền cọc"
              min="0"
              step="1000"
            />
            <span v-else class="info-value amount">{{ formatCurrency(category.depositAmount) }}</span>
            <span v-if="errors.depositAmount" class="error-message">{{ errors.depositAmount }}</span>
          </div>

          <!-- Category ID -->
          <div class="form-group">
            <label class="form-label">ID loại xe</label>
            <span class="info-value category-id">#{{ category.categoryId }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.category-detail-container {
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  margin: 20px;
  overflow: hidden;
  max-width: 800px;
  margin: 20px auto;
}

.detail-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 24px 32px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.category-name {
  font-size: 24px;
  font-weight: 700;
  margin: 0;
}

.header-actions {
  display: flex;
  gap: 12px;
}

.btn {
  padding: 8px 16px;
  border: none;
  border-radius: 6px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-primary {
  background: rgba(255, 255, 255, 0.2);
  color: white;
  border: 1px solid rgba(255, 255, 255, 0.3);
}

.btn-primary:hover {
  background: rgba(255, 255, 255, 0.3);
}

.btn-success {
  background: #22c55e;
  color: white;
}

.btn-success:hover:not(:disabled) {
  background: #16a34a;
}

.btn-secondary {
  background: #6b7280;
  color: white;
}

.btn-secondary:hover {
  background: #4b5563;
}

.btn-danger {
  background: #ef4444;
  color: white;
}

.btn-danger:hover {
  background: #dc2626;
}

.detail-content {
  padding: 32px;
}

.info-section {
  margin-bottom: 32px;
}

.section-title {
  font-size: 18px;
  font-weight: 600;
  color: #1f2937;
  margin-bottom: 24px;
  padding-bottom: 12px;
  border-bottom: 2px solid #e5e7eb;
}

.info-form {
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

.info-value {
  font-size: 16px;
  color: #1f2937;
  padding: 12px 0;
  min-height: 44px;
  display: flex;
  align-items: center;
}

.info-value.amount {
  color: #059669;
  font-weight: 600;
}

.info-value.category-id {
  color: #6366f1;
  font-weight: 600;
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

@media (max-width: 768px) {
  .category-detail-container {
    margin: 10px;
  }

  .detail-header {
    flex-direction: column;
    gap: 16px;
    text-align: center;
  }

  .header-actions {
    flex-wrap: wrap;
    justify-content: center;
  }

  .detail-content {
    padding: 20px;
  }
}
</style>