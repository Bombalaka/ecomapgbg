import { createRouter, createWebHistory } from 'vue-router'
import { hasSeenLanding } from '../constants/landingStorage'

const routes = [
  {
    path: '/',
    name: 'Landing',
    component: () => import('../views/Landing.vue'),
    meta: { hideChrome: true },
  },
  {
    path: '/home',
    name: 'Home',
    component: () => import('../views/Home.vue'),
  },
  { path: '/places', name: 'Places', component: () => import('../views/Places.vue') },
  { path: '/places/add', name: 'AddPlace', component: () => import('../views/AddPlace.vue') },
  { path: '/about', name: 'About', component: () => import('../views/About.vue') },
  { path: '/help', name: 'Help', component: () => import('../views/Help.vue') },
  { path: '/events', name: 'Events', component: () => import('../views/Events.vue') },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

/** After first visit to landing + "enter app", skip "/" → go to /home. Use ?replay=1 to see landing again. */
router.beforeEach((to) => {
  const skipLanding =
    to.name === 'Landing' &&
    to.query.replay !== '1' &&
    hasSeenLanding()
  if (skipLanding) return { name: 'Home', replace: true }
})

export default router
