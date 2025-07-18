<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import ReceptionistLayout from '@/views/layouts/Receptionist/ReceptionistLayout.vue'
import CreateContract from '@/components/Receptionist/Contract/CreateContract.vue'
import { customerService } from '@/api/services/customerService'
import { motorbikeService } from '@/api/services/motorbikeService'
import { discountService } from '@/api/services/discountService'
import { contractService } from '@/api/services/contractService'
import { contractStorage } from '@/utils/ContractStorageUtils.js'

const router = useRouter()
const createContractRef = ref(null)

// State
const customer = ref(null)
const motorbike = ref(null)
const discounts = ref([])
const isLoading = ref(false)
const isInitialLoading = ref(true)

// Get stored IDs
const customerId = contractStorage.getCustomer()
const motorbikeId = contractStorage.getMotorbike()

onMounted(async () => {
  if (!customerId || !motorbikeId) {
    alert('Thiếu thông tin khách hàng hoặc xe máy')
    router.push('/receptionist/customers')
    return
  }

  try {
    // Fetch all data
    const [customerResponse, motorbikeResponse] = await Promise.all([
      customerService.getById(customerId),
      motorbikeService.getById(motorbikeId)
    ])

    customer.value = customerResponse.data
    motorbike.value = motorbikeResponse.data

    // Fetch discounts based on motorbike category
    if (motorbike.value?.categoryId) {
      const discountsResponse = await discountService.getAll({ 
        IsActive: true, 
        CategoryId: motorbike.value.categoryId 
      })
      discounts.value = discountsResponse.data.data || []
    }

  } catch (error) {
    console.error('Error fetching data:', error)
    alert('Có lỗi khi tải dữ liệu: ' + (error.response?.data?.message || error.message))
  } finally {
    isInitialLoading.value = false
  }
})

async function handleCalculatePrice(params) {
  try {
    console.log('Calculating price with params:', params)
    const response = await contractService.calculatePrice(params)
    console.log('Price calculation response:', response)
    
    // Update price calculation in child component
    createContractRef.value?.setPriceCalculation(response.data)
  } catch (error) {
    console.error('Error calculating price:', error)
    alert('Lỗi tính toán giá: ' + (error.response?.data?.message || error.message))
  }
}

async function handleCreateContract(params) {
  try {
    isLoading.value = true
    console.log('Creating contract with params:', params)
    
    const response = await contractService.createContract(params)
    console.log('Create contract response:', response)
    
    alert('✅ Tạo hợp đồng thành công!')
    
    // Clear storage and redirect
    contractStorage.clear()
    router.push('/receptionist/contracts')
    
  } catch (error) {
    console.error('Error creating contract:', error)
    alert('❌ Lỗi tạo hợp đồng: ' + (error.response?.data?.message || error.message))
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <ReceptionistLayout>
    <div v-if="isInitialLoading" class="loading-container">
      <div class="loading-spinner"></div>
      <p>Đang tải dữ liệu...</p>
    </div>

    <div v-else-if="!customer || !motorbike" class="error-container">
      <div class="error-icon">⚠️</div>
      <h3>Không tìm thấy dữ liệu</h3>
      <p>Không tìm thấy thông tin khách hàng hoặc xe máy</p>
      <button @click="router.push('/receptionist/customers')" class="btn btn-primary">
        Quay lại danh sách khách hàng
      </button>
    </div>

    <CreateContract
      v-else
      ref="createContractRef"
      :customer="customer"
      :motorbike="motorbike"
      :discounts="discounts"
      :isLoading="isLoading"
      @calculate-price="handleCalculatePrice"
      @create-contract="handleCreateContract"
    />
  </ReceptionistLayout>
</template>

<style scoped>
.loading-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 400px;
  gap: 20px;
}

.loading-spinner {
  width: 40px;
  height: 40px;
  border: 4px solid #e1e8ed;
  border-top: 4px solid #667eea;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.error-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 400px;
  gap: 20px;
  text-align: center;
}

.error-icon {
  font-size: 48px;
}

.error-container h3 {
  color: #e74c3c;
  margin: 0;
}

.error-container p {
  color: #7f8c8d;
  margin: 0;
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

.btn-primary {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}
</style>