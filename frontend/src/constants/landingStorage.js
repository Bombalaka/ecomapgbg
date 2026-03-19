/**
 * Remember that the user has finished the landing intro once.
 * Stored in the browser (localStorage) so "/" can skip straight to /home next time.
 */
export const LANDING_SEEN_KEY = 'ecomapgbg_seen_landing'

export function hasSeenLanding() {
  try {
    return globalThis.localStorage?.getItem(LANDING_SEEN_KEY) === '1'
  } catch {
    return false
  }
}

export function markLandingSeen() {
  try {
    globalThis.localStorage?.setItem(LANDING_SEEN_KEY, '1')
  } catch {
    /* private mode / blocked storage — intro will show again next visit */
  }
}
