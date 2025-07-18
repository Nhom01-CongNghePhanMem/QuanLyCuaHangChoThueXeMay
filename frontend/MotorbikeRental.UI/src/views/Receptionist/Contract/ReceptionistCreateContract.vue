<script setup>
import ReceptionistLayout from '@/views/layouts/Receptionist/ReceptionistLayout.vue'
import { customerService } from '../../../api/services/customerService';
import { motorbikeService } from '../../../api/services/motorbikeService';
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()
const customerId = route.query.customerId || null;
const customer = ref(null);
const motorbike = ref(null);
const selectedMotorbike = (motorbike) => {
  motorbike.value = motorbike;
};
onMounted(async () => {
  try {
    if (customerId) {
      const response = await customerService.getById(customerId);
      customer.value = response.data;
    }
  } catch (error) {
    console.error('Error fetching data:', error);
  }
});
</script>

<template>
    <ReceptionistLayout>
        <h1>Create Contract</h1>
    </ReceptionistLayout>
</template>