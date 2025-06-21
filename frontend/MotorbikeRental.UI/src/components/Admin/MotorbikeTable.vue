<script setup>
import { ref, computed } from 'vue'

const props = defineProps({
  motorbikes: {
    type: Array,
    required: true,
  },
  categories: {
    type: Array,
    required: true,
  },
  brands: {
    type: Array,
    required: true,
  },
  motorbikeStatuses: {
    type: Array,
    required: true,
  },
  totalMotorbikes: {
    type: Number,
    required: true,
  },
});

// Filter states
const selectedCategory = ref('')
const selectedBrand = ref('')
const selectedStatus = ref('')
const searchQuery = ref('')

// Computed filtered motorbikes
const filteredMotorbikes = computed(() => {
  return props.motorbikes.filter(motorbike => {
    const matchesCategory = !selectedCategory.value || motorbike.categoryId === selectedCategory.value
    const matchesBrand = !selectedBrand.value || motorbike.brand === selectedBrand.value
    const matchesStatus = selectedStatus.value === '' || motorbike.status === selectedStatus.value
    const matchesSearch = !searchQuery.value || 
      motorbike.motorbikeName.toLowerCase().includes(searchQuery.value.toLowerCase())
    
    return matchesCategory && matchesBrand && matchesStatus && matchesSearch
  })
})

function getFullPath(path) {
  const rawBaseUrl = import.meta.env.VITE_API_URL;
  const baseUrl = rawBaseUrl.replace(/\/$/, "");
  const cleanPath = path.startsWith("/") ? path : "/" + path;
  return baseUrl + cleanPath;
}

function clearFilters() {
  selectedCategory.value = ''
  selectedBrand.value = ''
  selectedStatus.value = ''
  searchQuery.value = ''
}

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

// Map status number to text and color
function getStatusInfo(statusCode) {
  const statusMap = {
    0: { text: 'Có sẵn', class: 'status-available' },
    1: { text: 'Đang thuê', class: 'status-rented' },
    2: { text: 'Bảo trì', class: 'status-maintenance' },
    3: { text: 'Không hoạt động', class: 'status-inactive' },
    4: { text: 'Đã hư', class: 'status-broken' },
    5: { text: 'Chờ xử lý', class: 'status-pending' }
  }
  
  return statusMap[statusCode] || { text: 'Không xác định', class: 'status-unknown' }
}

function getStatusText(statusCode) {
  return getStatusInfo(statusCode).text
}

function getStatusClass(statusCode) {
  return getStatusInfo(statusCode).class
}
</script>

