<script setup>
import { ref, reactive, watch } from 'vue'

const props = defineProps({
  discount: {
    type: Object,
    required: true
  },
  categories: {
    type: Array,
    required: true,
    default: () => []
  }
})

const emit = defineEmits(['update', 'delete', 'back'])

// Edit mode
const isEditing = ref(false)
const isSubmitting = ref(false)
const errors = ref({})

// Form data
const form = reactive({
  name: '',
  description: '',
  categoryId: [],
  value: '',
  startDate: '',
  endDate: '',
  isActive: true
})

// Watch for discount changes
watch(() => props.discount, (newDiscount) => {
  if (newDiscount) {
    form.name = newDiscount.name
    form.description = newDiscount.description || ''
    // Convert categoryNames to categoryIds for editing
    form.categoryId = getCategoryIdsByNames(newDiscount.categoryNames)
    form.value = newDiscount.value
    form.startDate = formatDateForInput(newDiscount.startDate)
    form.endDate = formatDateForInput(newDiscount.endDate)
    form.isActive = newDiscount.isActive
  }
}, { immediate: true })

function getCategoryIdsByNames(categoryNames) {
  if (!categoryNames || !props.categories) return []
  return props.categories
    .filter(cat => categoryNames.includes(cat.categoryName))
    .map(cat => cat.categoryId)
}

function formatDateForInput(dateString) {
  if (!dateString) return ''
  return new Date(dateString).toISOString().split('T')[0]
}

function formatDateForDisplay(dateString) {
  if (!dateString) return ''
  return new Date(dateString).toLocaleDateString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit'
  })
}

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

function toggleEdit() {
  isEditing.value = !isEditing.value
  if (!isEditing.value) {
    // Reset form if canceling
    form.name = props.discount.name
    form.description = props.discount.description || ''
    form.categoryId = getCategoryIdsByNames(props.discount.categoryNames)
    form.value = props.discount.value
    form.startDate = formatDateForInput(props.discount.startDate)
    form.endDate = formatDateForInput(props.discount.endDate)
    form.isActive = props.discount.isActive
    errors.value = {}
  }
}

function handleSave() {
  if (!validateForm()) return
  
  isSubmitting.value = true
  
  const updateData = {
    discountId: props.discount.discountId,
    name: form.name,
    description: form.description,
    categoryId: form.categoryId,
    value: parseFloat(form.value),
    startDate: form.startDate + 'T00:00:00.000Z',
    endDate: form.endDate + 'T23:59:59.999Z',
    isActive: form.isActive
  }
  
  emit('update', updateData)
  
  setTimeout(() => {
    isSubmitting.value = false
    isEditing.value = false
  }, 1000)
}

function handleDelete() {
  if (confirm('Bạn có chắc chắn muốn xóa mã giảm giá này không? Hành động này không thể hoàn tác!')) {
    emit('delete', props.discount.discountId)
  }
}

