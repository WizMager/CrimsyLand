using Game;
using UnityEngine;

namespace ComponentScripts
{
    public class BonusComponent : MonoBehaviour
    {
        [SerializeField] private BonusType bonusType;

        public BonusType GetBonusType => bonusType;
    }
}