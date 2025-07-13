<script setup>
import { ref, watch, computed } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

const props = defineProps({
  discounts: {
    type: Array,
    required: true,
    default: () => []
  },
  isLoading: {
    type: Boolean,
    default: false
  },
  totalCount: {
    type: Number,
    default: 0
  },
  query: {
    type: Object,
    default: () => ({
      Search: '',
      PageNumber: 1,
      PageSize: 12,
      FilterStartDate: null,
      FilterEndDate: null,
      IsActive: null
    })
  }
})

const emit = defineEmits(['updateQuery', 'create-discount'])

// Form state
const searchValue = ref(props.query.Search || '')
const filterStartDate = ref(props.query.FilterStartDate || '')
const filterEndDate = ref(props.query.FilterEndDate || '')
const filterIsActive = ref(props.query.IsActive !== null ? props.query.IsActive : '')

// Search debounce
let searchTimeout = null
watch(searchValue, (newVal) => {
  clearTimeout(searchTimeout)
  searchTimeout = setTimeout(() => {
    emit('updateQuery', { Search: newVal, PageNumber: 1 })
  }, 300)
})

// Filter watchers
watch(filterStartDate, (newVal) => {
  emit('updateQuery', { FilterStartDate: newVal || null, PageNumber: 1 })
})

watch(filterEndDate, (newVal) => {
  emit('updateQuery', { FilterEndDate: newVal || null, PageNumber: 1 })
})

watch(filterIsActive, (newVal) => {
  emit('updateQuery', { IsActive: newVal === '' ? null : newVal === 'true', PageNumber: 1 })
})

// Computed
const totalPages = computed(() => Math.ceil(props.totalCount / props.query.PageSize))
const showingFrom = computed(() => (props.query.PageNumber - 1) * props.query.PageSize + 1)
const showingTo = computed(() => Math.min(props.query.PageNumber * props.query.PageSize, props.totalCount))

// Methods
function changePage(page) {
  if (page >= 1 && page <= totalPages.value) {
    emit('updateQuery', { PageNumber: page })
  }
}

function changePageSize(size) {
  emit('updateQuery', { PageSize: parseInt(size), PageNumber: 1 })
}

function clearSearch() {
  searchValue.value = ''
}

function clearFilters() {
  searchValue.value = ''
  filterStartDate.value = ''
  filterEndDate.value = ''
  filterIsActive.value = ''
  emit('updateQuery', {
    Search: '',
    FilterStartDate: null,
    FilterEndDate: null,
    IsActive: null,
    PageNumber: 1
  })
}

function createDiscount() {
  emit('create-discount')
}

function goToDetail(discountId) {
  router.push(`/admin/discounts/${discountId}`)
}

function formatDate(dateString) {
  return new Date(dateString).toLocaleDateString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit'
  })
}

function getStatusBadgeClass(isActive) {
  return isActive ? 'status-active' : 'status-inactive'
}

function getStatusText(isActive) {
  return isActive ? 'Hoạt động' : 'Không hoạt động'
}
</script>

