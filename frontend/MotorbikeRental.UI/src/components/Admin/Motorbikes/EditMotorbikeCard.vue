<script setup>
import { ref, onMounted } from 'vue'
import { motorbikeService } from '@/api/services/motorbikeService'
import { categoryService } from '@/api/services/categoryService'
import { useRouter } from 'vue-router'
import { getFullPath } from '@/utils/UrlUtils'

const props = defineProps({
  motorbike: {
    type: Object,
    required: true,
  },
})

const statusOptions = [
  { value: 0, label: 'Có sẵn', class: 'status-available' },
  { value: 1, label: 'Đang thuê', class: 'status-rented' },
  { value: 2, label: 'Bảo trì', class: 'status-maintenance' },
  { value: 3, label: 'Không hoạt động', class: 'status-inactive' },
  { value: 4, label: 'Đã hư', class: 'status-broken' },
  { value: 5, label: 'Chờ xử lý', class: 'status-pending' },
]

const conditionOptions = [
  { value: 0, label: 'Mới' },
  { value: 1, label: 'Như mới' },
  { value: 2, label: 'Tốt' },
  { value: 3, label: 'Khá' },
  { value: 4, label: 'Trung bình' },
]

const router = useRouter()
const form = ref({
  ...props.motorbike,
  formFile: null,
})
const isSaving = ref(false)
const categories = ref([])

onMounted(async () => {
  try {
    const res = await categoryService.getAll()
    categories.value = res.data || res
  } catch (e) {
    categories.value = []
  }
})

function handleFileChange(e) {
  form.value.formFile = e.target.files[0]
}

async function onSubmit() {
  isSaving.value = true
  try {
    const res = await motorbikeService.updateMotorbike(form.value)
    if (res?.success) {
      alert('Cập nhật thành công!')
      router.push('/admin/motorbike/detail/' + form.value.motorbikeId)
    } else {
      alert(res?.message || 'Cập nhật thất bại!')
    }
  } catch (e) {
    alert('Có lỗi xảy ra!')
  } finally {
    isSaving.value = false
  }
}

function goBack() {
  router.push('/admin/motorbike/detail/' + form.value.motorbikeId)
}
</script>

