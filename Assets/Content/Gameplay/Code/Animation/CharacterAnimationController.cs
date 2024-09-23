using Content.Gameplay.Code.Movement.Contracts;
using Mirror;
using UnityEngine;

namespace Content.Gameplay.Code.Animation
{
    public class CharacterAnimationController : NetworkBehaviour
    {
        [SerializeField] private MovementControllerBase movementController;
        [SerializeField] private Animator animator;

        private readonly int _animatorMovementToggleName = Animator.StringToHash("IsMoving");

        [Server]
        private void Update()
        {
            animator.SetBool(_animatorMovementToggleName, movementController.MovementSpeed > 0f);
        }
    }
}