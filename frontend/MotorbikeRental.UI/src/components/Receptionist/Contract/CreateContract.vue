<script setup>
import { ref, computed, watch } from 'vue'
import { getFullPath } from '@/utils/UrlUtils'

const props = defineProps({
  customer: {
    type: Object,
    required: true,
  },
  motorbike: {
    type: Object,
    required: true,
  },
  discounts: {
    type: Array,
    default: () => [],
  },
  isLoading: {
    type: Boolean,
    default: false,
  },
})

const emit = defineEmits(['calculate-price', 'create-contract'])

// Form data
const formData = ref({
  rentalDate: new Date().toISOString().slice(0, 16),
  expectedReturnDate: '',
  rentalTypeStatus: 0, // 0: hourly, 1: daily
  discountId: null,
  totalAmount: 0,
  idCardHeld: false,
  note: ''
})

// Calculation result
const priceCalculation = ref(null)
const isCalculating = ref(false)

// Computed
const maxTotalAmount = computed(() => {
  return priceCalculation.value ? priceCalculation.value.totalPrice : 0
})

const isValidForm = computed(() => {
  return formData.value.rentalDate &&
         formData.value.expectedReturnDate &&
         formData.value.totalAmount > 0 &&
         formData.value.totalAmount <= maxTotalAmount.value &&
         formData.value.idCardHeld
})

// Watch for date changes to auto calculate
watch([
  () => formData.value.rentalDate,
  () => formData.value.expectedReturnDate,
  () => formData.value.rentalTypeStatus,
  () => formData.value.discountId
], () => {
  if (formData.value.rentalDate && formData.value.expectedReturnDate) {
    calculatePrice()
  }
}, { deep: true })

// Functions
async function calculatePrice() {
  if (!formData.value.rentalDate || !formData.value.expectedReturnDate) {
    return
  }
  
  if (new Date(formData.value.expectedReturnDate) <= new Date(formData.value.rentalDate)) {
    alert('Ngày trả dự kiến phải sau ngày thuê')
    return
  }

  const payload = {
    motorbikeId: props.motorbike.motorbikeId,
    rentalDate: formData.value.rentalDate,
    expectedReturnDate: formData.value.expectedReturnDate,
    rentalTypeStatus: parseInt(formData.value.rentalTypeStatus),
    discountId: formData.value.discountId ? parseInt(formData.value.discountId) : null
  }
  
  isCalculating.value = true
  emit('calculate-price', payload)
}

function setPriceCalculation(result) {
  priceCalculation.value = result
  if (result && result.totalPrice) {
    formData.value.totalAmount = result.totalPrice
  }
  isCalculating.value = false
}

