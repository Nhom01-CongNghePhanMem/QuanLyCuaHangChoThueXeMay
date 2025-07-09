<script setup>
import { ref, watch } from 'vue'
import { getFullPath } from '@/utils/UrlUtils'
import { useRouter } from 'vue-router'
import defaultAvatar from '@/assets/image.png'

const route = useRouter()
const props = defineProps({
  employees: {
    type: Array,
    required: true,
  },
  roles: {
    type: Array,
    required: true,
  },
  totalCount: {
    type: Number,
    default: 0,
  },
  query: {
    type: Object,
    required: true,
  },
})

const emit = defineEmits(['update-query', 'create-employee', 'create-account'])

const searchQuery = ref(props.query.Search || '')
const selectedRole = ref(props.query.RoleId || '')
const selectedStatus = ref(props.query.Status || '')
const pageSize = ref(props.query.PageSize || 12)
const pageNumber = ref(props.query.PageNumber || 1)

const statusOptions = {
  0: 'Đang làm',
  1: 'Xin nghỉ',
  2: 'Đã nghỉ',
}

const statusBadgeClass = {
  0: 'status-badge status-badge--active',
  1: 'status-badge status-badge--leave',
  2: 'status-badge status-badge--inactive',
}

function clearFilters() {
  searchQuery.value = ''
  selectedRole.value = ''
  selectedStatus.value = ''
  pageNumber.value = 1
  emitFilters()
}

function emitFilters() {
  emit('update-query', {
    Search: searchQuery.value,
    RoleId: selectedRole.value,
    Status: selectedStatus.value,
    PageNumber: pageNumber.value,
    PageSize: pageSize.value,
  })
}

watch([searchQuery, selectedRole, selectedStatus, pageSize], () => {
  pageNumber.value = 1
  emitFilters()
})

function prevPage() {
  if (pageNumber.value > 1) {
    pageNumber.value--
    emitFilters()
  }
}

function nextPage() {
  if (pageNumber.value < Math.ceil(props.totalCount / pageSize.value)) {
    pageNumber.value++
    emitFilters()
  }
}

function onImgError(event) {
  if (!event.target.src.endsWith('/placeholder-bike.jpg')) {
    event.target.src = '/placeholder-bike.jpg'
  }
}

function createEmployee() {
  route.push('/admin/employees/create')
}

function createAccount(employeeId) {
  route.push('/admin/employees/create-user/' + employeeId)
}

function formatSalary(salary) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND',
  }).format(salary)
}

function formatDateTime(dateStr) {
  if (!dateStr) return ''
  const date = new Date(dateStr)
  if (isNaN(date)) return ''
  date.setHours(date.getHours() + 7)
  return date.toLocaleString('vi-VN', {
    hour: '2-digit',
    minute: '2-digit',
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  })
}

function editEmployee(id) {
  route.push(`/admin/employees/edit/${id}`)
}
</script>