<template>
  <div class="edit-motorbike-container">
    <!-- Header -->
    <div class="page-header">
      <div class="header-content">
        <div class="header-info">
          <h1 class="page-title">
            <i class="title-icon">✏️</i>
            Chỉnh sửa xe máy
          </h1>
          <p class="page-subtitle">Cập nhật thông tin xe máy {{ form.motorbikeName }}</p>
        </div>
        <div class="header-actions">
          <button type="button" @click="goBack" class="btn btn-outline">
            <i class="btn-icon">←</i>
            Quay lại
          </button>
        </div>
      </div>
    </div>

    <!-- Form -->
    <form @submit.prevent="onSubmit" class="edit-form">
      <!-- Basic Information Card -->
      <div class="form-card">
        <div class="card-header">
          <h3 class="card-title">
            <i class="card-icon">📝</i>
            Thông tin cơ bản
          </h3>
        </div>
        <div class="card-content">
          <div class="form-grid">
            <div class="form-group">
              <label class="form-label">Tên xe *</label>
              <input v-model="form.motorbikeName" class="form-input" required placeholder="Nhập tên xe" />
            </div>

            <div class="form-group">
              <label class="form-label">Loại xe *</label>
              <select v-model="form.categoryId" class="form-select" required>
                <option value="" disabled>Chọn loại xe</option>
                <option v-for="cat in categories" :key="cat.categoryId" :value="cat.categoryId">
                  {{ cat.categoryName }}
                </option>
              </select>
            </div>

            <div class="form-group">
              <label class="form-label">Biển số *</label>
              <input v-model="form.licensePlate" class="form-input" required placeholder="Nhập biển số" />
            </div>

            <div class="form-group">
              <label class="form-label">Thương hiệu *</label>
              <input v-model="form.brand" class="form-input" required placeholder="Nhập thương hiệu" />
            </div>

            <div class="form-group">
              <label class="form-label">Năm sản xuất *</label>
              <input v-model="form.year" type="number" class="form-input" required placeholder="Nhập năm sản xuất" />
            </div>

            <div class="form-group">
              <label class="form-label">Màu sắc *</label>
              <input v-model="form.color" class="form-input" required placeholder="Nhập màu sắc" />
            </div>
          </div>
        </div>
      </div>

      <!-- Pricing Card -->
      <div class="form-card">
        <div class="card-header">
          <h3 class="card-title">
            <i class="card-icon">💰</i>
            Thông tin giá cả
          </h3>
        </div>
        <div class="card-content">
          <div class="form-grid">
            <div class="form-group">
              <label class="form-label">Giá theo giờ (VNĐ) *</label>
              <input v-model="form.hourlyRate" type="number" class="form-input" required placeholder="Nhập giá theo giờ" />
            </div>

            <div class="form-group">
              <label class="form-label">Giá theo ngày (VNĐ) *</label>
              <input v-model="form.dailyRate" type="number" class="form-input" required placeholder="Nhập giá theo ngày" />
            </div>
          </div>
        </div>
      </div>

      <!-- Technical Information Card -->
      <div class="form-card">
        <div class="card-header">
          <h3 class="card-title">
            <i class="card-icon">🔧</i>
            Thông tin kỹ thuật
          </h3>
        </div>
        <div class="card-content">
          <div class="form-grid">
            <div class="form-group">
              <label class="form-label">Dung tích động cơ (cc) *</label>
              <input v-model="form.engineCapacity" type="number" class="form-input" required placeholder="Nhập dung tích" />
            </div>

            <div class="form-group">
              <label class="form-label">Số khung *</label>
              <input v-model="form.chassisNumber" class="form-input" required placeholder="Nhập số khung" />
            </div>

            <div class="form-group">
              <label class="form-label">Số máy *</label>
              <input v-model="form.engineNumber" class="form-input" required placeholder="Nhập số máy" />
            </div>

            <div class="form-group">
              <label class="form-label">Số km đã đi</label>
              <input v-model="form.mileage" type="number" class="form-input" placeholder="Nhập số km" />
            </div>
          </div>

          <div class="form-group full-width">
            <label class="form-label">Mô tả</label>
            <textarea v-model="form.description" class="form-textarea" rows="4" placeholder="Nhập mô tả chi tiết về xe"></textarea>
          </div>
        </div>
      </div>

      <!-- Status & Condition Card -->
      <div class="form-card">
        <div class="card-header">
          <h3 class="card-title">
            <i class="card-icon">🏷️</i>
            Tình trạng và trạng thái
          </h3>
        </div>
        <div class="card-content">
          <div class="form-grid">
            <div class="form-group">
              <label class="form-label">Tình trạng xe *</label>
              <select v-model="form.motorbikeConditionStatus" class="form-select" required>
                <option value="" disabled>Chọn tình trạng</option>
                <option v-for="option in conditionOptions" :key="option.value" :value="option.value">
                  {{ option.label }}
                </option>
              </select>
            </div>

            <div class="form-group">
              <label class="form-label">Trạng thái hoạt động *</label>
              <select v-model="form.status" class="form-select" required>
                <option value="" disabled>Chọn trạng thái</option>
                <option v-for="option in statusOptions" :key="option.value" :value="option.value">
                  {{ option.label }}
                </option>
              </select>
            </div>
          </div>
        </div>
      </div>

      <!-- Image Card -->
      <div class="form-card">
        <div class="card-header">
          <h3 class="card-title">
            <i class="card-icon">📷</i>
            Hình ảnh xe
          </h3>
        </div>
        <div class="card-content">
          <div class="image-section">
            <div class="image-upload">
              <label class="form-label">Chọn ảnh mới (nếu muốn thay đổi)</label>
              <input type="file" @change="handleFileChange" class="file-input" accept="image/*" />
            </div>
            
            <div v-if="form.imageUrl" class="current-image">
              <p class="image-label">Ảnh hiện tại:</p>
              <div class="image-preview">
                <img :src="getFullPath(form.imageUrl)" alt="Ảnh xe" />
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Form Actions -->
      <div class="form-actions">
        <button type="button" @click="goBack" class="btn btn-outline">
          <i class="btn-icon">❌</i>
          Hủy bỏ
        </button>
        <button type="submit" :disabled="isSaving" class="btn btn-primary">
          <i class="btn-icon">💾</i>
          {{ isSaving ? 'Đang lưu...' : 'Lưu thay đổi' }}
        </button>
      </div>
    </form>
  </div>