function formatPrice(price) {
  if (!price) return '0 ₫'
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function createContract() {
  const payload = {
    customerId: props.customer.customerId,
    motorbikeId: props.motorbike.motorbikeId,
    employeeId: 1,
    rentalDate: formData.value.rentalDate,
    expectedReturnDate: formData.value.expectedReturnDate,
    totalAmount: parseFloat(formData.value.totalAmount),
    discountId: formData.value.discountId ? parseInt(formData.value.discountId) : null,
    rentalTypeStatus: parseInt(formData.value.rentalTypeStatus),
    idCardHeld: formData.value.idCardHeld,
    status: 0,
    note: formData.value.note || ''
  }
  
  emit('create-contract', payload)
}

// Expose method to parent
defineExpose({
  setPriceCalculation
})
</script>

<template>
  <div class="contract-container">
    <!-- Header -->
    <div class="header">
      <h1>Tạo hợp đồng thuê xe</h1>
    </div>

    <div class="content">
      <!-- Info Cards -->
      <div class="info-cards">
        <!-- Customer Card -->
        <div class="card">
          <div class="card-header">
            <h3>👤 Khách hàng</h3>
          </div>
          <div class="card-body">
            <div class="info-list">
              <div class="info-item">
                <span class="label">Họ tên:</span>
                <span class="value">{{ customer?.fullName }}</span>
              </div>
              <div class="info-item">
                <span class="label">Điện thoại:</span>
                <span class="value">{{ customer?.phoneNumber }}</span>
              </div>
              <div class="info-item">
                <span class="label">CCCD:</span>
                <span class="value">{{ customer?.idNumber }}</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Motorbike Card -->
        <div class="card">
          <div class="card-header">
            <h3>🏍️ Xe máy</h3>
          </div>
          <div class="card-body">
            <div class="bike-info">
              <div class="bike-image">
                <img 
                  :src="getFullPath(motorbike?.imageUrl)" 
                  :alt="motorbike?.motorbikeName"
                  @error="$event.target.src = '/placeholder-bike.jpg'"
                >
              </div>
              <div class="bike-details">
                <h4>{{ motorbike?.motorbikeName }}</h4>
                <div class="info-list">
                  <div class="info-item">
                    <span class="label">Biển số:</span>
                    <span class="value">{{ motorbike?.licensePlate }}</span>
                  </div>
                  <div class="info-item">
                    <span class="label">Hãng:</span>
                    <span class="value">{{ motorbike?.brand }}</span>
                  </div>
                  <div class="info-item">
                    <span class="label">Giá/giờ:</span>
                    <span class="value price">{{ formatPrice(motorbike?.hourlyRate) }}</span>
                  </div>
                  <div class="info-item">
                    <span class="label">Giá/ngày:</span>
                    <span class="value price">{{ formatPrice(motorbike?.dailyRate) }}</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Contract Form -->
      <div class="card">
        <div class="card-header">
          <h3>📋 Thông tin hợp đồng</h3>
        </div>
        <div class="card-body">
          <form @submit.prevent="createContract">
            <!-- Basic Info -->
            <div class="form-row">
              <div class="form-group">
                <label>Ngày thuê</label>
                <input 
                  v-model="formData.rentalDate" 
                  type="datetime-local" 
                  required
                >
              </div>
              
              <div class="form-group">
                <label>Ngày trả dự kiến</label>
                <input 
                  v-model="formData.expectedReturnDate" 
                  type="datetime-local" 
                  required
                >
              </div>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label>Loại thuê</label>
                <select v-model="formData.rentalTypeStatus">
                  <option :value="0">Theo giờ</option>
                  <option :value="1">Theo ngày</option>
                </select>
              </div>

              <div class="form-group">
                <label>Khuyến mãi</label>
                <select v-model="formData.discountId">
                  <option :value="null">Không áp dụng</option>
                  <option 
                    v-for="discount in discounts" 
                    :key="discount.discountId" 
                    :value="discount.discountId"
                  >
                    {{ discount.name }} (-{{ discount.value }}%)
                  </option>
                </select>
              </div>
            </div>

            <!-- Price Calculation -->
            <div v-if="isCalculating" class="calculating">
              <div class="loading-spinner"></div>
              <span>Đang tính toán giá...</span>
            </div>

            <div v-else-if="priceCalculation" class="price-summary">
              <h4>💰 Chi tiết giá</h4>
              <div class="price-list">
                <div class="price-item">
                  <span>Giá thuê cơ bản:</span>
                  <span>{{ formatPrice(priceCalculation.rentalPrice) }}</span>
                </div>
                <div v-if="priceCalculation.discountAmount" class="price-item discount">
                  <span>Giảm giá:</span>
                  <span>-{{ formatPrice(priceCalculation.discountAmount) }}</span>
                </div>
                <div class="price-item">
                  <span>Tiền cọc:</span>
                  <span>{{ formatPrice(priceCalculation.depositAmount) }}</span>
                </div>
                <div class="price-item total">
                  <span>Tổng cộng:</span>
                  <span>{{ formatPrice(priceCalculation.totalPrice) }}</span>
                </div>
              </div>
            </div>

            <!-- Contract Amount -->
            <div class="form-group">
              <label>Tổng tiền hợp đồng</label>
              <input 
                v-model.number="formData.totalAmount" 
                type="number" 
                :max="maxTotalAmount"
                placeholder="Nhập tổng tiền"
                required
              >
              <small v-if="maxTotalAmount > 0">
                Tối đa: {{ formatPrice(maxTotalAmount) }}
              </small>
            </div>

            <!-- ID Card Checkbox -->
            <div class="checkbox-group">
              <label class="checkbox-label">
                <input 
                  v-model="formData.idCardHeld" 
                  type="checkbox" 
                  required
                >
                <span class="checkmark"></span>
                Đã giữ CMND/CCCD của khách hàng
              </label>
            </div>

            <!-- Note -->
            <div class="form-group">
              <label>Ghi chú</label>
              <textarea 
                v-model="formData.note" 
                rows="3"
                placeholder="Ghi chú thêm về hợp đồng..."
              ></textarea>
            </div>

            <!-- Actions -->
            <div class="form-actions">
              <button type="button" @click="calculatePrice" class="btn btn-secondary">
                🔄 Tính lại
              </button>
              <button 
                type="submit" 
                :disabled="!isValidForm || isLoading"
                class="btn btn-primary"
              >
                {{ isLoading ? '⏳ Đang tạo...' : '✅ Tạo hợp đồng' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.contract-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.header {
  text-align: center;
  margin-bottom: 30px;
}

.header h1 {
  color: #2c3e50;
  margin: 0;
  font-size: 28px;
  font-weight: 600;
}

.content {
  display: flex;
  flex-direction: column;
  gap: 25px;
}

.info-cards {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 10px rgba(0,0,0,0.08);
  overflow: hidden;
  border: 1px solid #e1e8ed;
}

.card-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 15px 20px;
}

.card-header h3 {
  margin: 0;
  font-size: 16px;
  font-weight: 500;
}

.card-body {
  padding: 20px;
}

.info-list {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.info-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 8px 0;
  border-bottom: 1px solid #f0f0f0;
}

.info-item:last-child {
  border-bottom: none;
}

.label {
  color: #666;
  font-size: 14px;
}

.value {
  font-weight: 500;
  color: #2c3e50;
}

.value.price {
  color: #27ae60;
  font-weight: 600;
}

.bike-info {
  display: flex;
  gap: 15px;
}

.bike-image {
  width: 80px;
  height: 60px;
  border-radius: 8px;
  overflow: hidden;
  flex-shrink: 0;
}

.bike-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.bike-details {
  flex: 1;
}

.bike-details h4 {
  margin: 0 0 10px 0;
  color: #2c3e50;
  font-size: 16px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
  margin-bottom: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.form-group label {
  font-weight: 500;
  color: #2c3e50;
  font-size: 14px;
}

.form-group input,
.form-group select,
.form-group textarea {
  padding: 12px;
  border: 2px solid #e1e8ed;
  border-radius: 8px;
  font-size: 14px;
  transition: border-color 0.3s ease;
}

.form-group input:focus,
.form-group select:focus,
.form-group textarea:focus {
  outline: none;
  border-color: #667eea;
}

.form-group small {
  color: #7f8c8d;
  font-size: 12px;
}

.calculating {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 20px;
  background: #f8f9fa;
  border-radius: 8px;
  margin: 20px 0;
}

.loading-spinner {
  width: 20px;
  height: 20px;
  border: 2px solid #e1e8ed;
  border-top: 2px solid #667eea;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.price-summary {
  background: #f8f9fa;
  border-radius: 12px;
  padding: 20px;
  margin: 20px 0;
  border-left: 4px solid #27ae60;
}

.price-summary h4 {
  margin: 0 0 15px 0;
  color: #2c3e50;
  font-size: 16px;
}

.price-list {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.price-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 8px 0;
}

.price-item.discount {
  color: #27ae60;
}

.price-item.total {
  font-weight: 600;
  font-size: 16px;
  border-top: 2px solid #bdc3c7;
  padding-top: 12px;
  margin-top: 8px;
  color: #e74c3c;
}

.checkbox-group {
  margin: 20px 0;
}

.checkbox-label {
  display: flex;
  align-items: center;
  gap: 10px;
  cursor: pointer;
  font-size: 14px;
  color: #2c3e50;
}

.checkbox-label input[type="checkbox"] {
  width: 18px;
  height: 18px;
  margin: 0;
}

.form-actions {
  display: flex;
  gap: 15px;
  justify-content: flex-end;
  margin-top: 30px;
  padding-top: 20px;
  border-top: 2px solid #ecf0f1;
}

.btn {
  padding: 12px 24px;
  border: none;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s ease;
}

.btn-secondary {
  background: #95a5a6;
  color: white;
}

.btn-secondary:hover {
  background: #7f8c8d;
}

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.btn-primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}

.btn-primary:disabled {
  background: #bdc3c7;
  cursor: not-allowed;
  transform: none;
  box-shadow: none;
}

@media (max-width: 768px) {
  .info-cards {
    grid-template-columns: 1fr;
  }
  
  .form-row {
    grid-template-columns: 1fr;
  }
  
  .bike-info {
    flex-direction: column;
  }
  
  .bike-image {
    width: 100%;
    height: 120px;
  }
  
  .form-actions {
    flex-direction: column;
  }
}
</style>