<template>
  <div class="employee-management">
    <!-- Header Section -->
    <div class="page-header">
      <div class="header-content">
        <div class="header-info">
          <h1 class="page-title">
            <i class="page-icon"></i>
            Quản lý nhân viên
          </h1>
          <p class="page-subtitle">Tổng cộng {{ totalCount }} nhân viên</p>
        </div>
        <button @click="createEmployee" class="btn btn-primary">
          <i class="btn-icon">➕</i>
          Thêm nhân viên mới
        </button>
      </div>
    </div>

    <!-- Filters Section -->
    <div class="filters-section">
      <div class="filters-card">
        <div class="card-header">
          <h3 class="card-title">
            <i class="card-icon"></i>
            Bộ lọc tìm kiếm
          </h3>
        </div>
        <div class="card-body">
          <div class="filters-grid">
            <div class="form-group">
              <label class="form-label">Tìm kiếm</label>
              <div class="input-wrapper">
                <i class="input-icon">🔍</i>
                <input
                  v-model="searchQuery"
                  type="text"
                  class="form-input"
                  placeholder="Tên nhân viên, email, số điện thoại..."
                />
              </div>
            </div>

            <div class="form-group">
              <label class="form-label">Quyền hạn</label>
              <div class="select-wrapper">
                <select v-model="selectedRole" class="form-select">
                  <option value="">Tất cả quyền hạn</option>
                  <option v-for="role in roles" :key="role.id" :value="role.id">
                    {{ role.roleName }}
                  </option>
                </select>
                <i class="select-arrow">⌄</i>
              </div>
            </div>

            <div class="form-group">
              <label class="form-label">Trạng thái</label>
              <div class="select-wrapper">
                <select v-model="selectedStatus" class="form-select">
                  <option value="">Tất cả trạng thái</option>
                  <option v-for="(label, code) in statusOptions" :key="code" :value="code">
                    {{ label }}
                  </option>
                </select>
                <i class="select-arrow">⌄</i>
              </div>
            </div>

            <div class="form-group">
              <label class="form-label">&nbsp;</label>
              <button @click="clearFilters" class="btn btn-outline">
                <i class="btn-icon">🗑️</i>
                Xóa bộ lọc
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Employee Grid -->
    <div class="employees-grid">
      <div v-for="emp in employees" :key="emp.employeeId" class="employee-card">
        <div class="card-header-employee">
          <div class="employee-avatar">
            <img
              :src="emp.avatar ? getFullPath(emp.avatar) : defaultAvatar"
              :alt="emp.avatar"
              @error="onImgError"
            />
            <div :class="statusBadgeClass[emp.status]" class="status-indicator"></div>
          </div>
          <div class="employee-basic-info">
            <h3 class="employee-name">{{ emp.fullName }}</h3>
            <div class="employee-role">
              <span v-if="emp.roleName" class="role-badge role-badge--active">
                {{ emp.roleName }}
              </span>
              <span v-else class="role-badge role-badge--empty"> Chưa có tài khoản </span>
            </div>
          </div>
        </div>

        <div class="card-body-employee">
          <div class="employee-detail">
            <div class="detail-item">
              <i class="detail-icon"></i>
              <div class="detail-content">
                <span class="detail-label">Lương</span>
                <span class="detail-value">{{ formatSalary(emp.salary) }}</span>
              </div>
            </div>

            <div class="detail-item">
              <i class="detail-icon"></i>
              <div class="detail-content">
                <span class="detail-label">Trạng thái</span>
                <span :class="statusBadgeClass[emp.status]">
                  {{ statusOptions[emp.status] }}
                </span>
              </div>
            </div>
          </div>
        </div>

        <div class="card-footer-employee">
          <div class="action-buttons">
            <template v-if="!emp.roleName">
              <button @click="createAccount(emp.employeeId)" class="btn btn-success btn-sm">
                <i class="btn-icon">🔑</i>
                Tạo tài khoản
              </button>
              <button class="btn btn-outline btn-sm" @click="editEmployee(emp.employeeId)">
                <i class="btn-icon">✏️</i>
                Chỉnh sửa
              </button>
            </template>
            <template v-else>
              <div class="last-login-info">
                <i class="btn-icon">🕒</i>
                Lần đăng nhập gần nhất:
                <span class="last-login-date">
                  {{ formatDateTime(emp.lastLogin) || 'Chưa từng đăng nhập' }}
                </span>
              </div>
              <button class="btn btn-outline btn-sm" @click="editEmployee(emp.employeeId)">
                <i class="btn-icon">✏️</i>
                Chỉnh sửa
              </button>
            </template>
          </div>
        </div>
      </div>
    </div>

    <!-- Pagination -->
    <div class="pagination-section">
      <div class="pagination-info">
        <span
          >Hiển thị {{ (pageNumber - 1) * pageSize + 1 }}-{{
            Math.min(pageNumber * pageSize, totalCount)
          }}
          trong {{ totalCount }} nhân viên</span
        >
      </div>
      <div class="pagination-controls">
        <button @click="prevPage" :disabled="pageNumber === 1" class="pagination-btn">
          <i class="btn-icon">‹</i>
          Trước
        </button>

        <div class="page-numbers">
          <span class="current-page">{{ pageNumber }}</span>
          <span class="page-separator">/</span>
          <span class="total-pages">{{ Math.ceil(totalCount / pageSize) || 1 }}</span>
        </div>

        <button
          @click="nextPage"
          :disabled="pageNumber >= Math.ceil(totalCount / pageSize)"
          class="pagination-btn"
        >
          Sau
          <i class="btn-icon">›</i>
        </button>

        <div class="page-size-selector">
          <select v-model="pageSize" class="page-size-select">
            <option :value="6">6</option>
            <option :value="12">12</option>
            <option :value="24">24</option>
          </select>
          <span class="page-size-label">/ trang</span>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.employee-management {
  padding: 2rem;
  background: #f8fafc;
  min-height: 100vh;
}

