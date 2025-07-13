<script setup>
import { useRouter, useRoute } from 'vue-router'

// Hàm chuyển đổi trạng thái sang tiếng Việt
function getStatusText(status) {
  switch (status) {
    case 0:
      return 'Có sẵn'
    case 1:
      return 'Đang thuê'
    case 2:
      return 'Bảo trì'
    case 3:
      return 'Đã đặt trước'
    case 4:
      return 'Không hoạt động'
    case 5:
      return 'Đã hư hỏng'
    default:
      return 'Không xác định'
  }
}

function getConditionText(condition) {
  switch (condition) {
    case 0:
      return 'Mới'
    case 1:
      return 'Như mới'
    case 2:
      return 'Đã qua sử dụng (tốt)'
    case 3:
      return 'Đã qua sử dụng (khá)'
    default:
      return 'Không xác định'
  }
}

function getStatusColor(status) {
  switch (status) {
    case 0:
      return '#10b981' // green
    case 1:
      return '#f59e0b' // yellow
    case 2:
      return '#6366f1' // indigo
    case 3:
      return '#8b5cf6' // purple
    case 4:
      return '#6b7280' // gray
    case 5:
      return '#ef4444' // red
    default:
      return '#6b7280'
  }
}
const { motorbike } = defineProps({
  motorbike: {
    type: Object,
    required: true,
  },
})
const emit = defineEmits(['delete'])


function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN').format(price)
}
</script>

