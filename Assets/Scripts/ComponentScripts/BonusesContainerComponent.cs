using Game;
using UnityEngine;

namespace ComponentScripts
{
    public class BonusesContainerComponent : MonoBehaviour
    {
        [SerializeField] private BonusComponent[] bonuses;

        public BonusComponent[] GetBonusesScripts => bonuses;
    }
}