import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('@/views/Admin/Motorbike/Index.vue'),
    },
    {
      path: '/admin',
      children: [
        {
          path: 'index',
          name: 'AdminMotorbikeList',
          component: () => import('@/views/Admin/Motorbike/Index.vue'),
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
        }
      ],
    },
  ],
})

export default router