<template>
  <div class="motorbike-detail">
    <!-- Header Section -->
    <div class="detail-header">
      <div class="header-content">
        <h1 class="motorbike-title">{{ motorbike.motorbikeName }}</h1>
        <div class="status-badge" :style="{ backgroundColor: getStatusColor(motorbike.status) }">
          {{ getStatusText(motorbike.status) }}
        </div>
      </div>
      <div class="header-actions">
        <button class="btn btn-primary">
          <i class="btn-icon">📋</i>
          Tạo hợp đồng thuê
        </button>
      </div>
    </div>

    <!-- Main Content -->
    <div class="detail-content">
      <!-- Image Section -->
      <div class="image-section">
        <div class="image-container">
          <img
            v-if="motorbike.imageUrl"
            :src="
              motorbike.imageUrl.startsWith('/')
                ? 'https://localhost:7060' + motorbike.imageUrl
                : motorbike.imageUrl
            "
            alt="Ảnh xe"
            class="motorbike-image"
          />
          <div v-else class="no-image">
            <i class="no-image-icon">🏍️</i>
            <span>Chưa có ảnh</span>
          </div>
        </div>
      </div>

      <!-- Info Section -->
      <div class="info-section">
        <!-- Basic Info Card -->
        <div class="info-card">
          <div class="card-header">
            <h3 class="card-title">
              <i class="title-icon">📋</i>
              Thông tin cơ bản
            </h3>
          </div>
          <div class="card-content">
            <div class="info-grid">
              <div class="info-item">
                <span class="info-label">Loại xe</span>
                <span class="info-value">{{ motorbike.categoryName }}</span>
              </div>
              <div class="info-item">
                <span class="info-label">Biển số</span>
                <span class="info-value license-plate">{{ motorbike.licensePlate }}</span>
              </div>
              <div class="info-item">
                <span class="info-label">Thương hiệu</span>
                <span class="info-value">{{ motorbike.brand }}</span>
              </div>
              <div class="info-item">
                <span class="info-label">Năm sản xuất</span>
                <span class="info-value">{{ motorbike.year }}</span>
              </div>
              <div class="info-item">
                <span class="info-label">Màu sắc</span>
                <span class="info-value">{{ motorbike.color }}</span>
              </div>
              <div class="info-item">
                <span class="info-label">Dung tích</span>
                <span class="info-value">{{ motorbike.engineCapacity }} cc</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Technical Info Card -->
        <div class="info-card">
          <div class="card-header">
            <h3 class="card-title">
              <i class="title-icon">🔧</i>
              Thông tin kỹ thuật
            </h3>
          </div>
          <div class="card-content">
            <div class="info-grid">
              <div class="info-item">
                <span class="info-label">Số khung</span>
                <span class="info-value code">{{ motorbike.chassisNumber }}</span>
              </div>
              <div class="info-item">
                <span class="info-label">Số máy</span>
                <span class="info-value code">{{ motorbike.engineNumber }}</span>
              </div>
              <div class="info-item">
                <span class="info-label">Tình trạng</span>
                <span class="info-value condition">{{
                  getConditionText(motorbike.motorbikeConditionStatus)
                }}</span>
              </div>
              <div class="info-item">
                <span class="info-label">Số km đã đi</span>
                <span class="info-value">{{
                  motorbike.mileage ? formatPrice(motorbike.mileage) + ' km' : 'Chưa cập nhật'
                }}</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Pricing Card -->
        <div class="info-card pricing-card">
          <div class="card-header">
            <h3 class="card-title">
              <i class="title-icon">💰</i>
              Bảng giá thuê
            </h3>
          </div>
          <div class="card-content">
            <div class="pricing-grid">
              <div class="price-item">
                <div class="price-type">Giá theo giờ</div>
                <div class="price-value">{{ formatPrice(motorbike.hourlyRate) }} VNĐ</div>
                <div class="price-note">/ giờ</div>
              </div>
              <div class="price-item">
                <div class="price-type">Giá theo ngày</div>
                <div class="price-value">{{ formatPrice(motorbike.dailyRate) }} VNĐ</div>
                <div class="price-note">/ ngày</div>
              </div>
            </div>
          </div>
        </div>

        <!-- Description Card -->
        <div class="info-card" v-if="motorbike.description">
          <div class="card-header">
            <h3 class="card-title">
              <i class="title-icon">📝</i>
              Mô tả
            </h3>
          </div>
          <div class="card-content">
            <p class="description-text">{{ motorbike.description === null || motorbike.description === 'null' ? 'Không có mô tả' : motorbike.description }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.motorbike-detail {
  max-width: 1200px;
  margin: 0 auto;
}

/* Header */
.detail-header {
  background: white;
  border-radius: 16px;
  padding: 2rem;
  margin-bottom: 2rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.header-content {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.motorbike-title {
  font-size: 2rem;
  font-weight: 700;
  color: #1e293b;
  margin: 0;
}

.status-badge {
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 20px;
  font-size: 0.875rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.header-actions {
  display: flex;
  gap: 1rem;
}

.btn {
  padding: 0.75rem 1.5rem;
  border-radius: 12px;
  font-weight: 600;
  font-size: 0.875rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  cursor: pointer;
  transition: all 0.2s ease;
  border: none;
}

.btn-outline {
  background: white;
  border: 2px solid #e2e8f0;
  color: #64748b;
}

.btn-outline:hover {
  border-color: #667eea;
  color: #667eea;
  transform: translateY(-1px);
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.btn-primary:hover {
  transform: translateY(-1px);
  box-shadow: 0 8px 20px rgba(102, 126, 234, 0.3);
}

.btn-icon {
  font-size: 1rem;
}

/* Main Content */
.detail-content {
  display: grid;
  grid-template-columns: 400px 1fr;
  gap: 2rem;
}

/* Image Section */
.image-section {
  position: sticky;
  top: 2rem;
  height: fit-content;
}

.image-container {
  background: white;
  border-radius: 16px;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
}

.motorbike-image {
  width: 100%;
  height: 300px;
  object-fit: cover;
  display: block;
}

.no-image {
  height: 300px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  color: #94a3b8;
  background: #f8fafc;
}

.no-image-icon {
  font-size: 3rem;
  margin-bottom: 0.5rem;
}

/* Info Section */
.info-section {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.info-card {
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
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

.title-icon {
  font-size: 1.5rem;
}

.card-content {
  padding: 2rem;
}

/* Info Grid */
.info-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
}

.info-item {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.info-label {
  font-size: 0.875rem;
  font-weight: 500;
  color: #64748b;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.info-value {
  font-size: 1rem;
  font-weight: 600;
  color: #1e293b;
}

.info-value.license-plate {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  text-align: center;
  font-family: 'Courier New', monospace;
  letter-spacing: 0.1em;
  width: fit-content;
}

.info-value.code {
  font-family: 'Courier New', monospace;
  background: #f1f5f9;
  padding: 0.375rem 0.75rem;
  border-radius: 6px;
  color: #475569;
  width: fit-content;
}

.info-value.condition {
  background: #ecfdf5;
  color: #059669;
  padding: 0.375rem 0.75rem;
  border-radius: 20px;
  width: fit-content;
  font-size: 0.875rem;
}

/* Pricing Card */
.pricing-card .card-header {
  background: linear-gradient(135deg, #fef3c7 0%, #fde68a 100%);
}

.pricing-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1.5rem;
}

.price-item {
  text-align: center;
  padding: 1.5rem;
  background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%);
  border-radius: 12px;
  border: 2px solid #e2e8f0;
}

.price-type {
  font-size: 0.875rem;
  font-weight: 500;
  color: #64748b;
  margin-bottom: 0.5rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.price-value {
  font-size: 2rem;
  font-weight: 700;
  color: #1e293b;
  margin-bottom: 0.25rem;
}

.price-note {
  font-size: 0.875rem;
  color: #64748b;
}

/* Description */
.description-text {
  font-size: 1rem;
  line-height: 1.6;
  color: #475569;
  margin: 0;
}
.btn-danger {
  background: linear-gradient(135deg, #ef4444 0%, #f87171 100%);
  color: white;
  border: none;
}
.btn-danger:hover {
  background: linear-gradient(135deg, #dc2626 0%, #f87171 100%);
  color: #fff;
  transform: translateY(-1px);
}
/* Responsive */
@media (max-width: 1024px) {
  .detail-content {
    grid-template-columns: 1fr;
  }

  .image-section {
    position: static;
  }
}

@media (max-width: 768px) {
  .detail-header {
    flex-direction: column;
    gap: 1rem;
    text-align: center;
  }

  .header-actions {
    width: 100%;
    justify-content: center;
  }

  .info-grid {
    grid-template-columns: 1fr;
  }

  .pricing-grid {
    grid-template-columns: 1fr;
  }

  .card-content {
    padding: 1rem;
  }
}
</style>