</template>

<style scoped>
.edit-motorbike-container {
  max-width: 1200px;
  margin: 0 auto;
}

/* Page Header */
.page-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 2rem;
  border-radius: 12px;
  margin-bottom: 2rem;
  box-shadow: 0 8px 32px rgba(102, 126, 234, 0.3);
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.header-info {
  flex: 1;
}

.page-title {
  font-size: 2rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.title-icon {
  font-size: 2rem;
}

.page-subtitle {
  font-size: 1.125rem;
  opacity: 0.9;
  margin: 0;
}

.header-actions {
  display: flex;
  gap: 1rem;
}

/* Form */
.edit-form {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

/* Form Cards */
.form-card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.card-header {
  background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%);
  padding: 1.5rem 2rem;
  border-bottom: 1px solid #e2e8f0;
}

.card-title {
  font-size: 1.25rem;
  font-weight: 600;
  color: #334155;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.card-icon {
  font-size: 1.5rem;
}

.card-content {
  padding: 2rem;
}

/* Form Grid */
.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
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
  font-weight: 600;
  color: #374151;
  font-size: 0.875rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.form-input,
.form-select,
.form-textarea {
  padding: 0.75rem 1rem;
  border: 2px solid #e5e7eb;
  border-radius: 8px;
  font-size: 1rem;
  transition: all 0.2s ease;
  background: white;
}

.form-input:focus,
.form-select:focus,
.form-textarea:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.form-textarea {
  resize: vertical;
  min-height: 100px;
}

/* Image Section */
.image-section {
  display: grid;
  grid-template-columns: 1fr 300px;
  gap: 2rem;
  align-items: start;
}

.image-upload .form-label {
  margin-bottom: 0.75rem;
}

.file-input {
  padding: 0.75rem;
  border: 2px dashed #d1d5db;
  border-radius: 8px;
  width: 100%;
  transition: all 0.2s ease;
  cursor: pointer;
}

.file-input:hover {
  border-color: #667eea;
  background: #f8fafc;
}

.current-image {
  text-align: center;
}

.image-label {
  font-weight: 600;
  color: #374151;
  margin-bottom: 1rem;
  font-size: 0.875rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.image-preview {
  border: 2px solid #e5e7eb;
  border-radius: 8px;
  overflow: hidden;
  background: #f9fafb;
}

.image-preview img {
  width: 100%;
  height: 200px;
  object-fit: cover;
  display: block;
}

/* Buttons */
.btn {
  padding: 0.75rem 1.5rem;
  border-radius: 8px;
  font-weight: 600;
  font-size: 0.875rem;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  cursor: pointer;
  transition: all 0.2s ease;
  text-decoration: none;
  border: none;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(102, 126, 234, 0.4);
}

.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

.btn-outline {
  background: white;
  color: #374151;
  border: 2px solid #e5e7eb;
}

.btn-outline:hover {
  background: #f9fafb;
  border-color: #d1d5db;
  transform: translateY(-1px);
}

.btn-icon {
  font-size: 1rem;
}

/* Form Actions */
.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  padding: 2rem;
  background: #f8fafc;
  border-radius: 12px;
  border: 1px solid #e2e8f0;
}

/* Responsive */
@media (max-width: 768px) {
  .page-header {
    padding: 1.5rem;
  }

  .header-content {
    flex-direction: column;
    gap: 1rem;
    text-align: center;
  }

  .form-grid {
    grid-template-columns: 1fr;
  }

  .image-section {
    grid-template-columns: 1fr;
  }

  .card-content {
    padding: 1.5rem;
  }

  .form-actions {
    flex-direction: column;
  }
}

@media (max-width: 480px) {
  .page-title {
    font-size: 1.5rem;
  }

  .page-subtitle {
    font-size: 1rem;
  }
}
</style>