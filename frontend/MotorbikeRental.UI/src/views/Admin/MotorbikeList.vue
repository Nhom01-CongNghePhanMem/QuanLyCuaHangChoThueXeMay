<template>
  <div class="test-api">
    <h2>🧪 Test API - Danh sách xe máy</h2>

    <!-- Button gọi API -->
    <button @click="testGetMotorbikes" :disabled="loading" class="btn-test">
      {{ loading ? 'Đang gọi API...' : '🚀 Gọi API Test' }}
    </button>

    <!-- Loading state -->
    <div v-if="loading" class="loading">⏳ Đang gọi API, chờ chút...</div>

    <!-- Error state -->
    <div v-if="error" class="error">❌ <strong>Lỗi:</strong> {{ error }}</div>

    <!-- Success - Hiển thị raw JSON -->
    <div v-if="rawResponse" class="raw-response">
      <h3>📋 Raw JSON Response:</h3>
      <pre>{{ JSON.stringify(rawResponse, null, 2) }}</pre>
    </div>

    <!-- Success - Hiển thị data đã format -->
    <div v-if="motorbikes.length > 0" class="motorbike-list">
      <h3>🏍️ Danh sách xe máy ({{ motorbikes.length }} xe):</h3>

      <div class="motorbike-grid">
        <div v-for="bike in motorbikes" :key="bike.MotorbikeId" class="motorbike-card">
          <!-- Hình ảnh xe -->
          <div class="bike-image">
            <img
              :src="bike.ImageUrl || '/images/default-bike.jpg'"
              :alt="bike.MotorbikeName"
              @error="handleImageError"
            />
          </div>

          <!-- Thông tin xe -->
          <div class="bike-info">
            <h4>{{ bike.MotorbikeName }}</h4>
            <p><strong>ID:</strong> {{ bike.MotorbikeId }}</p>
            <p><strong>Hãng:</strong> {{ bike.Brand }}</p>
            <p><strong>Loại:</strong> {{ bike.CategoryName }}</p>
            <p><strong>Giá theo giờ:</strong> {{ formatPrice(bike.HourlyRate) }}</p>
            <p><strong>Giá theo ngày:</strong> {{ formatPrice(bike.DailyRate) }}</p>
            <p>
              <strong>Trạng thái:</strong>
              <span :class="getStatusClass(bike.Status)">
                {{ getStatusText(bike.Status) }}
              </span>
            </p>
          </div>
        </div>
      </div>
    </div>

    <!-- Nếu không có data -->
    <div v-if="!loading && !error && motorbikes.length === 0 && hasTriedFetch" class="no-data">
      📭 Không có xe máy nào trong hệ thống
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { motorbikeService } from '@/api/services/motorbikeService'

// Reactive data
const motorbikes = ref([]) // Danh sách xe máy
const rawResponse = ref(null) // Raw response để debug
const loading = ref(false) // Trạng thái loading
const error = ref(null) // Lưu lỗi
const hasTriedFetch = ref(false) // Đã thử gọi API chưa

// Function gọi API test
const testGetMotorbikes = async () => {
  console.log('🔄 Bắt đầu test API...')

  // Reset state
  loading.value = true
  error.value = null
  motorbikes.value = []
  rawResponse.value = null
  hasTriedFetch.value = true

  try {
    // Gọi API
    const response = await motorbikeService.getAll()

    // Lưu raw response để debug
    rawResponse.value = response

    // Kiểm tra response structure
    if (Array.isArray(response)) {
      motorbikes.value = response
      console.log('✅ Nhận được', response.length, 'xe máy')
    } else if (response.data && Array.isArray(response.data)) {
      motorbikes.value = response.data
      console.log('✅ Nhận được', response.data.length, 'xe máy')
    } else {
      console.warn('⚠️ Response không phải array:', response)
      motorbikes.value = [response] // Nếu chỉ có 1 object
    }

    // Log chi tiết từng xe
    motorbikes.value.forEach((bike, index) => {
      console.log(`🏍️ Xe ${index + 1}:`, {
        id: bike.MotorbikeId,
        name: bike.MotorbikeName,
        brand: bike.Brand,
        hourlyRate: bike.HourlyRate,
        dailyRate: bike.DailyRate,
        status: bike.Status,
      })
    })
  } catch (err) {
    console.error('💥 Lỗi test API:', err)
    error.value = err.message || 'Lỗi không xác định'

    // Log chi tiết lỗi
    if (err.response) {
      console.error('📤 Response error:', {
        status: err.response.status,
        statusText: err.response.statusText,
        data: err.response.data,
      })
      error.value = `HTTP ${err.response.status}: ${err.response.statusText}`
    } else if (err.request) {
      console.error('📡 Network error:', err.request)
      error.value = 'Lỗi kết nối mạng - Kiểm tra backend có chạy không?'
    }
  } finally {
    loading.value = false
  }
}