<template>
  <div class="motorbike-page">
    <!-- Page Header -->
    <div class="page-header">
      <div class="page-title">
        <h1>🏍️ Danh sách xe máy</h1>
        <p class="page-subtitle">Tổng cộng {{ filteredMotorbikes.length }} / {{ totalMotorbikes }} xe</p>
      </div>
      
      <div class="page-actions">
        <button class="btn btn-primary">
          <i class="btn-icon">➕</i>
          Thêm xe mới
        </button>
      </div>
    </div>

    <!-- Filters Card -->
    <div class="filters-card">
      <div class="card-header">
        <h3 class="card-title">🔍 Bộ lọc tìm kiếm</h3>
      </div>
      
      <div class="card-body">
        <div class="filters-grid">
          <!-- Search Input -->
          <div class="form-group">
            <label class="form-label">Tìm kiếm</label>
            <div class="input-wrapper">
              <i class="input-icon">🔍</i>
              <input 
                v-model="searchQuery" 
                type="text" 
                class="form-input"
                placeholder="Nhập tên xe..."
              >
            </div>
          </div>

          <!-- Category Filter -->
          <div class="form-group">
            <label class="form-label">Loại xe</label>
            <select v-model="selectedCategory" class="form-select">
              <option value="">Tất cả loại xe</option>
              <option v-for="cat in categories" :key="cat.categoryId" :value="cat.categoryId">
                {{ cat.categoryName }}
              </option>
            </select>
          </div>

          <!-- Brand Filter -->
          <div class="form-group">
            <label class="form-label">Thương hiệu</label>
            <select v-model="selectedBrand" class="form-select">
              <option value="">Tất cả thương hiệu</option>
              <option v-for="brand in brands" :key="brand" :value="brand">
                {{ brand }}
              </option>
            </select>
          </div>

          <!-- Status Filter -->
          <div class="form-group">
            <label class="form-label">Trạng thái</label>
            <select v-model="selectedStatus" class="form-select">
              <option value="">Tất cả trạng thái</option>
              <option v-for="status in motorbikeStatuses" :key="status" :value="status">
                {{ getStatusText(status) }}
              </option>
            </select>
          </div>

          <!-- Clear Button -->
          <div class="form-group">
            <label class="form-label">&nbsp;</label>
            <button @click="clearFilters" class="btn btn-outline">
              <i class="btn-icon">❌</i>
              Xóa bộ lọc
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Results Section -->
    <div class="results-section">
      <!-- No Results -->
      <div v-if="filteredMotorbikes.length === 0" class="empty-state">
        <div class="empty-icon">🔍</div>
        <h3 class="empty-title">Không tìm thấy xe nào</h3>
        <p class="empty-text">Hãy thử điều chỉnh bộ lọc hoặc từ khóa tìm kiếm</p>
      </div>

      <!-- Motorbikes Grid -->
      <div v-else class="motorbikes-grid">
        <div v-for="motorbike in filteredMotorbikes" :key="motorbike.motorbikeId" class="motorbike-card">
          <!-- Card Image -->
          <div class="card-image">
            <img 
              :src="getFullPath(motorbike.imageUrl)" 
              :alt="motorbike.motorbikeName"
              @error="$event.target.src = '/placeholder-bike.jpg'"
            />
            <div class="status-badge" :class="getStatusClass(motorbike.status)">
              {{ getStatusText(motorbike.status) }}
            </div>
          </div>
          
          <!-- Card Content -->
          <div class="card-content">
            <div class="card-header">
              <h3 class="bike-name">{{ motorbike.motorbikeName }}</h3>
              <span class="bike-id">#{{ motorbike.motorbikeId }}</span>
            </div>
            
            <div class="bike-info">
              <div class="info-row">
                <span class="info-label">Loại:</span>
                <span class="info-value">{{ motorbike.categoryName }}</span>
              </div>
              <div class="info-row">
                <span class="info-label">Hãng:</span>
                <span class="info-value">{{ motorbike.brand }}</span>
              </div>
            </div>

            <div class="pricing-section">
              <div class="price-item">
                <span class="price-label">Giá theo giờ</span>
                <span class="price-value">{{ formatPrice(motorbike.hourlyRate) }}</span>
              </div>
              <div class="price-item">
                <span class="price-label">Giá theo ngày</span>
                <span class="price-value">{{ formatPrice(motorbike.dailyRate) }}</span>
              </div>
            </div>

            <div class="card-actions">
              <button class="btn btn-primary btn-sm" :disabled="motorbike.status !== 0">
                <i class="btn-icon">🏍️</i>
                {{ motorbike.status === 0 ? 'Thuê ngay' : 'Không khả dụng' }}
              </button>
              <button class="btn btn-outline btn-sm">
                <i class="btn-icon">👁️</i>
                Chi tiết
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.motorbike-page {
  background: #f8fafc;
  min-height: 100vh;
  padding: 0;
  margin: 0;
}

/* Page Header */
.page-header {
  background: white;
  padding: 2rem;
  border-bottom: 1px solid #e2e8f0;
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

.page-title h1 {
  font-size: 2rem;
  font-weight: 700;
  color: #1e293b;
  margin: 0 0 0.5rem 0;
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.page-subtitle {
  color: #64748b;
  font-size: 1rem;
  margin: 0;
}

.page-actions {
  display: flex;
  gap: 1rem;
}

/* Cards */
.filters-card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1);
  margin: 0 2rem 2rem 2rem;
  overflow: hidden;
}

.card-header {
  padding: 1.5rem 2rem 0 2rem;
}

