<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

const props = defineProps({
  categories: {
    type: Array,
    required: true,
    default: () => []
  }
})

const emit = defineEmits(['create-category'])

function goToDetail(categoryId) {
  router.push(`/admin/category/${categoryId}`)
}

function createCategory() {
  emit('create-category')
}

function formatCurrency(amount) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(amount)
}
</script>

<template>
  <div class="category-container">
    <!-- Header -->
    <div class="category-header">
      <h1 class="category-title">Danh sách loại xe</h1>
      <button class="btn-create" @click="createCategory">
        Thêm loại xe
      </button>
    </div>

    <!-- Categories List -->
    <div class="category-content">
      <div v-if="categories.length === 0" class="empty-state">
        <p>Chưa có loại xe nào</p>
      </div>
      
      <div v-else class="category-grid">
        <div 
          v-for="category in categories" 
          :key="category.categoryId"
          class="category-card"
        >
          <div class="card-content">
            <h3 class="category-name">{{ category.categoryName }}</h3>
            <p class="deposit-amount">
              <span class="label">Tiền cọc:</span>
              <span class="amount">{{ formatCurrency(category.depositAmount) }}</span>
            </p>
            <p class="category-id">
              <span class="label">ID:</span>
              <span class="id-value">#{{ category.categoryId }}</span>
            </p>
          </div>
          <div class="card-actions">
            <button 
              class="btn-detail" 
              @click="goToDetail(category.categoryId)"
            >
              Chi tiết
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.category-container {
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  margin: 20px;
  overflow: hidden;
}

.category-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 24px 32px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.category-title {
  font-size: 24px;
  font-weight: 700;
  margin: 0;
}

.btn-create {
  padding: 8px 16px;
  border-radius: 6px;
  border: 1px solid rgba(255, 255, 255, 0.3);
  background: rgba(255, 255, 255, 0.2);
  color: white;
  font-size: 14px;
  cursor: pointer;
  transition: background 0.2s;
}

.btn-create:hover {
  background: rgba(255, 255, 255, 0.3);
}

.category-content {
  padding: 32px;
}

.empty-state {
  text-align: center;
  padding: 60px 20px;
  color: #6b7280;
}

.category-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 24px;
}

.category-card {
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  padding: 20px;
  transition: all 0.2s;
}

.category-card:hover {
  border-color: #3b82f6;
  box-shadow: 0 4px 12px rgba(59, 130, 246, 0.1);
}

.card-content {
  margin-bottom: 16px;
}

.category-name {
  font-size: 18px;
  font-weight: 600;
  color: #1f2937;
  margin: 0 0 12px 0;
}

.deposit-amount,
.category-id {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin: 8px 0;
  font-size: 14px;
}

.label {
  color: #6b7280;
  font-weight: 500;
}

.amount {
  color: #059669;
  font-weight: 600;
}

.id-value {
  color: #6366f1;
  font-weight: 600;
}

.card-actions {
  display: flex;
  justify-content: flex-end;
  border-top: 1px solid #f3f4f6;
  padding-top: 16px;
}

.btn-detail {
  padding: 6px 14px;
  border-radius: 6px;
  border: 1px solid #d1d5db;
  background: #fff;
  color: #3b82f6;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-detail:hover {
  background: #f3f4f6;
  color: #2563eb;
}

@media (max-width: 768px) {
  .category-container {
    margin: 10px;
  }

  .category-header {
    flex-direction: column;
    gap: 16px;
    text-align: center;
  }

  .category-content {
    padding: 20px;
  }

  .category-grid {
    grid-template-columns: 1fr;
  }
}
</style>