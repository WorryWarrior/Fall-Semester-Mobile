using Content.Gameplay.Code.Movement.Contracts;
using UnityEngine;

namespace Content.Gameplay.Code.Movement
{
    public class LeafMovementController : MovementControllerBase
    {
        [SerializeField] private float movementFactor = 1f;
        [SerializeField] private Vector2 xBounds;
        [SerializeField] private Vector2 zBounds;
        private void Update()
        {
            if (!isLocalPlayer)
                return;

            Tick();
        }

        private void Tick()
        {
            MovementDirection = InputJoystick.Direction;
            MovementSpeed = MovementDirection.magnitude;

            if (MovementSpeed > 0f)
            {
                float angle = Mathf.Atan2(MovementDirection.y, -MovementDirection.x) * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.AngleAxis(angle - 90, Vector3.up);
                transform.rotation = rot;
            }

            transform.position += transform.forward * (MovementSpeed * movementFactor * Time.deltaTime);
            Vector3 oldPosition = transform.position;

            if (transform.position.x < xBounds.x)
            {
                oldPosition.x = xBounds.x;
            }

            if (transform.position.x > xBounds.y)
            {
                oldPosition.x = xBounds.y;
            }

            if (transform.position.z < zBounds.x)
            {
                oldPosition.z = xBounds.x;
            }

            if (transform.position.z > zBounds.y)
            {
                oldPosition.z = xBounds.y;
            }

            transform.position = oldPosition;
        }
    }
}