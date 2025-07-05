<script setup>
import AdminLayout from '@/views/layouts/Admin/AdminLayout.vue'
import { employeeService } from '@/api/services/employeeService'
import EmployeeEdit from '@/components/Employees/EmployeeEdit.vue'
import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const employee = ref(null)
const isLoading = ref(true)

onMounted(async () => {
  const id = route.params.id
  try {
    const response = await employeeService.getEmployeeById(id)
    employee.value = response.data
  } catch (error) {
    console.error('Error fetching employee:', error)
  } finally {
    isLoading.value = false
  }
})

async function updateEmployee(form) {
  try {
    await employeeService.updateEmployee(form.employeeId, form)
    router.push('/admin/employees')
  } catch (error) {
    console.error('Error updating employee:', error)
    alert('Cập nhật thất bại!')
  }
}

function createAccount(form) {
  alert('Tạo tài khoản cho nhân viên: ' + form.fullName)
}
</script>

<template>
  <AdminLayout>
    <div v-if="isLoading">Đang tải...</div>
    <div v-else-if="employee">
      <EmployeeEdit
        :employee="employee"
        @update="updateEmployee"
        @create-account="createAccount"
      />
    </div>
    <div v-else>
      <p>Không tìm thấy nhân viên.</p>
    </div>
  </AdminLayout>
</template>