/* Header Section */
.page-header {
  margin-bottom: 2rem;
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  background: white;
  padding: 2rem;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
}

.header-info {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.page-title {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 2rem;
  font-weight: 700;
  color: #1e293b;
  margin: 0;
}

.page-icon {
  font-size: 2.5rem;
}

.page-subtitle {
  color: #64748b;
  font-size: 1.125rem;
  margin: 0;
}
.last-login-info {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.85rem;
  color: #64748b;
  font-style: italic;
}

.last-login-date {
  font-weight: 600;
  color: #6366f1;
}
/* Buttons */
.btn {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1.5rem;
  border-radius: 12px;
  font-weight: 600;
  font-size: 0.875rem;
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
  text-decoration: none;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3);
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
}

.btn-success {
  background: linear-gradient(135deg, #10b981 0%, #059669 100%);
  color: white;
}

.btn-success:hover {
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(16, 185, 129, 0.3);
}

.btn-outline {
  background: white;
  color: #6366f1;
  border: 2px solid #e0e7ff;
}

.btn-outline:hover {
  background: #6366f1;
  color: white;
  border-color: #6366f1;
}

.btn-sm {
  padding: 0.5rem 1rem;
  font-size: 0.8rem;
}

.btn-icon {
  font-size: 1rem;
}

/* Filters Section */
.filters-section {
  margin-bottom: 2rem;
}

.filters-card {
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  overflow: hidden;
}

.card-header {
  background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%);
  padding: 1.5rem 2rem;
  border-bottom: 1px solid #e2e8f0;
}

.card-title {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  font-size: 1.25rem;
  font-weight: 600;
  color: #334155;
  margin: 0;
}

.card-icon {
  font-size: 1.5rem;
}

.card-body {
  padding: 2rem;
}

.filters-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.form-label {
  font-weight: 600;
  color: #374151;
  font-size: 0.875rem;
}

.input-wrapper {
  position: relative;
}

.input-icon {
  position: absolute;
  left: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: #9ca3af;
  font-size: 1rem;
}

.form-input {
  width: 100%;
  padding: 0.75rem 1rem 0.75rem 2.5rem;
  border: 2px solid #e5e7eb;
  border-radius: 12px;
  font-size: 0.875rem;
  transition: all 0.2s ease;
}

.form-input:focus {
  outline: none;
  border-color: #6366f1;
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.1);
}

.select-wrapper {
  position: relative;
}

.form-select {
  width: 100%;
  padding: 0.75rem 2.5rem 0.75rem 1rem;
  border: 2px solid #e5e7eb;
  border-radius: 12px;
  font-size: 0.875rem;
  background: white;
  cursor: pointer;
  transition: all 0.2s ease;
  appearance: none;
}

.form-select:focus {
  outline: none;
  border-color: #6366f1;
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.1);
}

