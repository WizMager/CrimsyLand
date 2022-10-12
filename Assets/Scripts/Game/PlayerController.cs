using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform playerBody;
        [SerializeField] private Transform shootPosition;
        [SerializeField] private GameObject bullet;
        [SerializeField] private float moveSpeed;
        private InputActions _inputActions;
        private Vector3 _mouseWorldPosition;

        #region MonoBeh

        private void Awake()
        {
            _inputActions = new InputActions();
        }

        private void OnEnable()
        {
            _inputActions.MouseAndKeyboard.Enable();
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine) return;
            var fixedDeltaTime = Time.fixedDeltaTime;
            Move(fixedDeltaTime);
            Aim();
            Shoot();
        }

        private void OnDisable()
        {
            _inputActions.MouseAndKeyboard.Disable();
        }

        #endregion

        private void Move(float deltaTime)
        {
            var moveAction = _inputActions.MouseAndKeyboard.Move;
            if (moveAction.phase != InputActionPhase.Started) return;
            var moveDirection = moveAction.ReadValue<Vector2>() * moveSpeed * deltaTime;
            var currentPosition = playerBody.position;
            playerBody.position = new Vector3(moveDirection.x + currentPosition.x, moveDirection.y + currentPosition.y);
        }

        private void Aim()
        {
            var aimAction = _inputActions.MouseAndKeyboard.Aim;
            if (aimAction.phase != InputActionPhase.Started) return;
            var mousePosition = aimAction.ReadValue<Vector2>();
            _mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            var lookMouseDirection = (playerBody.position - _mouseWorldPosition).normalized;
            var angleAxisZ = Mathf.Atan2(lookMouseDirection.y, lookMouseDirection.x) * Mathf.Rad2Deg + 90;
            playerBody.rotation = Quaternion.Euler(0, 0, angleAxisZ);
        }

        private void Shoot()
        {
            var shootAction = _inputActions.MouseAndKeyboard.Shoot;
            if (shootAction.phase != InputActionPhase.Performed) return;
            Instantiate(bullet, shootPosition.position, shootPosition.rotation);
        }
    }
}