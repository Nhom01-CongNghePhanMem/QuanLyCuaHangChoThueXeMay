<script>
import { login } from '@/api/services/authService'
import { jwtDecode } from 'jwt-decode'
import { useRouter } from 'vue-router'

export default {
  name: 'Login',
  data() {
    return {
      UserName: '',
      Password: '',
      showPassword: false,
      loading: false,
      error: ''
    }
  },
  mounted() {
    // Kiểm tra token khi vào trang đăng nhập
    const token = localStorage.getItem('token')
    if (token) {
      try {
        const user = jwtDecode(token)
        const now = Date.now() / 1000 // giây
        if (user.exp && user.exp > now) {
          // Token còn hạn, chuyển hướng theo role
          if (user.role === 'Manager') {
            this.$router.push({ name: 'AdminMotorbikeList' })
          }
          if (user.role === 'Receptionist') {
            
          } else {
            this.$router.push({ name: 'Home' })
          }
        }
      } catch {
        // Token lỗi, cho phép đăng nhập lại
      }
    }
  },
  methods: {
    async handleLogin() {       
      this.error = ''
      this.loading = true
      const result = await login({
        UserName: this.UserName,
        Password: this.Password
      })
      this.loading = false
      if (result.success) {
        localStorage.setItem('token', result.data)
        // Giải mã token để lấy thông tin user
        const userInfo = jwtDecode(result.data)
        // Ví dụ: chuyển trang dựa vào role
        if (userInfo.role === 'Manager') {
          this.$router.push({ name: 'AdminMotorbikeList' })
        } else {
          this.$router.push({ name: 'ReceptionistIndex' })
        }
      } else {
        this.error = result.message || 'Đăng nhập thất bại'
      }
    }
  }
}
</script>
<template>
  <div class="login-container">
    <form class="login-form" @submit.prevent="handleLogin">
      <h2>Đăng nhập</h2>
      <input
        v-model="UserName"
        type="text"
        placeholder="Tên đăng nhập"
        required
      />
      <input
        v-model="Password"
        :type="showPassword ? 'text' : 'password'"
        placeholder="Mật khẩu"
        required
      />
      <label class="show-password">
        <input type="checkbox" v-model="showPassword" />
        Hiện mật khẩu
      </label>
      <div v-if="error" class="error">{{ error }}</div>
      <button type="submit" :disabled="loading">
        {{ loading ? 'Đang đăng nhập...' : 'Đăng nhập' }}
      </button>
    </form>
  </div>
</template>

<style scoped>
.login-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #2193b0 0%, #6dd5ed 100%);
}
.login-form {
  background: #fff;
  padding: 2.5rem 2rem;
  border-radius: 18px;
  box-shadow: 0 8px 32px 0 rgba(31, 38, 135, 0.2);
  display: flex;
  flex-direction: column;
  min-width: 320px;
}
.login-form h2 {
  text-align: center;
  margin-bottom: 1.5rem;
  color: #2193b0;
}
.login-form input[type="text"],
.login-form input[type="password"] {
  margin-bottom: 1rem;
  padding: 0.7rem;
  border: 1px solid #bdbdbd;
  border-radius: 8px;
  font-size: 1rem;
}
.login-form button {
  background: #2193b0;
  color: #fff;
  border: none;
  padding: 0.8rem;
  border-radius: 8px;
  font-size: 1.1rem;
  cursor: pointer;
  transition: background 0.2s;
}
.login-form button:disabled {
  background: #bdbdbd;
  cursor: not-allowed;
}
.login-form .error {
  color: #e53935;
  margin-bottom: 1rem;
  text-align: center;
}
.show-password {
  font-size: 0.95rem;
  margin-bottom: 1rem;
  color: #555;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}
</style>