.select-arrow {
  position: absolute;
  right: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: #9ca3af;
  pointer-events: none;
  font-size: 1.2rem;
}

/* Employee Grid */
.employees-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(380px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.employee-card {
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  overflow: hidden;
  transition: all 0.3s ease;
}

.employee-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.12);
}

.card-header-employee {
  padding: 1.5rem;
  background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%);
  display: flex;
  align-items: center;
  gap: 1rem;
}

.employee-avatar {
  position: relative;
}

.employee-avatar img {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  object-fit: cover;
  border: 3px solid white;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.status-indicator {
  position: absolute;
  bottom: 2px;
  right: 2px;
  width: 16px;
  height: 16px;
  border-radius: 50%;
  border: 2px solid white;
}

.employee-basic-info {
  flex: 1;
}

.employee-name {
  font-size: 1.125rem;
  font-weight: 700;
  color: #1e293b;
  margin: 0 0 0.5rem 0;
}

.role-badge {
  display: inline-block;
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.025em;
}

.role-badge--active {
  background: #dbeafe;
  color: #1e40af;
}

.role-badge--empty {
  background: #fee2e2;
  color: #dc2626;
  font-style: italic;
  text-transform: none;
}

.card-body-employee {
  padding: 1.5rem;
}

.employee-detail {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.detail-item {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.detail-icon {
  font-size: 1.25rem;
}

.detail-content {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
  flex: 1;
}

.detail-label {
  font-size: 0.75rem;
  color: #6b7280;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.detail-value {
  font-size: 0.875rem;
  font-weight: 600;
  color: #374151;
}

/* Status Badges */
.status-badge {
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.025em;
}

.status-badge--active {
  background: #dcfce7;
  color: #166534;
}

.status-indicator.status-badge--active {
  background: #22c55e;
}

.status-badge--leave {
  background: #fef3c7;
  color: #92400e;
}

.status-indicator.status-badge--leave {
  background: #f59e0b;
}

.status-badge--inactive {
  background: #fee2e2;
  color: #dc2626;
}

.status-indicator.status-badge--inactive {
  background: #ef4444;
}

.card-footer-employee {
  padding: 1rem 1.5rem;
  background: #f8fafc;
  border-top: 1px solid #e2e8f0;
}

.action-buttons {
  display: flex;
  gap: 0.75rem;
  justify-content: flex-end;
}

/* Pagination */
.pagination-section {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background: white;
  padding: 1.5rem 2rem;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
}

.pagination-info {
  color: #6b7280;
  font-size: 0.875rem;
}

.pagination-controls {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.pagination-btn {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.5rem 1rem;
  background: white;
  border: 2px solid #e5e7eb;
  border-radius: 8px;
  color: #6b7280;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
}

.pagination-btn:hover:not(:disabled) {
  background: #f3f4f6;
  border-color: #d1d5db;
}

.pagination-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.page-numbers {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 600;
}

.current-page {
  color: #6366f1;
  font-size: 1.125rem;
}

.page-separator {
  color: #d1d5db;
}

.total-pages {
  color: #6b7280;
}

.page-size-selector {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.page-size-select {
  padding: 0.5rem;
  border: 2px solid #e5e7eb;
  border-radius: 8px;
  font-size: 0.875rem;
}

.page-size-label {
  color: #6b7280;
  font-size: 0.875rem;
}

/* Responsive */
@media (max-width: 768px) {
  .employee-management {
    padding: 1rem;
  }

  .header-content {
    flex-direction: column;
    gap: 1rem;
    align-items: stretch;
  }

  .filters-grid {
    grid-template-columns: 1fr;
  }

  .employees-grid {
    grid-template-columns: 1fr;
  }

  .pagination-section {
    flex-direction: column;
    gap: 1rem;
  }

  .pagination-controls {
    flex-wrap: wrap;
    justify-content: center;
  }
}
</style>
