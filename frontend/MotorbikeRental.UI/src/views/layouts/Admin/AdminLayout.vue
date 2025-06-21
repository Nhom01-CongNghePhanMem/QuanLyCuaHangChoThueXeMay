<!-- src/layouts/AdminLayout.vue - Version đẹp hơn -->
 <script setup>
import { computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()

const currentPageName = computed(() => {
  const names = {
    '/admin': 'Dashboard',
    '/admin/motorbikes': 'Quản lý xe máy',
    '/admin/employees': 'Quản lý nhân viên',
    '/admin/customers': 'Quản lý khách hàng',
    '/admin/reports': 'Báo cáo thống kê',
    '/admin/rentals': 'Lịch sử thuê xe'
  }
  return names[route.path] || 'Admin'
})

const isActive = (path) => {
  if (path === '/admin') {
    return route.path === '/admin'
  }
  return route.path.startsWith(path)
}

const logout = () => {
  if (confirm('Bạn có chắc muốn đăng xuất?')) {
    localStorage.removeItem('token')
    router.push('/login')
  }
}
</script>

<template>
  <div class="admin-layout">
    <!-- Header -->
    <header class="admin-header">
      <div class="header-left">
        <div class="logo">
          <i class="logo-icon">🏍️</i>
          <span class="logo-text">MotorRental</span>
          <span class="logo-badge">Admin</span>
        </div>
      </div>
      
      <div class="header-right">
        <div class="user-menu">
          <div class="user-avatar">
            <i class="avatar-icon">👨‍💼</i>
          </div>
          <div class="user-info">
            <span class="user-name">Admin User</span>
            <span class="user-role">Administrator</span>
          </div>
          <button @click="logout" class="logout-btn">
            <i class="logout-icon">🚪</i>
            Đăng xuất
          </button>
        </div>
      </div>
    </header>

    <div class="admin-body">
      <!-- Sidebar -->
      <aside class="admin-sidebar">
        <nav class="sidebar-nav">
          <div class="nav-section">
            <div class="nav-section-title">MENU CHÍNH</div>
            
            <router-link to="/admin" class="nav-item" :class="{ active: isActive('/admin') }">
              <i class="nav-icon">📊</i>
              <span class="nav-text">Dashboard</span>
            </router-link>

            <router-link to="/admin/motorbikes" class="nav-item" :class="{ active: isActive('/admin/motorbikes') }">
              <i class="nav-icon">🏍️</i>
              <span class="nav-text">Quản lý xe máy</span>
            </router-link>

            <router-link to="/admin/employees" class="nav-item" :class="{ active: isActive('/admin/employees') }">
              <i class="nav-icon">👥</i>
              <span class="nav-text">Quản lý nhân viên</span>
            </router-link>

            <router-link to="/admin/customers" class="nav-item" :class="{ active: isActive('/admin/customers') }">
              <i class="nav-icon">👤</i>
              <span class="nav-text">Quản lý khách hàng</span>
            </router-link>
          </div>

          <div class="nav-section">
            <div class="nav-section-title">BÁO CÁO</div>
            
            <router-link to="/admin/reports" class="nav-item" :class="{ active: isActive('/admin/reports') }">
              <i class="nav-icon">📈</i>
              <span class="nav-text">Báo cáo thống kê</span>
            </router-link>

            <router-link to="/admin/rentals" class="nav-item" :class="{ active: isActive('/admin/rentals') }">
              <i class="nav-icon">📋</i>
              <span class="nav-text">Lịch sử thuê xe</span>
            </router-link>
          </div>
        </nav>
      </aside>

      <!-- Main Content -->
      <main class="admin-content">
        <!-- Breadcrumb -->
        <div class="breadcrumb-container">
          <div class="breadcrumb">
            <span class="breadcrumb-item">🏠 Admin</span>
            <span class="breadcrumb-separator">›</span>
            <span class="breadcrumb-item current">{{ currentPageName }}</span>
          </div>
        </div>

        <!-- Content -->
        <div class="content-area">
          <slot />
        </div>
      </main>
    </div>
  </div>
</template>



<style scoped>
* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

.admin-layout {
  min-height: 100vh;
  background: #f8fafc;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  margin: 0;
  padding: 0;
  width: 100%;
}

/* Header */
.admin-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 0 2rem;
  height: 70px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  position: sticky;
  top: 0;
  z-index: 1000;
  margin: 0;
  width: 100%;
}

