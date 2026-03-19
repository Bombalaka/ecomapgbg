<template>
  <div class="landing">
    <!-- Background video (optional URL) + gradient fallback -->
    <div class="landing-media" aria-hidden="true">
      <video
        v-if="videoSrc"
        class="landing-video"
        autoplay
        muted
        loop
        playsinline
        tabindex="-1"
      >
        <source :src="videoSrc" type="video/mp4" />
        <track
          kind="captions"
          srclang="sv"
          label="Inget tal"
          src="/landing-empty.vtt"
          default
        />
      </video>
      <div class="landing-fallback" />
      <div class="landing-scrim" />
    </div>

    <div class="landing-inner">
      <!-- Phase 1: one short logo jump (not “bouncy” elastic) -->
      <div class="landing-stage">
        <img
          src="/logo-ecomapgbg.svg"
          alt="EcoMapGbg"
          width="96"
          height="96"
          class="landing-logo"
          :class="{ 'landing-logo--done': introDone }"
          @animationend="onJumpEnd"
        />
      </div>

      <!-- Phase 2: copy + CTA after intro -->
      <transition name="landing-reveal">
        <div v-show="showContent" class="landing-content">
          <p class="landing-badge">Återbrukskartan · Göteborg</p>
          <h1 class="landing-title">EcoMapGbg</h1>
          <p class="landing-lead">
            Hitta och dela ställen för återbruk, reparation och cirkulär konsumtion.
          </p>
          <div class="landing-actions">
            <v-btn
              color="white"
              size="x-large"
              rounded="xl"
              class="landing-cta text-primary"
              elevation="4"
              @click="goApp"
            >
              Utforska appen
            </v-btn>
            <v-btn variant="text" class="text-white landing-skip" @click="goApp">
              Hoppa över intro
            </v-btn>
          </div>
        </div>
      </transition>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'
import { markLandingSeen } from '../constants/landingStorage'

const router = useRouter()

/** Set in frontend/.env — your own loop (MP4/WebM). If empty, gradient “poster” only. */
const videoSrc = computed(() => {
  const url = import.meta.env.VITE_LANDING_VIDEO_URL
  return typeof url === 'string' && url.trim() !== '' ? url.trim() : ''
})

const introDone = ref(false)
const showContent = ref(false)
/** Match users who turn off motion in OS settings */
const reduceMotion = ref(false)

let introFallbackTimer

onMounted(() => {
  reduceMotion.value = globalThis.matchMedia('(prefers-reduced-motion: reduce)').matches
  if (reduceMotion.value) {
    introDone.value = true
    showContent.value = true
    return
  }
  /* If animationend never fires (rare), still show CTA */
  introFallbackTimer = globalThis.setTimeout(() => {
    if (!showContent.value) {
      introDone.value = true
      showContent.value = true
    }
  }, 900)
})

onUnmounted(() => {
  if (introFallbackTimer) globalThis.clearTimeout(introFallbackTimer)
})

function onJumpEnd() {
  if (reduceMotion.value) return
  introDone.value = true
  showContent.value = true
}

function goApp() {
  markLandingSeen()
  router.push({ name: 'Home' })
}
</script>

<style scoped>
.landing {
  position: relative;
  min-height: 100dvh;
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
  color: #fff;
}

.landing-media {
  position: absolute;
  inset: 0;
  z-index: 0;
}

.landing-video {
  position: absolute;
  inset: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
  pointer-events: none;
}

/* Shown under / instead of video when no URL — still looks intentional */
.landing-fallback {
  position: absolute;
  inset: 0;
  background: linear-gradient(145deg, #0d2818 0%, #1b5e20 35%, #2e7d32 70%, #004d40 100%);
}

.landing-scrim {
  position: absolute;
  inset: 0;
  background: linear-gradient(
    to bottom,
    rgba(0, 0, 0, 0.25) 0%,
    rgba(0, 0, 0, 0.45) 50%,
    rgba(0, 0, 0, 0.65) 100%
  );
}

.landing-inner {
  position: relative;
  z-index: 1;
  text-align: center;
  padding: 2rem 1.25rem 3rem;
  max-width: 36rem;
}

.landing-stage {
  min-height: 6.5rem;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 0.5rem;
}

.landing-logo {
  filter: drop-shadow(0 12px 32px rgba(0, 0, 0, 0.35));
}

/* One clean “jump in” — ease-out, no overshoot curve (feels less “template AI”) */
.landing-logo:not(.landing-logo--done) {
  animation: landing-jump-in 0.65s ease-out forwards;
}

.landing-logo.landing-logo--done {
  animation: none;
}

.landing-content {
  margin-top: 0.25rem;
}

.landing-badge {
  font-size: 0.8rem;
  text-transform: uppercase;
  letter-spacing: 0.12em;
  opacity: 0.9;
  margin: 0 0 0.5rem;
}

.landing-title {
  font-size: clamp(2rem, 6vw, 2.75rem);
  font-weight: 700;
  margin: 0 0 0.75rem;
  letter-spacing: -0.03em;
  text-shadow: 0 2px 20px rgba(0, 0, 0, 0.35);
}

.landing-lead {
  font-size: 1.05rem;
  line-height: 1.55;
  opacity: 0.92;
  margin: 0 0 1.75rem;
}

.landing-actions {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
}

.landing-cta {
  text-transform: none;
  font-weight: 700;
  padding: 0 2rem !important;
}

.landing-skip {
  text-transform: none;
  opacity: 0.85;
}

.landing-reveal-enter-active {
  transition: opacity 0.45s ease, transform 0.45s ease;
}

.landing-reveal-enter-from {
  opacity: 0;
  transform: translateY(10px);
}

@keyframes landing-jump-in {
  0% {
    opacity: 0;
    transform: translateY(20px) scale(0.94);
  }
  55% {
    opacity: 1;
    transform: translateY(-10px) scale(1);
  }
  75% {
    transform: translateY(4px) scale(1);
  }
  100% {
    opacity: 1;
    transform: translateY(0) scale(1);
  }
}

@media (prefers-reduced-motion: reduce) {
  .landing-logo:not(.landing-logo--done),
  .landing-logo {
    animation: none !important;
  }

  .landing-reveal-enter-active {
    transition: none;
  }

  .landing-reveal-enter-from {
    opacity: 1;
    transform: none;
  }

  .landing-video {
    display: none;
  }
}
</style>
