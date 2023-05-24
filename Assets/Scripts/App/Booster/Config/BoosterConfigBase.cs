using App.Booster.Boosters;
using UnityEngine;
using Zenject;

namespace App.Booster.Config
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Booster/BoosterConfigBase", fileName = "BoosterConfigBase")]
    public abstract class BoosterConfigBase : ScriptableObject
    {
        [SerializeField] private BoosterId boosterId;
        [SerializeField] private float _duration;

        public BoosterId BoosterId => boosterId;
        public float Duration => _duration;

        public abstract BoosterBase CreateBooster(DiContainer container);
    }
}