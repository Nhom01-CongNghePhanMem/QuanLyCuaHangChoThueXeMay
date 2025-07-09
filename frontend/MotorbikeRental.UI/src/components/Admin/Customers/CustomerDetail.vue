<script setup>
import { ref, watch } from 'vue'

const props = defineProps({
    customer: {
        type: Object,
        required: true
    },
    isLoading: {
        type: Boolean,
        default: false
    }
})

const emit = defineEmits(['update-customer', 'delete-customer'])

// Form data
const form = ref({
    customerId: props.customer.customerId,
    fullName: props.customer.fullName,
    gender: props.customer.gender,
    idNumber: props.customer.idNumber,
    phoneNumber: props.customer.phoneNumber,
    email: props.customer.email
})

// Edit mode
const isEditing = ref(false)

// Watch for customer prop changes
watch(() => props.customer, (newCustomer) => {
    form.value = {
        customerId: newCustomer.customerId,
        fullName: newCustomer.fullName,
        gender: newCustomer.gender,
        idNumber: newCustomer.idNumber,
        phoneNumber: newCustomer.phoneNumber,
        email: newCustomer.email
    }
}, { deep: true })

function toggleEdit() {
    isEditing.value = !isEditing.value
    if (!isEditing.value) {
        // Reset form if canceling
        form.value = {
            customerId: props.customer.customerId,
            fullName: props.customer.fullName,
            gender: props.customer.gender,
            idNumber: props.customer.idNumber,
            phoneNumber: props.customer.phoneNumber,
            email: props.customer.email
        }
    }
}

function saveCustomer() {
    emit('update-customer', form.value)
    isEditing.value = false
}

function deleteCustomer() {
    if (confirm('Bạn có chắc chắn muốn xóa khách hàng này không? Hành động này không thể hoàn tác!')) {
        emit('delete-customer', props.customer.customerId)
    }
}

function formatDate(dateString) {
    return new Date(dateString).toLocaleDateString('vi-VN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
    })
}

function getGenderText(gender) {
    const genderOptions = {
        0: 'Nam',
        1: 'Nữ',
        2: 'Khác'
    }
    return genderOptions[gender] || 'Không xác định'
}
</script>

<template>
    <div class="customer-detail-container">
        <!-- Header -->
        <div class="detail-header">
            <h1 class="customer-name">{{ customer.fullName }}</h1>
            <div class="header-actions">
                <button 
                    v-if="!isEditing" 
                    @click="toggleEdit" 
                    class="btn btn-primary"
                >
                    Chỉnh sửa
                </button>
                <button 
                    v-if="isEditing" 
                    @click="saveCustomer" 
                    class="btn btn-success"
                >
                    Lưu thay đổi
                </button>
                <button 
                    v-if="isEditing" 
                    @click="toggleEdit" 
                    class="btn btn-secondary"
                >
                    Hủy bỏ
                </button>
                <button 
                    @click="deleteCustomer" 
                    class="btn btn-danger"
                >
                    Xóa khách hàng
                </button>
            </div>
        </div>

        <!-- Customer Info -->
        <div class="detail-content">
            <div class="info-section">
                <h2 class="section-title">Thông tin khách hàng</h2>
                <div class="info-grid">
                    <!-- Full Name -->
                    <div class="info-item">
                        <label class="info-label">Họ và tên</label>
                        <input 
                            v-if="isEditing"
                            v-model="form.fullName"
                            type="text"
                            class="info-input"
                            placeholder="Nhập họ và tên"
                        />
                        <span v-else class="info-value">{{ customer.fullName }}</span>
                    </div>

                    <!-- Gender -->
                    <div class="info-item">
                        <label class="info-label">Giới tính</label>
                        <select 
                            v-if="isEditing"
                            v-model="form.gender"
                            class="info-select"
                        >
                            <option :value="0">Nam</option>
                            <option :value="1">Nữ</option>
                            <option :value="2">Khác</option>
                        </select>
                        <span v-else class="info-value">{{ getGenderText(customer.gender) }}</span>
                    </div>

                    <!-- ID Number -->
                    <div class="info-item">
                        <label class="info-label">Số CCCD/CMND</label>
                        <input 
                            v-if="isEditing"
                            v-model="form.idNumber"
                            type="text"
                            class="info-input"
                            placeholder="Nhập số CCCD/CMND"
                        />
                        <span v-else class="info-value">{{ customer.idNumber }}</span>
                    </div>

                    <!-- Phone Number -->
                    <div class="info-item">
                        <label class="info-label">Số điện thoại</label>
                        <input 
                            v-if="isEditing"
                            v-model="form.phoneNumber"
                            type="tel"
                            class="info-input"
                            placeholder="Nhập số điện thoại"
                        />
                        <a v-else :href="`tel:${customer.phoneNumber}`" class="info-value phone-link">
                            {{ customer.phoneNumber }}
                        </a>
                    </div>

                    <!-- Email -->
                    <div class="info-item">
                        <label class="info-label">Email</label>
                        <input 
                            v-if="isEditing"
                            v-model="form.email"
                            type="email"
                            class="info-input"
                            placeholder="Nhập email"
                        />
                        <a v-else :href="`mailto:${customer.email}`" class="info-value email-link">
                            {{ customer.email }}
                        </a>
                    </div>

                    <!-- Rental Count -->
                    <div class="info-item">
                        <label class="info-label">Số lần thuê xe</label>
                        <span class="info-value rental-count">{{ customer.rentalCount }}</span>
                    </div>

                    <!-- Create Date -->
                    <div class="info-item">
                        <label class="info-label">Ngày tạo tài khoản</label>
                        <span class="info-value date-value">{{ formatDate(customer.createAt) }}</span>
                    </div>

                    <!-- Customer ID -->
                    <div class="info-item">
                        <label class="info-label">ID khách hàng</label>
                        <span class="info-value customer-id">#{{ customer.customerId }}</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.customer-detail-container {
    background: #fff;
    border-radius: 12px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
    margin: 20px;
    overflow: hidden;
}

