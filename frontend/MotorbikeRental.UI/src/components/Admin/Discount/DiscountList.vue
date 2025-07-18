<script setup>
import ReceptionistLayout from '@/views/layouts/Receptionist/ReceptionistLayout.vue'
import { customerService } from '../../../api/services/customerService'
import { motorbikeService } from '../../../api/services/motorbikeService'
import { ref, onMounted } from 'vue'
import { discountService } from '@/api/services/discountService'
import { jwtDecode } from 'jwt-decode'
import { contractStorage } from '@/utils/ContractStorageUtils.js'
import CreateContract from '@/components/Receptionist/Contract/CreateContract.vue'
import { contractService } from '@/api/services/contractService'
import { useRouter } from 'vue-router'

const router = useRouter()
const createContractRef = ref(null)

const customerId = contractStorage.getCustomer()
const motorbikeId = contractStorage.getMotorbike()
const isLoading = ref(true)
const token = localStorage.getItem('token')
let employeeId = null
if (token) {
  const decodedToken = jwtDecode(token)
  employeeId = decodedToken.sub
}

const customer = ref(null)
const motorbike = ref(null)
const discounts = ref([])

onMounted(async () => {
  try {
    if (customerId) {
      const response = await customerService.getById(customerId)
      customer.value = response.data
    }
    if (motorbikeId) {
      const response = await motorbikeService.getById(motorbikeId)
      motorbike.value = response.data // Sửa: thêm .data
    }
    const discountsResponse = await discountService.getAll({ IsActive: true, CategoryId: motorbike.value?.categoryId });
    discounts.value = discountsResponse.data.data
  } catch (error) {
    console.error('Error fetching data:', error)
  } finally {
    isLoading.value = false
  }
})

async function handleCreateContract(params) { // Sửa tên function
  try {
    isLoading.value = true
    await contractService.createContract(params)
    alert('Tạo hợp đồng thành công')
    contractStorage.clear()
    router.push('/receptionist/contracts') // Sửa route
  } catch (error) {
    console.error('Error creating contract:', error)
    alert('Tạo hợp đồng thất bại: ' + (error.response?.data?.message || 'Lỗi không xác định'))
  } finally {
    isLoading.value = false
  }
}

async function handleCalculatePrice(params) { // Sửa tên function
  try {
    const response = await contractService.calculatePrice(params)
    createContractRef.value?.setPriceCalculation(response.data) // Gọi method của child component
  } catch (error) {
    console.error('Error calculating contract price:', error)
    alert('Tính toán giá thất bại')
  }
}
</script>

<template>
  <ReceptionistLayout>
    <CreateContract
      v-if="customer && motorbike"
      ref="createContractRef"
      :customer="customer"
      :motorbike="motorbike"
      :discounts="discounts"
      :isLoading="isLoading"
      @create-contract="handleCreateContract"
      @calculate-price="handleCalculatePrice"
    />
    <div v-else-if="isLoading" class="loading">
      Đang tải dữ liệu...
    </div>
    <div v-else class="error">
      Không tìm thấy thông tin khách hàng hoặc xe máy
    </div>
  </ReceptionistLayout>
</template>

<style scoped>
.loading, .error {
  text-align: center;
  padding: 40px;
  font-size: 16px;
}

.error {
  color: #dc3545;
}
</style>