using Mirror;
using UnityEngine;

namespace Content.Gameplay.Code.Movement.Contracts
{
    public abstract class MovementControllerBase : NetworkBehaviour
    {
        private Joystick _inputJoystick;
        protected Joystick InputJoystick
        {
            get
            {
                if (_inputJoystick == null)
                {
                    _inputJoystick = FindObjectOfType<Joystick>();
                }

                return _inputJoystick;
            }
        }

        public float MovementSpeed { get; protected set; }
        protected Vector2 MovementDirection { get; set; }
    }
}