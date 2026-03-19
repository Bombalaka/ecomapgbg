import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  { path: '/', name: 'Home', component: () => import('../views/Home.vue') },
  { path: '/places', name: 'Places', component: () => import('../views/Places.vue') },
  { path: '/places/add', name: 'AddPlace', component: () => import('../views/AddPlace.vue') },
  { path: '/about', name: 'About', component: () => import('../views/About.vue') },
  { path: '/help', name: 'Help', component: () => import('../views/Help.vue') },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
