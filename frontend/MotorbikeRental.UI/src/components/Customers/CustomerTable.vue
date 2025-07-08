<script setup>
import { ref, watch, computed } from 'vue'

const props = defineProps({
    customers: {
        type: Array,
        required: true
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
            PageSize: 12
        })
    }
})

const emit = defineEmits(['update-query'])

const genderOptions = {
    0: 'Nam',
    1: 'Nữ', 
    2: 'Khác'
}

// Search
const searchValue = ref(props.query.Search || '')

// Debounced search
let searchTimeout = null
watch(searchValue, (newVal) => {
    clearTimeout(searchTimeout)
    searchTimeout = setTimeout(() => {
        emit('update-query', { 
            Search: newVal, 
            PageNumber: 1 
        })
    }, 300)
})

// Computed properties
const totalPages = computed(() => Math.ceil(props.totalCount / props.query.PageSize))
const showingFrom = computed(() => (props.query.PageNumber - 1) * props.query.PageSize + 1)
const showingTo = computed(() => Math.min(props.query.PageNumber * props.query.PageSize, props.totalCount))

// Functions
function changePage(page) {
    if (page >= 1 && page <= totalPages.value) {
        emit('update-query', { PageNumber: page })
    }
}

function changePageSize(size) {
    emit('update-query', { 
        PageSize: parseInt(size), 
        PageNumber: 1 
    })
}

function clearSearch() {
    searchValue.value = ''
}

function formatDate(dateString) {
    return new Date(dateString).toLocaleDateString('vi-VN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
    })
}

function getGenderText(gender) {
    return genderOptions[gender] || 'Không xác định'
}

function getGenderBadgeClass(gender) {
    switch(gender) {
        case 0: return 'badge-male'
        case 1: return 'badge-female'
        default: return 'badge-other'
    }
}
</script>

