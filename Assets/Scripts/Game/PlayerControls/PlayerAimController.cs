using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.PlayerControls
{
    public class PlayerAimController : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private Transform playerBody;
        [SerializeField] private PlayerInput input;
        [SerializeField] private string useActionMap;
        private Camera _mainCamera;
        private Vector3 _mouseWorldPosition;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            input.actions[useActionMap].Enable();
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine) return;
            Aim();
        }

        private void OnDisable()
        {
            input.actions[useActionMap].Disable();
        }

        private void Aim()
        {
            var aimAction = input.actions[useActionMap];
            if (aimAction.phase != InputActionPhase.Started) return;
            var mousePosition = aimAction.ReadValue<Vector2>();
            _mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
            var lookMouseDirection = (playerBody.position - _mouseWorldPosition).normalized;
            var angleAxisZ = Mathf.Atan2(lookMouseDirection.y, lookMouseDirection.x) * Mathf.Rad2Deg + 90;
            playerBody.rotation = Quaternion.Euler(0, 0, angleAxisZ);
        }
    }
}