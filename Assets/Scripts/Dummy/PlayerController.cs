using UnityEngine;

namespace GameJamPlus {
    public class PlayerController : MonoBehaviour {

        public InputSystem_Actions controls;

        public Vector2 lastMovementInput;

        void Awake() {
            controls = new InputSystem_Actions();
            controls.Enable();
        }

        void Update() {
            Vector2 movementInput = controls.Player.Move.ReadValue<Vector2>();
            if (movementInput != Vector2.zero) {
                lastMovementInput = movementInput;
            }

            // Move the player
            transform.Translate(movementInput * 5f * Time.deltaTime, Space.World);

            // For demonstration, rotate the player to face movement direction
            float angle = Mathf.Atan2(lastMovementInput.y, lastMovementInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }
}