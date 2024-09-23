using Content.Gameplay.Code.Level;
using Content.Gameplay.Code.Progress;
using Mirror;
using UnityEngine;

namespace Content.Gameplay.Code.Networking
{
    public class ClientColliderController : NetworkBehaviour
    {
        [SerializeField] private float collisionScore = 20f;

        private ProgressController _progressController;
        private ProgressController ProgressController
        {
            get
            {
                if (_progressController == null)
                {
                    _progressController = FindObjectOfType<ProgressController>();
                }

                return _progressController;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isLocalPlayer)
                return;

            if (other.GetComponentInParent<ServerColliderController>() != null)
            {
                HandleCollision();
            }
        }

        [Command]
        private void HandleCollision()
        {
            Vector3 newPosition = NetworkManager.startPositions[Random.Range(0, NetworkManager.startPositions.Count)].position;

            ProgressController.Tick(-collisionScore);
            UpdatePositionRpc(newPosition);
        }

        [ClientRpc]
        private void UpdatePositionRpc(Vector3 newPosition)
        {
            transform.position = newPosition;
        }
    }
}