<template>
  <div class="discount-container">
    <!-- Header -->
    <div class="header">
      <div class="header-content">
        <h1 class="title">Danh sách mã giảm giá</h1>
        <div class="header-actions">
          <button class="btn-create" @click="createDiscount">
            Tạo mã giảm giá
          </button>
          <span class="total-count">{{ totalCount }} mã giảm giá</span>
        </div>
      </div>
    </div>

    <!-- Filters -->
    <div class="filters">
      <!-- Search -->
      <div class="search-group">
        <div class="search-box">
          <input
            v-model="searchValue"
            class="search-input"
            placeholder="Tìm kiếm theo tên mã giảm giá..."
            type="text"
          />
          <button v-if="searchValue" @click="clearSearch" class="clear-search">
            ×
          </button>
        </div>
        <div class="search-info">
          <span v-if="searchValue" class="search-result">
            Tìm kiếm: "{{ searchValue }}" - {{ totalCount }} kết quả
          </span>
        </div>
      </div>

      <!-- Filter Controls -->
      <div class="filter-controls">
        <div class="filter-row">
          <div class="filter-item">
            <label class="filter-label">Ngày bắt đầu</label>
            <input
              v-model="filterStartDate"
              type="date"
              class="filter-input"
            />
          </div>
          
          <div class="filter-item">
            <label class="filter-label">Ngày kết thúc</label>
            <input
              v-model="filterEndDate"
              type="date"
              class="filter-input"
            />
          </div>
          
          <div class="filter-item">
            <label class="filter-label">Trạng thái</label>
            <select v-model="filterIsActive" class="filter-select">
              <option value="">Tất cả</option>
              <option value="true">Hoạt động</option>
              <option value="false">Không hoạt động</option>
            </select>
          </div>
          
          <div class="filter-item">
            <label class="filter-label">Hiển thị</label>
            <select
              :value="props.query.PageSize"
              @change="changePageSize($event.target.value)"
              class="filter-select"
            >
              <option :value="6">6 / trang</option>
              <option :value="12">12 / trang</option>
              <option :value="24">24 / trang</option>
              <option :value="50">50 / trang</option>
            </select>
          </div>
        </div>
        
        <button @click="clearFilters" class="btn-clear">
          Xóa bộ lọc
        </button>
      </div>
    </div>

    <!-- Table Info -->
    <div class="table-info" v-if="!isLoading && totalCount > 0">
      Hiển thị {{ showingFrom }} - {{ showingTo }} trong {{ totalCount }} mã giảm giá
    </div>

    <!-- Content -->
    <div class="content">
      <!-- Loading -->
      <div v-if="isLoading" class="loading">
        <div class="loading-spinner"></div>
        <p>Đang tải dữ liệu...</p>
      </div>

      <!-- Table -->
      <div v-else-if="discounts && discounts.length > 0" class="table-container">
        <table class="table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Tên mã giảm giá</th>
              <th>Loại xe áp dụng</th>
              <th>Giá trị (%)</th>
              <th>Ngày bắt đầu</th>
              <th>Ngày kết thúc</th>
              <th>Trạng thái</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="discount in discounts" :key="discount.discountId" class="table-row">
              <td>
                <span class="discount-id">#{{ discount.discountId }}</span>
              </td>
              <td>
                <span class="discount-name">{{ discount.name }}</span>
              </td>
              <td>
                <div class="category-list">
                  <span 
                    v-for="category in discount.categoryNames" 
                    :key="category"
                    class="category-tag"
                  >
                    {{ category }}
                  </span>
                </div>
              </td>
              <td>
                <span class="discount-value">{{ discount.value }}%</span>
              </td>
              <td>
                <span class="date">{{ formatDate(discount.startDate) }}</span>
              </td>
              <td>
                <span class="date">{{ formatDate(discount.endDate) }}</span>
              </td>
              <td>
                <span class="status" :class="getStatusBadgeClass(discount.isActive)">
                  {{ getStatusText(discount.isActive) }}
                </span>
              </td>
              <td>
                <button class="btn-detail" @click="goToDetail(discount.discountId)">
                  Chi tiết
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Empty State -->
      <div v-else class="empty-state">
        <div class="empty-icon">🏷️</div>
        <h3>{{ searchValue ? 'Không tìm thấy kết quả' : 'Chưa có mã giảm giá nào' }}</h3>
        <p v-if="searchValue">Không tìm thấy mã giảm giá nào với từ khóa "{{ searchValue }}"</p>
        <p v-else>Chưa có mã giảm giá nào được tạo trong hệ thống</p>
        <button v-if="searchValue" @click="clearSearch" class="btn-primary">
          Xóa tìm kiếm
        </button>
      </div>
    </div>

    <!-- Pagination -->
    <div v-if="totalPages > 1" class="pagination">
      <div class="pagination-controls">
        <button
          :disabled="props.query.PageNumber === 1"
          @click="changePage(1)"
          class="pagination-btn"
        >
          ⟪
        </button>
        <button
          :disabled="props.query.PageNumber === 1"
          @click="changePage(props.query.PageNumber - 1)"
          class="pagination-btn"
        >
          ‹
        </button>

        <span class="pagination-info">
          Trang {{ props.query.PageNumber }} / {{ totalPages }}
        </span>

        <button
          :disabled="props.query.PageNumber >= totalPages"
          @click="changePage(props.query.PageNumber + 1)"
          class="pagination-btn"
        >
          ›
        </button>
        <button
          :disabled="props.query.PageNumber >= totalPages"
          @click="changePage(totalPages)"
          class="pagination-btn"
        >
          ⟫
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.discount-container {
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

.header-actions {
  display: flex;
  align-items: center;
  gap: 16px;
}

.btn-create {
  background: rgba(255, 255, 255, 0.2);
  border: 1px solid rgba(255, 255, 255, 0.3);
  color: white;
  padding: 8px 16px;
  border-radius: 6px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-create:hover {
  background: rgba(255, 255, 255, 0.3);
}

.total-count {
  font-size: 14px;
  opacity: 0.9;
}

/* Filters */
.filters {
  background: #f8fafc;
  padding: 20px 32px;
  border-bottom: 1px solid #e5e7eb;
}

.search-group {
  margin-bottom: 16px;
}

.search-box {
  position: relative;
  max-width: 400px;
}

.search-input {
  width: 100%;
  padding: 10px 16px;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  font-size: 14px;
  transition: border-color 0.2s;
}

.search-input:focus {
  outline: none;
  border-color: #3b82f6;
}

.clear-search {
  position: absolute;
  right: 8px;
  top: 50%;
  transform: translateY(-50%);
  background: #6b7280;
  color: white;
  border: none;
  border-radius: 50%;
  width: 20px;
  height: 20px;
  cursor: pointer;
  font-size: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.clear-search:hover {
  background: #4b5563;
}

.search-info {
  margin-top: 8px;
  min-height: 20px;
}

.search-result {
  font-size: 14px;
  color: #6b7280;
  font-style: italic;
}

.filter-controls {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  gap: 16px;
}

.filter-row {
  display: flex;
  gap: 16px;
  flex-wrap: wrap;
}

.filter-item {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.filter-label {
  font-size: 12px;
  color: #6b7280;
  font-weight: 500;
}

.filter-input,
.filter-select {
  padding: 6px 10px;
  border: 1px solid #d1d5db;
  border-radius: 4px;
  font-size: 14px;
  min-width: 120px;
}

.filter-input:focus,
.filter-select:focus {
  outline: none;
  border-color: #3b82f6;
}

.btn-clear {
  background: #f3f4f6;
  border: 1px solid #d1d5db;
  color: #6b7280;
  padding: 6px 12px;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-clear:hover {
  background: #e5e7eb;
}

/* Table Info */
.table-info {
  padding: 12px 32px;
  background: #fff;
  border-bottom: 1px solid #f3f4f6;
  font-size: 14px;
  color: #6b7280;
}

/* Content */
.content {
  min-height: 400px;
}

.loading {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 60px;
  color: #6b7280;
}

.loading-spinner {
  width: 32px;
  height: 32px;
  border: 3px solid #e5e7eb;
  border-top: 3px solid #3b82f6;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-bottom: 16px;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

/* Table */
.table-container {
  overflow-x: auto;
}

.table {
  width: 100%;
  border-collapse: collapse;
  font-size: 14px;
}

.table th {
  background: #f8fafc;
  padding: 12px;
  text-align: left;
  font-weight: 600;
  color: #374151;
  border-bottom: 1px solid #e5e7eb;
}

.table td {
  padding: 12px;
  border-bottom: 1px solid #f3f4f6;
}

.table-row:hover {
  background: #f8fafc;
}

.discount-id {
  font-weight: 600;
  color: #6366f1;
}

.discount-name {
  font-weight: 500;
  color: #1f2937;
}

.category-list {
  display: flex;
  flex-wrap: wrap;
  gap: 4px;
}

.category-tag {
  background: #dbeafe;
  color: #1e40af;
  padding: 2px 6px;
  border-radius: 10px;
  font-size: 12px;
  font-weight: 500;
}

.discount-value {
  font-weight: 600;
  color: #059669;
}

.date {
  color: #6b7280;
  font-size: 13px;
}

.status {
  padding: 4px 8px;
  border-radius: 10px;
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

.btn-detail {
  background: #fff;
  border: 1px solid #d1d5db;
  color: #3b82f6;
  padding: 4px 12px;
  border-radius: 4px;
  font-size: 13px;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-detail:hover {
  background: #f3f4f6;
  color: #2563eb;
}

/* Empty State */
.empty-state {
  text-align: center;
  padding: 60px 20px;
  color: #6b7280;
}

.empty-icon {
  font-size: 48px;
  margin-bottom: 16px;
}

.empty-state h3 {
  margin: 0 0 8px 0;
  color: #374151;
}

.btn-primary {
  background: #3b82f6;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 6px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-primary:hover {
  background: #2563eb;
}

/* Pagination */
.pagination {
  padding: 20px 32px;
  background: #f8fafc;
  border-top: 1px solid #e5e7eb;
}

.pagination-controls {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 8px;
}

.pagination-btn {
  background: white;
  border: 1px solid #d1d5db;
  color: #374151;
  padding: 6px 10px;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.2s;
}

.pagination-btn:hover:not(:disabled) {
  background: #f3f4f6;
}

.pagination-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.pagination-info {
  margin: 0 16px;
  font-size: 14px;
  color: #374151;
  font-weight: 500;
}

/* Responsive */
@media (max-width: 768px) {
  .discount-container {
    margin: 10px;
  }

  .header-content {
    flex-direction: column;
    gap: 16px;
    text-align: center;
  }

  .filters {
    padding: 16px 20px;
  }

  .filter-controls {
    flex-direction: column;
    align-items: stretch;
  }

  .filter-row {
    flex-direction: column;
  }

  .filter-item {
    width: 100%;
  }

  .table-info {
    padding: 12px 20px;
  }

  .pagination {
    padding: 16px 20px;
  }
}
</style>