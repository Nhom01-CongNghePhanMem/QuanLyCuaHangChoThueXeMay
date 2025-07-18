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

const searchValue = ref(props.query.Search || '')
const filterStartDate = ref(props.query.FilterStartDate || '')
const filterEndDate = ref(props.query.FilterEndDate || '')
const filterIsActive = ref(props.query.IsActive !== null ? props.query.IsActive : '')

let searchTimeout = null
watch(searchValue, (newVal) => {
  clearTimeout(searchTimeout)
  searchTimeout = setTimeout(() => {
    emit('updateQuery', { Search: newVal, PageNumber: 1 })
  }, 300)
})

watch(filterStartDate, (newVal) => {
  emit('updateQuery', { FilterStartDate: newVal || null, PageNumber: 1 })
})

watch(filterEndDate, (newVal) => {
  emit('updateQuery', { FilterEndDate: newVal || null, PageNumber: 1 })
})

watch(filterIsActive, (newVal) => {
  emit('updateQuery', { IsActive: newVal === '' ? null : newVal === 'true', PageNumber: 1 })
})

const totalPages = computed(() => Math.ceil(props.totalCount / props.query.PageSize))

function changePage(page) {
  if (page >= 1 && page <= totalPages.value) {
    emit('updateQuery', { PageNumber: page })
  }
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
  router.push(`/admin/discount/${discountId}`)
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
        <h1>Danh sách mã giảm giá</h1>
        <div class="header-actions">
          <button @click="createDiscount" class="btn-create">
            Tạo mã giảm giá
          </button>
          <span class="total-count">{{ totalCount }} mã giảm giá</span>
        </div>
      </div>
    </div>

    <!-- Filters -->
    <div class="filters">
      <div class="search-group">
        <input
          v-model="searchValue"
          type="text"
          placeholder="Tìm kiếm mã giảm giá..."
          class="search-input"
        />
        <button v-if="searchValue" @click="searchValue = ''" class="clear-btn">×</button>
      </div>
      
      <div class="filter-group">
        <div class="filter-item">
          <label>Ngày bắt đầu</label>
          <input v-model="filterStartDate" type="date" class="filter-input" />
        </div>
        
        <div class="filter-item">
          <label>Ngày kết thúc</label>
          <input v-model="filterEndDate" type="date" class="filter-input" />
        </div>
        
        <div class="filter-item">
          <label>Trạng thái</label>
          <select v-model="filterIsActive" class="filter-select">
            <option value="">Tất cả</option>
            <option value="true">Hoạt động</option>
            <option value="false">Không hoạt động</option>
          </select>
        </div>
        
        <button @click="clearFilters" class="btn-clear">
          Xóa bộ lọc
        </button>
      </div>
    </div>

    <!-- Table Info -->
    <div class="table-info" v-if="!isLoading && totalCount > 0">
      Hiển thị {{ (props.query.PageNumber - 1) * props.query.PageSize + 1 }} - 
      {{ Math.min(props.query.PageNumber * props.query.PageSize, totalCount) }} 
      trong {{ totalCount }} mã giảm giá
    </div>

    <!-- Content -->
    <div class="content">
      <!-- Loading -->
      <div v-if="isLoading" class="loading">
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
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="discount in discounts" :key="discount.discountId">
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
        <h3>{{ searchValue ? 'Không tìm thấy kết quả' : 'Chưa có mã giảm giá nào' }}</h3>
        <p v-if="searchValue">Không tìm thấy mã giảm giá nào với từ khóa "{{ searchValue }}"</p>
        <p v-else>Chưa có mã giảm giá nào được tạo trong hệ thống</p>
        <button v-if="searchValue" @click="searchValue = ''" class="btn-primary">
          Xóa tìm kiếm
        </button>
      </div>
    </div>

    <!-- Pagination -->
    <div v-if="totalPages > 1" class="pagination">
      <button
        :disabled="props.query.PageNumber === 1"
        @click="changePage(props.query.PageNumber - 1)"
        class="page-btn"
      >
        ‹
      </button>
      
      <span class="page-info">
        Trang {{ props.query.PageNumber }} / {{ totalPages }}
      </span>
      
      <button
        :disabled="props.query.PageNumber >= totalPages"
        @click="changePage(props.query.PageNumber + 1)"
        class="page-btn"
      >
        ›
      </button>
    </div>
  </div>
</template>

<style scoped>
.discount-container {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  padding: 20px;
  background: white;
  border-radius: 4px;
  border: 1px solid #ddd;
}

.header h1 {
  margin: 0;
  font-size: 24px;
  color: #333;
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 16px;
}

.btn-create {
  background: #007bff;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.btn-create:hover {
  background: #0056b3;
}

.total-count {
  font-size: 14px;
  color: #666;
}

.filters {
  margin-bottom: 20px;
  padding: 20px;
  background: white;
  border-radius: 4px;
  border: 1px solid #ddd;
}

.search-group {
  position: relative;
  margin-bottom: 16px;
  max-width: 400px;
}

.search-input {
  width: 100%;
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.search-input:focus {
  outline: none;
  border-color: #007bff;
}

.clear-btn {
  position: absolute;
  right: 8px;
  top: 50%;
  transform: translateY(-50%);
  background: #6c757d;
  color: white;
  border: none;
  border-radius: 50%;
  width: 20px;
  height: 20px;
  cursor: pointer;
  font-size: 14px;
}

.clear-btn:hover {
  background: #5a6268;
}

.filter-group {
  display: flex;
  gap: 16px;
  align-items: flex-end;
  flex-wrap: wrap;
}

.filter-item {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.filter-item label {
  font-size: 12px;
  color: #666;
  font-weight: 500;
}

.filter-input,
.filter-select {
  padding: 6px 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
  min-width: 120px;
}

.filter-input:focus,
.filter-select:focus {
  outline: none;
  border-color: #007bff;
}

.btn-clear {
  background: #f8f9fa;
  border: 1px solid #ddd;
  color: #6c757d;
  padding: 6px 12px;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
}

.btn-clear:hover {
  background: #e9ecef;
}

.table-info {
  padding: 12px 20px;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  margin-bottom: 20px;
  font-size: 14px;
  color: #666;
}

.content {
  min-height: 400px;
}

.loading {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 60px;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  color: #666;
}

.table-container {
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  overflow-x: auto;
}

.table {
  width: 100%;
  border-collapse: collapse;
  font-size: 14px;
}

.table th {
  background: #f8f9fa;
  padding: 12px;
  text-align: left;
  font-weight: 600;
  color: #333;
  border-bottom: 1px solid #ddd;
}

.table td {
  padding: 12px;
  border-bottom: 1px solid #f0f0f0;
}

.table tr:hover {
  background: #f8f9fa;
}

.discount-id {
  font-weight: 600;
  color: #6366f1;
}

.discount-name {
  font-weight: 500;
  color: #333;
}

.category-list {
  display: flex;
  flex-wrap: wrap;
  gap: 4px;
}

.category-tag {
  background: #e9ecef;
  color: #495057;
  padding: 2px 6px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 500;
}

.discount-value {
  font-weight: 600;
  color: #28a745;
}

.date {
  color: #666;
  font-size: 13px;
}

.status {
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 500;
}

.status-active {
  background: #d4edda;
  color: #155724;
}

.status-inactive {
  background: #f8d7da;
  color: #721c24;
}

.btn-detail {
  background: #007bff;
  color: white;
  border: none;
  padding: 4px 12px;
  border-radius: 4px;
  font-size: 13px;
  cursor: pointer;
}

.btn-detail:hover {
  background: #0056b3;
}

.empty-state {
  text-align: center;
  padding: 60px 20px;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
  color: #666;
}

.empty-state h3 {
  margin: 0 0 8px 0;
  color: #333;
}

.btn-primary {
  background: #007bff;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
}

.btn-primary:hover {
  background: #0056b3;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 12px;
  margin-top: 20px;
  padding: 20px;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.page-btn {
  background: white;
  border: 1px solid #ddd;
  color: #333;
  padding: 6px 10px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.page-btn:hover:not(:disabled) {
  background: #f8f9fa;
}

.page-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.page-info {
  font-size: 14px;
  color: #333;
  font-weight: 500;
}

@media (max-width: 768px) {
  .discount-container {
    padding: 10px;
  }

  .header {
    flex-direction: column;
    gap: 16px;
  }

  .filters {
    padding: 16px;
  }

  .filter-group {
    flex-direction: column;
    align-items: stretch;
  }

  .filter-item {
    width: 100%;
  }

  .table-info {
    padding: 12px 16px;
  }

  .pagination {
    padding: 16px;
  }
}
</style>