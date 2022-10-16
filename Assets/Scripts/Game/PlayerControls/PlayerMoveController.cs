using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.PlayerControls
{
    public class PlayerMoveController : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float moveSpeed;
        [SerializeField] private PlayerInput input;
        [SerializeField] private string useActionMap;

        private void OnEnable()
        {
            input.actions[useActionMap].Enable();
        }
        
        private void FixedUpdate()
        {
            if (!photonView.IsMine) return;
            Move(Time.fixedDeltaTime);
        }
        
        private void OnDisable()
        {
            input.actions[useActionMap].Disable();
        }
        
        private void Move(float deltaTime)
        {
            var moveAction = input.actions[useActionMap];
            if (moveAction.phase != InputActionPhase.Started) return;
            var moveDirection = moveAction.ReadValue<Vector2>() * moveSpeed * deltaTime;
            var currentPosition = playerTransform.position;
            transform.position = new Vector3(moveDirection.x + currentPosition.x, moveDirection.y + currentPosition.y);
        }
    }
}