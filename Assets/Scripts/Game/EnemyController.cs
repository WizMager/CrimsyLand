using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float timeRecheckNearestPlayer;
        private List<Transform> _playersTransform = new List<Transform>();
        private Transform _currentTarget;
        
        public List<Transform> SetPlayersTransform
        {
            set
            {
                _playersTransform.Clear();
                _playersTransform = value;
                _currentTarget = _playersTransform[CheckNearestPlayerIndex()];
            }
        }

        #region Monobeh

        private void Start()
        {
            StartCoroutine(RecheckNearestEnemyCooldown());
        }
        
        private void FixedUpdate()
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _currentTarget.position, moveSpeed * Time.fixedDeltaTime);
        }

        private void OnDestroy()
        {
            StopCoroutine(RecheckNearestEnemyCooldown());
        }

        #endregion

        #region CheckNearestPlayer

        private IEnumerator RecheckNearestEnemyCooldown()
        {
            for (float i = 0; i < timeRecheckNearestPlayer; i += Time.deltaTime)
            {
                yield return null;
            }
            
            RecheckNearestPlayer();
        }
        
        private int CheckNearestPlayerIndex()
        {
            var distance = 100f;
            var index = 0;
            for (int i = 0; i < _playersTransform.Count; i++)
            {
                var currentDistance = Vector3.Distance(transform.position, _playersTransform[i].position);
                if (currentDistance >= distance) continue;
                distance = currentDistance;
                index = i;
            }
            return index;
        }

        private void RecheckNearestPlayer()
        {
            _currentTarget = _playersTransform[CheckNearestPlayerIndex()];
            StartCoroutine(RecheckNearestEnemyCooldown());
        }

        #endregion
    }
}