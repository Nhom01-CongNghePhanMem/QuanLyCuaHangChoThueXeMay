<script setup>
import AdminLayout from '../layouts/Admin/AdminLayout.vue';
import {ref, onMounted} from 'vue';
import { motorbikeService } from '@/api/services/motorbikeService';
import MotorbikeTable from '@/components/Admin/MotorbikeTable.vue';

const PageNumber = ref(1)
const PageSize = 12

const fetchMotorbikes = async (page = 1, size = 12) => {
  try {
    const query = {
      pageNumber: page,
      pageSize: size
    }
    const response = await motorbikeService.getAll(query);
    return response.data;
  } catch (error) {
    console.error('Error fetching motorbikes:', error);
    throw error;
  }
};
const motorbikes = ref([]);
const totalMotorbikes = ref(0);
const categoriesDto = ref([]);
const brands = ref([]);
const motorbikeStatuses = ref([]);
onMounted(async () => {
  try {
    const [motorbikeRes, optionsRes] = await Promise.all([
      motorbikeService.getAll(),
      motorbikeService.getFilterOptions()
    ]);
    motorbikes.value = motorbikeRes.data.data;
    totalMotorbikes.value = motorbikeRes.data.totalCount;
    categoriesDto.value = optionsRes.data.categoriesDto;
    brands.value = optionsRes.data.brands;
    motorbikeStatuses.value = optionsRes.data.motorbikeStatuses;
  } catch (error) {
    console.error('Error fetching motorbikes:', error);
  }
});
</script>
<template>
  <AdminLayout>
    <MotorbikeTable
      :motorbikes="motorbikes"
      :totalMotorbikes="totalMotorbikes"
      :categories="categoriesDto"
      :brands="brands"
      :motorbike-statuses="motorbikeStatuses"
    />
  </AdminLayout>
</template>
<style></style>
