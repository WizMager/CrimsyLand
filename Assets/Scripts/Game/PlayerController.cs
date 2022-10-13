using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private Transform playerBody;
        [SerializeField] private Transform shootPosition;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float shootCooldown;
        private Camera _mainCamera;
        private InputActions _inputActions;
        private Vector3 _mouseWorldPosition;
        private bool _shootAvailable = true;

        #region MonoBeh

        private void Awake()
        {
            _inputActions = new InputActions();
            _mainCamera = Camera.main;
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
            _mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
            var lookMouseDirection = (playerBody.position - _mouseWorldPosition).normalized;
            var angleAxisZ = Mathf.Atan2(lookMouseDirection.y, lookMouseDirection.x) * Mathf.Rad2Deg + 90;
            playerBody.rotation = Quaternion.Euler(0, 0, angleAxisZ);
        }

        private void Shoot()
        {
            if (!_shootAvailable) return;
            var shootAction = _inputActions.MouseAndKeyboard.Shoot;
            if (shootAction.phase != InputActionPhase.Performed) return;
            PhotonNetwork.Instantiate(bulletPrefab.name, shootPosition.position, shootPosition.rotation);
            _shootAvailable = false;
            StartCoroutine(ShootCooldown());
        }

        private IEnumerator ShootCooldown()
        {
            for (float i = 0; i < shootCooldown; i += Time.deltaTime)
            {
                yield return null;
            }
            _shootAvailable = true;
        }
    }
}