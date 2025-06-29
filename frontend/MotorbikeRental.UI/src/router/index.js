import { createRouter, createWebHistory } from 'vue-router'
import { jwtDecode } from 'jwt-decode'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'Login',
      component: () => import('@/views/Auth/Login.vue'),
    },
    {
      path: '/admin',
      children: [
        {
          path: 'index',
          name: 'AdminMotorbikeList',
          component: () => import('@/views/Admin/Motorbike/Index.vue'),
          meta: { requiresAuth: true, roles: ['Manager'] },
        },
        {
          path: 'motorbike/create',
          name: 'AdminMotorbikeCreate',
          component: () => import('@/views/Admin/Motorbike/CreateMotorbike.vue'),
        },
        {
          path: 'motorbike/detail/:id',
          name: 'AdminMotorbikeDetail',
          component: () => import('@/views/Admin/Motorbike/DetailMotorbike.vue'),
        },
        {
          path: 'motorbike/edit/:id',
          name: 'AdminMotorbikeEdit',
          component: () => import('@/views/Admin/Motorbike/EditMotorbike.vue'),
        },
      ],
    },
    {
      path: '/forbidden',
      name: 'Forbidden',
      component: () => import('@/views/Forbidden.vue'),
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'NotFound',
      component: () => import('@/views/NotFound.vue'),
    },
  ],
})

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  if (to.name !== 'Login' && !token) {
    next({ name: 'Login' })
    return
  }
  const requiredRoles = to.matched.find((r) => r.meta && r.meta.roles)?.meta?.roles
  if (requiredRoles && token) {
    try {
      const user = jwtDecode(token) 
      if (requiredRoles.includes(user.role)) {
        next()
      } else {
        next({ name: 'Forbidden' })
      }
    } catch {
      localStorage.removeItem('token')
      next({ name: 'Login' })
    }
  } else {
    next()
  }
})

export default router
