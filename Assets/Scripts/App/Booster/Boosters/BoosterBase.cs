using App.Booster.Config;
using App.Booster.Service;
using UnityEngine;
using Zenject;

namespace App.Booster.Boosters
{
 
    public abstract class BoosterBase 
    {
        [Inject] private BoosterService _boosterService;
        private readonly BoosterConfigBase _config;
        private float _elapsedTime;
        private bool _isTimeExpired;

        public float Duration => _config.Duration;
        public BoosterId BoosterId => _config.BoosterId;
        public float ElapsedTime => _elapsedTime;

        public BoosterBase(BoosterConfigBase config)
        {
            _config = config;
        }
        public abstract void Start();

        public virtual void Tick()
        {
            if (_isTimeExpired) return;
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime < _config.Duration) return;
            _isTimeExpired = true;
            _boosterService.Stop(_config.BoosterId);
        }

        public abstract void Term();
        

    }
}