.logo {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.logo-icon {
  font-size: 2rem;
}

.logo-text {
  font-size: 1.5rem;
  font-weight: 700;
  letter-spacing: -0.025em;
}

.logo-badge {
  background: rgba(255, 255, 255, 0.2);
  padding: 0.25rem 0.75rem;
  border-radius: 12px;
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.user-menu {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.user-avatar {
  width: 40px;
  height: 40px;
  background: rgba(255, 255, 255, 0.2);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.avatar-icon {
  font-size: 1.25rem;
}

.user-info {
  display: flex;
  flex-direction: column;
  gap: 0.125rem;
}

.user-name {
  font-weight: 600;
  font-size: 0.875rem;
}

.user-role {
  font-size: 0.75rem;
  opacity: 0.8;
}

.logout-btn {
  background: rgba(231, 76, 60, 0.9);
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  cursor: pointer;
  font-size: 0.875rem;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: all 0.2s ease;
}

.logout-btn:hover {
  background: rgba(192, 57, 43, 0.9);
  transform: translateY(-1px);
}

/* Body */
.admin-body {
  display: flex;
  min-height: calc(100vh - 70px);
  margin: 0;
  padding: 0;
  width: 100%;
}

/* Sidebar */
.admin-sidebar {
  width: 280px;
  background: white;
  border-right: 1px solid #e2e8f0;
  box-shadow: 4px 0 12px rgba(0, 0, 0, 0.05);
  margin: 0;
  padding: 0;
}

.sidebar-nav {
  padding: 2rem 0;
}

.nav-section {
  margin-bottom: 2rem;
}

.nav-section-title {
  padding: 0 1.5rem 0.75rem 1.5rem;
  font-size: 0.75rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  color: #64748b;
  border-bottom: 1px solid #f1f5f9;
  margin-bottom: 0.75rem;
}

.nav-item {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.875rem 1.5rem;
  color: #475569;
  text-decoration: none;
  font-weight: 500;
  transition: all 0.2s ease;
  border-left: 3px solid transparent;
  margin-bottom: 0.25rem;
}

.nav-item:hover {
  background: #f8fafc;
  color: #334155;
  border-left-color: #cbd5e1;
}

.nav-item.active {
  background: linear-gradient(135deg, #667eea10, #764ba210);
  color: #667eea;
  border-left-color: #667eea;
  font-weight: 600;
}

.nav-icon {
  font-size: 1.125rem;
  width: 20px;
  text-align: center;
}

/* Content */
.admin-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  margin: 0;
  padding: 0;
}

.breadcrumb-container {
  background: white;
  border-bottom: 1px solid #e2e8f0;
  padding: 1rem 2rem;
  margin: 0;
}

.breadcrumb {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.875rem;
}

.breadcrumb-item {
  color: #64748b;
}

.breadcrumb-item.current {
  color: #334155;
  font-weight: 600;
}

.breadcrumb-separator {
  color: #cbd5e1;
  font-weight: 300;
}

.content-area {
  flex: 1;
  padding: 2rem;
  margin: 0;
}

/* Responsive */
@media (max-width: 1024px) {
  .admin-sidebar {
    width: 240px;
  }
}

@media (max-width: 768px) {
  .admin-header {
    padding: 0 1rem;
  }
  
  .user-info {
    display: none;
  }
  
  .admin-sidebar {
    width: 200px;
  }
  
  .content-area {
    padding: 1rem;
  }
}
</style>