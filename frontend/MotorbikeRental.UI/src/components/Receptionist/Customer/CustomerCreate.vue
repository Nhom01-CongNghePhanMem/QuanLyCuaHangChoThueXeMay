<script setup>
import { ref, reactive } from 'vue'

const emit = defineEmits(['submit'])

// Form data
const form = reactive({
    fullName: '',
    gender: 0,
    idNumber: '',
    phoneNumber: '',
    email: ''
})

// Form validation
const errors = ref({})
const isSubmitting = ref(false)

function validateForm() {
    errors.value = {}
    
    if (!form.fullName.trim()) {
        errors.value.fullName = 'Họ tên là bắt buộc'
    }
    
    if (!form.idNumber.trim()) {
        errors.value.idNumber = 'Số CCCD/CMND là bắt buộc'
    } else if (!/^\d{9,12}$/.test(form.idNumber)) {
        errors.value.idNumber = 'Số CCCD/CMND phải từ 9-12 chữ số'
    }
    
    if (!form.phoneNumber.trim()) {
        errors.value.phoneNumber = 'Số điện thoại là bắt buộc'
    } else if (!/^[0-9]{10,11}$/.test(form.phoneNumber)) {
        errors.value.phoneNumber = 'Số điện thoại không hợp lệ'
    }
    
    if (!form.email.trim()) {
        errors.value.email = 'Email là bắt buộc'
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email)) {
        errors.value.email = 'Email không hợp lệ'
    }
    
    return Object.keys(errors.value).length === 0
}

function handleSubmit() {
    if (!validateForm()) return
    
    isSubmitting.value = true
    emit('submit', { ...form })
    
    // Reset submitting state after a delay
    setTimeout(() => {
        isSubmitting.value = false
    }, 1000)
}

function resetForm() {
    Object.assign(form, {
        fullName: '',
        gender: 0,
        idNumber: '',
        phoneNumber: '',
        email: ''
    })
    errors.value = {}
}
</script>

<template>
    <div class="create-customer-container">
        <!-- Header -->
        <div class="form-header">
            <h1 class="form-title">Tạo khách hàng mới</h1>
            <p class="form-subtitle">Nhập thông tin khách hàng để tạo tài khoản mới</p>
        </div>

        <!-- Form -->
        <div class="form-content">
            <form @submit.prevent="handleSubmit" class="customer-form">
                <div class="form-grid">
                    <!-- Full Name -->
                    <div class="form-group">
                        <label class="form-label" for="fullName">
                            Họ và tên <span class="required">*</span>
                        </label>
                        <input
                            id="fullName"
                            v-model="form.fullName"
                            type="text"
                            class="form-input"
                            :class="{ 'error': errors.fullName }"
                            placeholder="Nhập họ và tên"
                        />
                        <span v-if="errors.fullName" class="error-message">{{ errors.fullName }}</span>
                    </div>

                    <!-- Gender -->
                    <div class="form-group">
                        <label class="form-label" for="gender">
                            Giới tính <span class="required">*</span>
                        </label>
                        <select
                            id="gender"
                            v-model="form.gender"
                            class="form-select"
                        >
                            <option :value="0">Nam</option>
                            <option :value="1">Nữ</option>
                            <option :value="2">Khác</option>
                        </select>
                    </div>

                    <!-- ID Number -->
                    <div class="form-group">
                        <label class="form-label" for="idNumber">
                            Số CCCD/CMND <span class="required">*</span>
                        </label>
                        <input
                            id="idNumber"
                            v-model="form.idNumber"
                            type="text"
                            class="form-input"
                            :class="{ 'error': errors.idNumber }"
                            placeholder="Nhập số CCCD/CMND"
                        />
                        <span v-if="errors.idNumber" class="error-message">{{ errors.idNumber }}</span>
                    </div>

                    <!-- Phone Number -->
                    <div class="form-group">
                        <label class="form-label" for="phoneNumber">
                            Số điện thoại <span class="required">*</span>
                        </label>
                        <input
                            id="phoneNumber"
                            v-model="form.phoneNumber"
                            type="tel"
                            class="form-input"
                            :class="{ 'error': errors.phoneNumber }"
                            placeholder="Nhập số điện thoại"
                        />
                        <span v-if="errors.phoneNumber" class="error-message">{{ errors.phoneNumber }}</span>
                    </div>

                    <!-- Email -->
                    <div class="form-group full-width">
                        <label class="form-label" for="email">
                            Email <span class="required">*</span>
                        </label>
                        <input
                            id="email"
                            v-model="form.email"
                            type="email"
                            class="form-input"
                            :class="{ 'error': errors.email }"
                            placeholder="Nhập địa chỉ email"
                        />
                        <span v-if="errors.email" class="error-message">{{ errors.email }}</span>
                    </div>
                </div>

                <!-- Form Actions -->
                <div class="form-actions">
                    <button
                        type="button"
                        @click="resetForm"
                        class="btn btn-secondary"
                        :disabled="isSubmitting"
                    >
                        Làm mới
                    </button>
                    <button
                        type="submit"
                        class="btn btn-primary"
                        :disabled="isSubmitting"
                    >
                        <span v-if="isSubmitting">Đang tạo...</span>
                        <span v-else>Tạo khách hàng</span>
                    </button>
                </div>
            </form>
        </div>
    </div>
