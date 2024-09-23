using Content.Gameplay.Code.Movement.Contracts;
using UnityEngine;

namespace Content.Gameplay.Code.Movement
{
    public class CharacterMovementController : MovementControllerBase
    {
        private const float INPUT_MAGNITUDE_THRESHOLD = 0.01f;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private float movementFactor = 1f;
        [SerializeField, Range(0f, 1f)] private float minInputMagnitude = 0.35f;

        private void Update()
        {
            if (!isServer)
                return;

            Tick(InputJoystick.Direction);
        }

        private void Tick(Vector2 dir)
        {
            MovementDirection = dir;
            MovementSpeed = MovementDirection.magnitude > INPUT_MAGNITUDE_THRESHOLD ?
                Mathf.Clamp(MovementDirection.magnitude, minInputMagnitude, 1f) * movementFactor :
                0f;

            if (MovementSpeed > 0f)
            {
                float angle = Mathf.Atan2(MovementDirection.y, -MovementDirection.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.up);
                rb.rotation = rotation;

                rb.MovePosition(rb.position + transform.forward * (MovementSpeed * Time.fixedDeltaTime));
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                //transform.position += transform.forward * MovementSpeed * Time.deltaTime;
            }
        }
    }
}