<template>
    <div class="customer-table-container">
        <!-- Header -->
        <div class="page-header">
            <h1 class="page-title">Danh sách khách hàng</h1>
            <div class="header-stats">
                <span class="stat-item">
                    Tổng: {{ totalCount }} khách hàng
                </span>
            </div>
        </div>

        <!-- Search Section -->
        <div class="search-section">
            <div class="search-wrapper">
                <div class="search-input-wrapper">
                    <input
                        v-model="searchValue"
                        class="search-input"
                        placeholder="Tìm kiếm khách hàng theo tên hoặc số điện thoại..."
                        type="text"
                    />
                    <button 
                        v-if="searchValue" 
                        @click="clearSearch" 
                        class="clear-btn"
                        title="Xóa tìm kiếm"
                    >
                        ✕
                    </button>
                </div>
                <div class="search-info">
                    <span v-if="searchValue">
                        Tìm kiếm: "{{ searchValue }}" - {{ totalCount }} kết quả
                    </span>
                </div>
            </div>

            <div class="page-size-control">
                <label>Hiển thị:</label>
                <select 
                    :value="props.query.PageSize" 
                    @change="changePageSize($event.target.value)"
                    class="page-size-select"
                >
                    <option :value="6">6</option>
                    <option :value="12">12</option>
                    <option :value="24">24</option>
                    <option :value="50">50</option>
                </select>
                <span>/ trang</span>
            </div>
        </div>

        <!-- Table Info -->
        <div class="table-info" v-if="!isLoading && totalCount > 0">
            <span>
                Hiển thị {{ showingFrom }} - {{ showingTo }} trong {{ totalCount }} khách hàng
            </span>
        </div>

        <!-- Loading State -->
        <div v-if="isLoading" class="loading-container">
            <div class="loading-spinner"></div>
            <p>Đang tải dữ liệu...</p>
        </div>

        <!-- Table -->
        <div v-else class="table-wrapper">
            <table v-if="customers && customers.length > 0" class="customer-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Họ tên</th>
                        <th>Giới tính</th>
                        <th>Số điện thoại</th>
                        <th>Lượt thuê</th>
                        <th>Ngày tạo</th>
                    </tr>
                </thead>
                <tbody>
                    <tr 
                        v-for="customer in customers" 
                        :key="customer.customerId"
                        class="table-row"
                    >
                        <td>
                            <span class="customer-id">#{{ customer.customerId }}</span>
                        </td>
                        <td>
                            <span class="customer-name">{{ customer.fullName }}</span>
                        </td>
                        <td>
                            <span 
                                class="gender-badge"
                                :class="getGenderBadgeClass(customer.gender)"
                            >
                                {{ getGenderText(customer.gender) }}
                            </span>
                        </td>
                        <td>
                            <a :href="`tel:${customer.phoneNumber}`" class="phone-link">
                                {{ customer.phoneNumber }}
                            </a>
                        </td>
                        <td>
                            <span class="rental-count">{{ customer.rentalCount }}</span>
                        </td>
                        <td>
                            <span class="date-text">{{ formatDate(customer.createAt) }}</span>
                        </td>
                    </tr>
                </tbody>
            </table>
            
            <!-- Empty State -->
            <div v-else class="empty-state">
                <div class="empty-icon">🔍</div>
                <h3>{{ searchValue ? 'Không tìm thấy kết quả' : 'Chưa có khách hàng nào' }}</h3>
                <p v-if="searchValue">
                    Không tìm thấy khách hàng nào với từ khóa "{{ searchValue }}"
                </p>
                <p v-else>
                    Chưa có khách hàng nào được đăng ký trong hệ thống
                </p>
                <button v-if="searchValue" @click="clearSearch" class="btn btn-primary">
                    Xóa tìm kiếm
                </button>
            </div>
        </div>

        <!-- Pagination -->
        <div v-if="totalPages > 1" class="pagination-container">
            <div class="pagination">
                <button
                    :disabled="props.query.PageNumber === 1"
                    @click="changePage(1)"
                    class="pagination-btn"
                    title="Trang đầu"
                >
                    ⟪
                </button>
                <button
                    :disabled="props.query.PageNumber === 1"
                    @click="changePage(props.query.PageNumber - 1)"
                    class="pagination-btn"
                >
                    ‹ Trước
                </button>
                
                <div class="pagination-pages">
                    <span class="page-info">
                        Trang {{ props.query.PageNumber }} / {{ totalPages }}
                    </span>
                </div>
                
                <button
                    :disabled="props.query.PageNumber >= totalPages"
                    @click="changePage(props.query.PageNumber + 1)"
                    class="pagination-btn"
                >
                    Sau ›
                </button>
                <button
                    :disabled="props.query.PageNumber >= totalPages"
                    @click="changePage(totalPages)"
                    class="pagination-btn"
                    title="Trang cuối"
                >
                    ⟫
                </button>
            </div>
        </div>
    </div>
</template>

<style scoped>
.customer-table-container {
    background: #fff;
    border-radius: 12px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
    margin: 20px;
    overflow: hidden;
}

.page-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 24px 32px;
    border-bottom: 1px solid #e5e7eb;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
}

.page-title {
    font-size: 24px;
    font-weight: 700;
    margin: 0;
}

.header-stats {
    display: flex;
    gap: 20px;
}

.stat-item {
    display: flex;
    align-items: center;
    gap: 8px;
    font-weight: 500;
}

.search-section {
    padding: 24px 32px;
    background: #f8fafc;
    border-bottom: 1px solid #e5e7eb;
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 20px;
}

.search-wrapper {
    flex: 1;
}

.search-input-wrapper {
    position: relative;
    margin-bottom: 8px;
}

.search-input {
    width: 100%;
    padding: 12px 16px;
    padding-right: 40px;
    border: 2px solid #d1d5db;
    border-radius: 10px;
    font-size: 16px;
    transition: all 0.2s;
}