.detail-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 24px 32px;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
}

.customer-name {
    font-size: 28px;
    font-weight: 700;
    margin: 0;
}

.header-actions {
    display: flex;
    gap: 12px;
}

.btn {
    padding: 10px 20px;
    border: none;
    border-radius: 8px;
    font-size: 14px;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s;
}

.btn-primary {
    background: rgba(255, 255, 255, 0.2);
    color: white;
    border: 1px solid rgba(255, 255, 255, 0.3);
}

.btn-primary:hover {
    background: rgba(255, 255, 255, 0.3);
}

.btn-success {
    background: #22c55e;
    color: white;
}

.btn-success:hover {
    background: #16a34a;
}

.btn-secondary {
    background: #6b7280;
    color: white;
}

.btn-secondary:hover {
    background: #4b5563;
}

.btn-danger {
    background: #ef4444;
    color: white;
}

.btn-danger:hover {
    background: #dc2626;
}

.detail-content {
    padding: 32px;
}

.info-section {
    margin-bottom: 32px;
}

.section-title {
    font-size: 20px;
    font-weight: 600;
    color: #1f2937;
    margin-bottom: 24px;
    padding-bottom: 12px;
    border-bottom: 2px solid #e5e7eb;
}

.info-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
    gap: 24px;
}

.info-item {
    display: flex;
    flex-direction: column;
    gap: 8px;
}

.info-label {
    font-size: 14px;
    font-weight: 600;
    color: #374151;
}

.info-value {
    font-size: 16px;
    color: #1f2937;
    padding: 12px 0;
    min-height: 44px;
    display: flex;
    align-items: center;
}

.info-input,
.info-select {
    padding: 12px 16px;
    border: 1px solid #d1d5db;
    border-radius: 8px;
    font-size: 16px;
    transition: border-color 0.2s;
}

.info-input:focus,
.info-select:focus {
    outline: none;
    border-color: #3b82f6;
    box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.phone-link,
.email-link {
    color: #3b82f6;
    text-decoration: none;
}

.phone-link:hover,
.email-link:hover {
    text-decoration: underline;
}

.rental-count {
    font-weight: 600;
    color: #059669;
}

.date-value {
    color: #6b7280;
}

.customer-id {
    font-weight: 600;
    color: #6366f1;
}

@media (max-width: 768px) {
    .customer-detail-container {
        margin: 10px;
    }

    .detail-header {
        flex-direction: column;
        gap: 16px;
        text-align: center;
    }

    .header-actions {
        flex-wrap: wrap;
        justify-content: center;
    }

    .detail-content {
        padding: 20px;
    }

    .info-grid {
        grid-template-columns: 1fr;
    }
}
</style>