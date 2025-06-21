import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    /*{
      path: '/',
      name: 'home',
      component: HomeView,
    },
    */
    {
      path: '/admin/Index',
      name: 'Index',  
      component: () => import('../views/Admin/Index.vue'),
    },
  ],
})

export default router
