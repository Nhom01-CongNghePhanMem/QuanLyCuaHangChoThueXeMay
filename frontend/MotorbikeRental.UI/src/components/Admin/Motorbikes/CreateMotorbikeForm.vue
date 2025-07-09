<script setup>
import { ref } from 'vue'

const props = defineProps({
  form: Object,
  categories: { type: Array, default: () => [] },
  isLoading: { type: Boolean, default: false },
})

const emit = defineEmits(['submit'])

const previewImage = ref(null)

function onSubmit() {
  emit('submit', props.form)
}

function onFileChange(event) {
  const file = event.target.files[0]
  if (file) {
    props.form.FormFile = file
    // Tạo preview ảnh
    const reader = new FileReader()
    reader.onload = (e) => (previewImage.value = e.target.result)
    reader.readAsDataURL(file)
  }
}

function removeImage() {
  props.form.FormFile = null
  previewImage.value = null
  document.querySelector('input[type="file"]').value = ''
}

// Status options
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
</script>

<template>
  <div class="create-motorbike-form">
    <div class="form-container">
      <!-- Form Header -->
      <div class="form-header">
        <div class="header-icon">🏍️</div>
        <h2 class="form-title">Thêm xe máy mới</h2>
        <p class="form-subtitle">Điền thông tin chi tiết về xe máy</p>
      </div>

      <!-- Form Content -->
      <form @submit.prevent="onSubmit" class="motorbike-form">
        <!-- Basic Information Section -->
        <div class="form-section">
          <div class="section-header">
            <h3 class="section-title">📋 Thông tin cơ bản</h3>
          </div>
          <div class="form-grid">
            <div class="form-group">
              <label class="form-label">Tên xe *</label>
              <input
                v-model="props.form.MotorbikeName"
                type="text"
                class="form-input"
                placeholder="VD: Honda Air Blade 125"
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">Loại xe *</label>
              <select v-model="props.form.CategoryId" class="form-select" required>
                <option value="">Chọn loại xe</option>
                <option v-for="cat in categories" :key="cat.categoryId" :value="cat.categoryId">
                  {{ cat.categoryName }}
                </option>
              </select>
            </div>

            <div class="form-group">
              <label class="form-label">Thương hiệu *</label>
              <input
                v-model="props.form.Brand"
                type="text"
                class="form-input"
                placeholder="VD: Honda, Yamaha, SYM..."
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">Năm sản xuất *</label>
              <input
                v-model="props.form.Year"
                type="number"
                class="form-input"
                :min="1990"
                :max="new Date().getFullYear()"
                placeholder="2023"
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">Màu sắc</label>
              <input
                v-model="props.form.Color"
                type="text"
                class="form-input"
                placeholder="VD: Đen, Trắng, Đỏ..."
              />
            </div>

            <div class="form-group">
              <label class="form-label">Dung tích động cơ (cc) *</label>
              <input
                v-model="props.form.EngineCapacity"
                type="number"
                class="form-input"
                placeholder="125"
                min="50"
                required
              />
            </div>
          </div>
        </div>

        <!-- Technical Information Section -->
        <div class="form-section">
          <div class="section-header">
            <h3 class="section-title">🔧 Thông tin kỹ thuật</h3>
          </div>
          <div class="form-grid">
            <div class="form-group">
              <label class="form-label">Biển số xe *</label>
              <input
                v-model="props.form.LicensePlate"
                type="text"
                class="form-input"
                placeholder="VD: 30A-12345"
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">Số khung *</label>
              <input
                v-model="props.form.ChassisNumber"
                type="text"
                class="form-input"
                placeholder="Nhập số khung xe"
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">Số máy *</label>
              <input
                v-model="props.form.EngineNumber"
                type="text"
                class="form-input"
                placeholder="Nhập số máy"
                required
              />
            </div>

            <div class="form-group">
              <label class="form-label">Số km đã đi</label>
              <input
                v-model="props.form.Mileage"
                type="number"
                class="form-input"
                placeholder="0"
                min="0"
              />
            </div>

            <div class="form-group">
              <label class="form-label">Tình trạng xe *</label>
              <select v-model="props.form.MotorbikeConditionStatus" class="form-select" required>
                <option value="">Chọn tình trạng</option>
                <option
                  v-for="condition in conditionOptions"
                  :key="condition.value"
                  :value="condition.value"
                >
                  {{ condition.label }}
                </option>
              </select>
            </div>

            <div class="form-group">
              <label class="form-label">Trạng thái hoạt động *</label>
              <select v-model="props.form.Status" class="form-select" required>
                <option value="">Chọn trạng thái</option>
                <option v-for="status in statusOptions" :key="status.value" :value="status.value">
                  {{ status.label }}
                </option>
              </select>
            </div>
          </div>
        </div>

        <!-- Pricing Section -->
        <div class="form-section">
          <div class="section-header">
            <h3 class="section-title">💰 Thông tin giá thuê</h3>
          </div>
          <div class="form-grid pricing-grid">
            <div class="form-group">
              <label class="form-label">Giá thuê theo giờ (VNĐ) *</label>
              <div class="input-with-suffix">
                <input
                  v-model="props.form.HourlyRate"
                  type="number"
                  class="form-input"
                  placeholder="50000"
                  min="0"
                  required
                />
                <span class="input-suffix">VNĐ/giờ</span>
              </div>
            </div>

            <div class="form-group">
              <label class="form-label">Giá thuê theo ngày (VNĐ) *</label>
              <div class="input-with-suffix">
                <input
                  v-model="props.form.DailyRate"
                  type="number"
                  class="form-input"
                  placeholder="300000"
                  min="0"
                  required
                />
                <span class="input-suffix">VNĐ/ngày</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Description Section -->
        <div class="form-section">
          <div class="section-header">
            <h3 class="section-title">📝 Mô tả & Hình ảnh</h3>
          </div>

          <div class="form-group">
            <label class="form-label">Mô tả chi tiết</label>
            <textarea
              v-model="props.form.Description"
              class="form-textarea"
              placeholder="Mô tả thêm về xe: tình trạng, đặc điểm nổi bật, lưu ý..."
              rows="4"
            ></textarea>
          </div>

          <!-- Image Upload Section -->
          <div class="form-group">
            <label class="form-label">Hình ảnh xe</label>
            <div class="image-upload-area">
              <div v-if="!previewImage" class="upload-placeholder">
                <div class="upload-icon">📷</div>
                <p class="upload-text">Chọn hình ảnh xe máy</p>
                <input type="file" accept="image/*" @change="onFileChange" class="file-input" />
              </div>

              <div v-else class="image-preview">
                <img :src="previewImage" alt="Preview" class="preview-img" />
                <button type="button" @click="removeImage" class="remove-btn">❌</button>
              </div>
            </div>
          </div>
        </div>

        <!-- Form Actions -->
        <div class="form-actions">
          <button type="button" class="btn btn-secondary">↩️ Hủy bỏ</button>
          <button type="submit" class="btn btn-primary" :disabled="isLoading">
            <span v-if="isLoading" class="loading-spinner">⏳</span>
            <span v-else>✅</span>
            {{ isLoading ? 'Đang tạo...' : 'Tạo xe máy' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.create-motorbike-form {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 2rem;
}

.form-container {
  max-width: 900px;
  margin: 0 auto;
  background: white;
  border-radius: 20px;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.form-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 3rem 2rem;
  text-align: center;
}

.header-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
}

.form-title {
  margin: 0 0 0.5rem 0;
  font-size: 2.5rem;
  font-weight: 700;
}

.form-subtitle {
  margin: 0;
  opacity: 0.9;
  font-size: 1.1rem;
}

.motorbike-form {
  padding: 2rem;
}

.form-section {
  margin-bottom: 3rem;
}

.section-header {
  margin-bottom: 1.5rem;
}

.section-title {
  color: #2d3748;
  font-size: 1.3rem;
  font-weight: 600;
  margin: 0;
  padding-bottom: 0.5rem;
  border-bottom: 2px solid #e2e8f0;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 1.5rem;
}

.pricing-grid {
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-label {
  font-weight: 600;
  color: #2d3748;
  margin-bottom: 0.5rem;
  font-size: 0.9rem;
}

.form-input,
.form-select,
.form-textarea {
  padding: 0.75rem 1rem;
  border: 2px solid #e2e8f0;
  border-radius: 12px;
  font-size: 1rem;
  transition: all 0.3s ease;
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
  font-family: inherit;
}

.input-with-suffix {
  position: relative;
}

.input-suffix {
  position: absolute;
  right: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: #718096;
  font-size: 0.9rem;
  font-weight: 500;
}

.image-upload-area {
  border: 2px dashed #cbd5e0;
  border-radius: 12px;
  overflow: hidden;
  transition: all 0.3s ease;
}

.image-upload-area:hover {
  border-color: #667eea;
  background: #f7fafc;
}

.upload-placeholder {
  padding: 3rem 2rem;
  text-align: center;
  position: relative;
  cursor: pointer;
}

.upload-icon {
  font-size: 3rem;
  margin-bottom: 1rem;
}

.upload-text {
  color: #718096;
  font-weight: 500;
  margin: 0;
}

.file-input {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  opacity: 0;
  cursor: pointer;
}

.image-preview {
  position: relative;
  display: inline-block;
  width: 100%;
}

.preview-img {
  width: 100%;
  height: 200px;
  object-fit: cover;
  display: block;
}

.remove-btn {
  position: absolute;
  top: 0.5rem;
  right: 0.5rem;
  background: rgba(0, 0, 0, 0.7);
  border: none;
  border-radius: 50%;
  width: 2rem;
  height: 2rem;
  cursor: pointer;
  font-size: 0.8rem;
}

.form-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  padding-top: 2rem;
  border-top: 2px solid #e2e8f0;
}

.btn {
  padding: 0.75rem 2rem;
  border: none;
  border-radius: 12px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 10px 20px rgba(102, 126, 234, 0.3);
}

.btn-primary:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.btn-secondary {
  background: #e2e8f0;
  color: #2d3748;
}

.btn-secondary:hover {
  background: #cbd5e0;
  transform: translateY(-2px);
}

.loading-spinner {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}

/* Responsive */
@media (max-width: 768px) {
  .create-motorbike-form {
    padding: 1rem;
  }

  .form-header {
    padding: 2rem 1rem;
  }

  .form-title {
    font-size: 2rem;
  }

  .motorbike-form {
    padding: 1rem;
  }

  .form-grid {
    grid-template-columns: 1fr;
  }

  .form-actions {
    flex-direction: column;
  }

  .btn {
    width: 100%;
    justify-content: center;
  }
}
</style>