function handleBack() {
  emit('back')
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
  <div class="discount-detail-container" v-if="discount">
    <!-- Header -->
    <div class="header">
      <div class="header-content">
        <h1 class="title">Chi tiết mã giảm giá</h1>
        <div class="header-actions">
          <button class="btn-back" @click="handleBack">
            ← Quay lại
          </button>
          <button 
            v-if="!isEditing" 
            @click="toggleEdit" 
            class="btn-edit"
          >
            Chỉnh sửa
          </button>
          <button 
            v-if="isEditing" 
            @click="handleSave" 
            class="btn-save"
            :disabled="isSubmitting"
          >
            <span v-if="isSubmitting">Đang lưu...</span>
            <span v-else>Lưu</span>
          </button>
          <button 
            v-if="isEditing" 
            @click="toggleEdit" 
            class="btn-cancel"
          >
            Hủy
          </button>
          <button 
            @click="handleDelete" 
            class="btn-delete"
          >
            Xóa
          </button>
        </div>
      </div>
    </div>

    <!-- Content -->
    <div class="content">
      <div class="info-container">
        <div class="info-grid">
          <!-- Name -->
          <div class="info-group">
            <label class="info-label">Tên mã giảm giá</label>
            <div v-if="isEditing" class="edit-field">
              <input
                v-model="form.name"
                type="text"
                class="form-input"
                :class="{ 'error': errors.name }"
                placeholder="Nhập tên mã giảm giá"
              />
              <span v-if="errors.name" class="error-message">{{ errors.name }}</span>
            </div>
            <span v-else class="info-value">{{ discount.name }}</span>
          </div>

          <!-- Discount ID -->
          <div class="info-group">
            <label class="info-label">ID mã giảm giá</label>
            <span class="info-value discount-id">#{{ discount.discountId }}</span>
          </div>

          <!-- Categories -->
          <div class="info-group full-width">
            <label class="info-label">Loại xe áp dụng</label>
            <div v-if="isEditing" class="edit-field">
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
            <div v-else class="category-tags">
              <span 
                v-for="categoryName in discount.categoryNames" 
                :key="categoryName"
                class="category-tag"
              >
                {{ categoryName }}
              </span>
            </div>
          </div>

          <!-- Value -->
          <div class="info-group">
            <label class="info-label">Giá trị giảm giá</label>
            <div v-if="isEditing" class="edit-field">
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
            <span v-else class="info-value discount-value">{{ discount.value }}%</span>
          </div>

          <!-- Status -->
          <div class="info-group">
            <label class="info-label">Trạng thái</label>
            <div v-if="isEditing" class="edit-field">
              <select v-model="form.isActive" class="form-select">
                <option :value="true">Hoạt động</option>
                <option :value="false">Không hoạt động</option>
              </select>
            </div>
            <span v-else class="info-value">
              <span 
                class="status-badge" 
                :class="discount.isActive ? 'status-active' : 'status-inactive'"
              >
                {{ discount.isActive ? 'Hoạt động' : 'Không hoạt động' }}
              </span>
            </span>
          </div>

          <!-- Start Date -->
          <div class="info-group">
            <label class="info-label">Ngày bắt đầu</label>
            <div v-if="isEditing" class="edit-field">
              <input
                v-model="form.startDate"
                type="date"
                class="form-input"
                :class="{ 'error': errors.startDate }"
              />
              <span v-if="errors.startDate" class="error-message">{{ errors.startDate }}</span>
            </div>
            <span v-else class="info-value">{{ formatDateForDisplay(discount.startDate) }}</span>
          </div>

          <!-- End Date -->
          <div class="info-group">
            <label class="info-label">Ngày kết thúc</label>
            <div v-if="isEditing" class="edit-field">
              <input
                v-model="form.endDate"
                type="date"
                class="form-input"
                :class="{ 'error': errors.endDate }"
              />
              <span v-if="errors.endDate" class="error-message">{{ errors.endDate }}</span>
            </div>
            <span v-else class="info-value">{{ formatDateForDisplay(discount.endDate) }}</span>
          </div>

          <!-- Description -->
          <div class="info-group full-width">
            <label class="info-label">Mô tả</label>
            <div v-if="isEditing" class="edit-field">
              <textarea
                v-model="form.description"
                class="form-textarea"
                placeholder="Nhập mô tả mã giảm giá"
                rows="3"
              ></textarea>
            </div>
            <span v-else class="info-value">{{ discount.description || 'Không có mô tả' }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.discount-detail-container {
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
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

.header-actions {
  display: flex;
  gap: 12px;
}

.btn-back,
.btn-edit,
.btn-save,
.btn-cancel,
.btn-delete {
  padding: 8px 16px;
  border: none;
  border-radius: 6px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-back,
.btn-edit {
  background: rgba(255, 255, 255, 0.2);
  border: 1px solid rgba(255, 255, 255, 0.3);
  color: white;
}

.btn-back:hover,
.btn-edit:hover {
  background: rgba(255, 255, 255, 0.3);
}

.btn-save {
  background: #22c55e;
  color: white;
}

.btn-save:hover:not(:disabled) {
  background: #16a34a;
}

.btn-save:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-cancel {
  background: #6b7280;
  color: white;
}

.btn-cancel:hover {
  background: #4b5563;
}

.btn-delete {
  background: #ef4444;
  color: white;
}

.btn-delete:hover {
  background: #dc2626;
}

/* Content */
.content {
  padding: 32px;
}

.info-container {
  max-width: 800px;
  margin: 0 auto;
}

.info-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 32px;
}

.info-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.info-group.full-width {
  grid-column: 1 / -1;
}

.info-label {
  font-size: 14px;
  font-weight: 600;
  color: #6b7280;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.info-value {
  font-size: 16px;
  color: #1f2937;
  font-weight: 500;
  padding: 12px 0;
  border-bottom: 1px solid #f3f4f6;
}

.discount-value {
  color: #059669;
  font-weight: 700;
  font-size: 18px;
}

.discount-id {
  color: #6366f1;
  font-weight: 700;
}

.edit-field {
  display: flex;
  flex-direction: column;
  gap: 6px;
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

.category-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  align-items: center;
  padding: 12px 0;
  border-bottom: 1px solid #f3f4f6;
}

.category-tag {
  background: #dbeafe;
  color: #1e40af;
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 500;
}

.status-badge {
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 500;
}

.status-active {
  background: #dcfce7;
  color: #166534;
}

.status-inactive {
  background: #fef2f2;
  color: #dc2626;
}

/* Responsive */
@media (max-width: 768px) {
  .discount-detail-container {
    margin: 10px;
  }

  .header-content {
    flex-direction: column;
    gap: 16px;
    text-align: center;
  }

  .header-actions {
    flex-wrap: wrap;
    justify-content: center;
  }

  .content {
    padding: 20px;
  }

  .info-grid {
    grid-template-columns: 1fr;
    gap: 24px;
  }

  .category-list {
    grid-template-columns: 1fr;
  }
}
</style>