</template>

<style scoped>
.create-customer-container {
    background: #fff;
    border-radius: 12px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
    margin: 20px;
    overflow: hidden;
}

.form-header {
    padding: 32px 32px 24px 32px;
    background: linear-gradient(135deg, #22c55e 0%, #16a34a 100%);
    color: white;
    text-align: center;
}

.form-title {
    font-size: 28px;
    font-weight: 700;
    margin: 0 0 8px 0;
}

.form-subtitle {
    font-size: 16px;
    margin: 0;
    opacity: 0.9;
}

.form-content {
    padding: 32px;
}

.customer-form {
    max-width: 800px;
    margin: 0 auto;
}

.form-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 24px;
    margin-bottom: 32px;
}

.form-group {
    display: flex;
    flex-direction: column;
    gap: 8px;
}

.form-group.full-width {
    grid-column: 1 / -1;
}

.form-label {
    font-size: 14px;
    font-weight: 600;
    color: #374151;
}

.required {
    color: #ef4444;
}

.form-input,
.form-select {
    padding: 12px 16px;
    border: 1px solid #d1d5db;
    border-radius: 8px;
    font-size: 16px;
    transition: all 0.2s;
}

.form-input:focus,
.form-select:focus {
    outline: none;
    border-color: #22c55e;
    box-shadow: 0 0 0 3px rgba(34, 197, 94, 0.1);
}

.form-input.error {
    border-color: #ef4444;
}

.error-message {
    font-size: 12px;
    color: #ef4444;
    margin-top: 4px;
}

.form-actions {
    display: flex;
    justify-content: center;
    gap: 16px;
    padding-top: 24px;
    border-top: 1px solid #e5e7eb;
}

.btn {
    padding: 12px 24px;
    border: none;
    border-radius: 8px;
    font-size: 16px;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s;
    min-width: 140px;
}

.btn:disabled {
    opacity: 0.6;
    cursor: not-allowed;
}

.btn-primary {
    background: #22c55e;
    color: white;
}

.btn-primary:hover:not(:disabled) {
    background: #16a34a;
    transform: translateY(-1px);
}

.btn-secondary {
    background: #f3f4f6;
    color: #374151;
    border: 1px solid #d1d5db;
}

.btn-secondary:hover:not(:disabled) {
    background: #e5e7eb;
}

@media (max-width: 768px) {
    .create-customer-container {
        margin: 10px;
    }

    .form-header {
        padding: 24px 20px;
    }

    .form-content {
        padding: 24px 20px;
    }

    .form-grid {
        grid-template-columns: 1fr;
        gap: 20px;
    }

    .form-actions {
        flex-direction: column;
    }

    .btn {
        width: 100%;
    }
}
</style>