.search-input:focus {
    outline: none;
    border-color: #3b82f6;
    box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.clear-btn {
    position: absolute;
    right: 12px;
    top: 50%;
    transform: translateY(-50%);
    background: #6b7280;
    color: white;
    border: none;
    border-radius: 50%;
    width: 24px;
    height: 24px;
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 12px;
}

.clear-btn:hover {
    background: #4b5563;
}

.search-info {
    font-size: 14px;
    color: #6b7280;
    font-style: italic;
}

.page-size-control {
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 14px;
    color: #6b7280;
    white-space: nowrap;
}

.page-size-select {
    padding: 6px 10px;
    border: 1px solid #d1d5db;
    border-radius: 6px;
    font-size: 14px;
}

.table-info {
    padding: 12px 32px;
    background: #fff;
    border-bottom: 1px solid #f3f4f6;
    font-size: 14px;
    color: #6b7280;
}

.loading-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    padding: 60px 20px;
    color: #6b7280;
}

.loading-spinner {
    width: 40px;
    height: 40px;
    border: 4px solid #e5e7eb;
    border-top: 4px solid #3b82f6;
    border-radius: 50%;
    animation: spin 1s linear infinite;
    margin-bottom: 16px;
}

@keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}

.table-wrapper {
    overflow-x: auto;
}

.customer-table {
    width: 100%;
    border-collapse: collapse;
    font-size: 14px;
}

.customer-table th {
    background: #f8fafc;
    padding: 16px 12px;
    text-align: left;
    font-weight: 600;
    color: #374151;
    border-bottom: 1px solid #e5e7eb;
}

.customer-table td {
    padding: 16px 12px;
    border-bottom: 1px solid #f3f4f6;
}

.table-row:hover {
    background: #f8fafc;
}

.customer-id {
    font-weight: 600;
    color: #6366f1;
}

.customer-name {
    font-weight: 600;
    color: #1f2937;
}

.gender-badge {
    padding: 4px 8px;
    border-radius: 12px;
    font-size: 12px;
    font-weight: 500;
}

.badge-male {
    background: #dbeafe;
    color: #1e40af;
}

.badge-female {
    background: #fce7f3;
    color: #be185d;
}

.badge-other {
    background: #f3f4f6;
    color: #6b7280;
}

.phone-link {
    color: #059669;
    text-decoration: none;
}

.phone-link:hover {
    text-decoration: underline;
}

.rental-count {
    font-weight: 600;
    color: #059669;
}

.date-text {
    color: #6b7280;
}

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

.btn {
    padding: 8px 16px;
    border-radius: 8px;
    font-size: 14px;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s;
    border: none;
}

.btn-primary {
    background: #3b82f6;
    color: white;
}

.btn-primary:hover {
    background: #2563eb;
}

.pagination-container {
    padding: 20px 32px;
    background: #f8fafc;
    border-top: 1px solid #e5e7eb;
}

.pagination {
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 8px;
}

.pagination-btn {
    padding: 8px 12px;
    border: 1px solid #d1d5db;
    background: white;
    border-radius: 8px;
    cursor: pointer;
    transition: all 0.2s;
    font-size: 14px;
}

.pagination-btn:hover:not(:disabled) {
    background: #f3f4f6;
    border-color: #9ca3af;
}

.pagination-btn:disabled {
    opacity: 0.5;
    cursor: not-allowed;
}

.pagination-pages {
    margin: 0 16px;
}

.page-info {
    font-weight: 600;
    color: #374151;
}

@media (max-width: 768px) {
    .customer-table-container {
        margin: 10px;
    }
    
    .page-header {
        flex-direction: column;
        gap: 16px;
        text-align: center;
    }
    
    .search-section {
        flex-direction: column;
        align-items: stretch;
        gap: 16px;
    }
    
    .page-size-control {
        justify-content: center;
    }
    
    .pagination {
        flex-wrap: wrap;
    }
}
</style>