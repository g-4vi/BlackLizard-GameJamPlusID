using UnityEngine;

namespace GameJamPlus.SkillModules.Behaviour {
    /// <summary>
    /// Handles the behavior of a projectile skill.
    /// This scrript attached to the projectile prefab. It behaves like normal projectile.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class ProjectileBehaviour : MonoBehaviour {

        Vector2 direction = Vector2.right;
        float speed = 5f;
        [SerializeField] SfxID _impactSFX;

        protected virtual void Update() {
            if (Time.deltaTime <= 0f) return; // Pause check
            transform.Translate(direction * speed * Time.unscaledDeltaTime, Space.World);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.CompareTag("Obstacle")) { // Hit object with tag "Obstacle"
                // TODO: Call something when hit an obstacle
                if (_impactSFX != SfxID.None) AudioManager.Instance.PlaySFX(_impactSFX);
                // e.g., play sound effect, spawn particle effect, etc.
                Destroy(collision.gameObject);
                Destroy(gameObject);
                Debug.Log($"[{name}] Projectile hit an obstacle and is destroyed.");
            }
        }

        // Destroy the projectile when it goes off-screen
        // Notes; scene view counts as visible area
        void OnBecameInvisible() {
            Destroy(gameObject);
            Debug.Log($"[{name}] Projectile went off-screen and is destroyed.");
        }

        #region Public Methods
        public void SetDirection(Vector2 dir) {
            if (dir == Vector2.zero) dir = Vector2.right; // prevent zero direction
            direction = dir;

            ComputeAngleForRotation();
        }

        public void SetSpeed(float spd) {
            if (spd <= 0f) spd = 5f; // prevent zero or negative speed
            speed = spd;
        }
        #endregion

        // Visual purpose, rotate the projectile to face its direction
        // Notes: Visual default direction should be right (1,0) with zero rotation
        void ComputeAngleForRotation() {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }
}