.card-title {
  font-size: 1.125rem;
  font-weight: 600;
  color: #334155;
  margin: 0;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.card-body {
  padding: 1.5rem 2rem 2rem 2rem;
}

/* Forms */
.filters-grid {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr 1fr auto;
  gap: 1.5rem;
  align-items: end;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-label {
  font-size: 0.875rem;
  font-weight: 600;
  color: #374151;
  margin-bottom: 0.5rem;
}

.input-wrapper {
  position: relative;
}

.input-icon {
  position: absolute;
  left: 0.75rem;
  top: 50%;
  transform: translateY(-50%);
  font-size: 1rem;
  color: #9ca3af;
}

.form-input,
.form-select {
  width: 100%;
  padding: 0.75rem;
  border: 2px solid #e5e7eb;
  border-radius: 8px;
  font-size: 0.875rem;
  background: white;
  transition: all 0.2s ease;
}

.form-input {
  padding-left: 2.5rem;
}

.form-input:focus,
.form-select:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

/* Buttons */
.btn {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 8px;
  font-size: 0.875rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  text-decoration: none;
}

.btn-sm {
  padding: 0.5rem 1rem;
  font-size: 0.8rem;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3);
}

.btn-primary:disabled {
  background: #9ca3af;
  cursor: not-allowed;
  transform: none;
  box-shadow: none;
}

.btn-outline {
  background: white;
  color: #6b7280;
  border: 2px solid #e5e7eb;
}

.btn-outline:hover {
  background: #f9fafb;
  border-color: #d1d5db;
}

.btn-icon {
  font-size: 1rem;
}

/* Results Section */
.results-section {
  margin: 0 2rem;
}

/* Empty State */
.empty-state {
  text-align: center;
  padding: 4rem 2rem;
  background: white;
  border-radius: 12px;
  box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1);
}

.empty-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
}

.empty-title {
  font-size: 1.5rem;
  font-weight: 600;
  color: #374151;
  margin: 0 0 0.5rem 0;
}

.empty-text {
  color: #6b7280;
  margin: 0;
}

/* Motorbikes Grid */
.motorbikes-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 1.5rem;
}

.motorbike-card {
  background: white;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease;
}

.motorbike-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 25px 0 rgba(0, 0, 0, 0.15);
}

/* Card Image */
.card-image {
  position: relative;
  height: 200px;
  overflow: hidden;
  background: #f3f4f6;
}

.card-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.motorbike-card:hover .card-image img {
  transform: scale(1.05);
}

.status-badge {
  position: absolute;
  top: 1rem;
  right: 1rem;
  padding: 0.375rem 0.75rem;
  border-radius: 20px;
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.025em;
}

/* Status Colors */
.status-available {
  background: #dcfce7;
  color: #166534;
}

.status-rented {
  background: #fee2e2;
  color: #991b1b;
}

.status-maintenance {
  background: #fef3c7;
  color: #92400e;
}

.status-inactive,
.status-broken,
.status-unknown {
  background: #f3f4f6;
  color: #374151;
}

.status-pending {
  background: #dbeafe;
  color: #1e40af;
}

/* Card Content */
.card-content {
  padding: 1.5rem;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.bike-name {
  font-size: 1.25rem;
  font-weight: 700;
  color: #1e293b;
  margin: 0;
}

.bike-id {
  background: #f1f5f9;
  color: #64748b;
  padding: 0.25rem 0.5rem;
  border-radius: 6px;
  font-size: 0.75rem;
  font-weight: 600;
}

.bike-info {
  margin-bottom: 1rem;
}

.info-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 0.5rem;
}

.info-label {
  color: #64748b;
  font-size: 0.875rem;
}

.info-value {
  color: #334155;
  font-weight: 500;
  font-size: 0.875rem;
}

.pricing-section {
  background: #f8fafc;
  border-radius: 8px;
  padding: 1rem;
  margin-bottom: 1.5rem;
}

.price-item {
  display: flex;
  justify-content: space-between;
  margin-bottom: 0.5rem;
}

.price-item:last-child {
  margin-bottom: 0;
}

.price-label {
  color: #64748b;
  font-size: 0.875rem;
}

.price-value {
  color: #059669;
  font-weight: 700;
  font-size: 0.875rem;
}

.card-actions {
  display: flex;
  gap: 0.75rem;
}

.card-actions .btn {
  flex: 1;
}

/* Responsive */
@media (max-width: 1200px) {
  .filters-grid {
    grid-template-columns: 1fr 1fr 1fr;
    gap: 1rem;
  }
  
  .filters-grid .form-group:last-child {
    grid-column: 1 / -1;
    justify-self: start;
  }
}

@media (max-width: 768px) {
  .page-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
    padding: 1.5rem;
  }
  
  .filters-card {
    margin: 0 1rem 1.5rem 1rem;
  }
  
  .card-body {
    padding: 1rem;
  }
  
  .filters-grid {
    grid-template-columns: 1fr;
    gap: 1rem;
  }
  
  .results-section {
    margin: 0 1rem;
  }
  
  .motorbikes-grid {
    grid-template-columns: 1fr;
  }
  
  .card-actions {
    flex-direction: column;
  }
}
</style>