// Function format giá tiền VND
const formatPrice = (price) => {
  if (!price) return '0₫'
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND',
  }).format(price)
}

// Function lấy text trạng thái
const getStatusText = (status) => {
  const statusMap = {
    0: 'Không khả dụng',
    1: 'Khả dụng',
    2: 'Đang cho thuê',
    3: 'Bảo trì',
    // Thêm các status khác nếu có
  }
  return statusMap[status] || `Trạng thái ${status}`
}

// Function lấy CSS class cho status
const getStatusClass = (status) => {
  const classMap = {
    0: 'status-unavailable',
    1: 'status-available',
    2: 'status-rented',
    3: 'status-maintenance',
  }
  return classMap[status] || 'status-unknown'
}

// Function xử lý lỗi hình ảnh
const handleImageError = (event) => {
  console.warn('🖼️ Lỗi load image:', event.target.src)
  event.target.src = '/images/default-bike.jpg'
}
</script>

<style scoped>
.test-api {
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
}

.btn-test {
  background: #2196f3;
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 6px;
  font-size: 16px;
  cursor: pointer;
  margin-bottom: 20px;
  transition: background 0.3s;
}

.btn-test:hover:not(:disabled) {
  background: #1976d2;
}

.btn-test:disabled {
  background: #ccc;
  cursor: not-allowed;
}

.loading {
  text-align: center;
  padding: 40px;
  font-size: 18px;
  color: #666;
  background: #f5f5f5;
  border-radius: 8px;
  margin: 20px 0;
}

.error {
  background: #ffebee;
  border: 1px solid #f44336;
  border-radius: 8px;
  padding: 20px;
  margin: 20px 0;
  color: #c62828;
}

.raw-response {
  background: #f5f5f5;
  border-radius: 8px;
  padding: 20px;
  margin: 20px 0;
}

.raw-response pre {
  background: #2d2d2d;
  color: #f8f8f2;
  padding: 15px;
  border-radius: 4px;
  overflow-x: auto;
  font-size: 12px;
  line-height: 1.4;
}

.motorbike-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 20px;
  margin-top: 20px;
}

.motorbike-card {
  border: 1px solid #ddd;
  border-radius: 12px;
  overflow: hidden;
  background: white;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  transition:
    transform 0.2s,
    box-shadow 0.2s;
}

.motorbike-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 15px rgba(0, 0, 0, 0.15);
}

.bike-image {
  height: 200px;
  background: #f0f0f0;
  display: flex;
  align-items: center;
  justify-content: center;
}

.bike-image img {
  max-width: 100%;
  max-height: 100%;
  object-fit: cover;
}

.bike-info {
  padding: 20px;
}

.bike-info h4 {
  margin: 0 0 15px 0;
  color: #333;
  font-size: 18px;
  font-weight: bold;
}

.bike-info p {
  margin: 8px 0;
  color: #666;
  font-size: 14px;
}

.bike-info p strong {
  color: #333;
  font-weight: 600;
}

/* Status styling */
.status-available {
  background: #e8f5e8;
  color: #2e7d32;
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: bold;
}

.status-rented {
  background: #ffebee;
  color: #c62828;
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: bold;
}

.status-maintenance {
  background: #fff3e0;
  color: #ef6c00;
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: bold;
}

.status-unavailable {
  background: #f5f5f5;
  color: #757575;
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: bold;
}

.no-data {
  text-align: center;
  padding: 60px 20px;
  color: #999;
  font-size: 18px;
  background: #fafafa;
  border-radius: 12px;
  margin: 20px 0;
}
</style>
