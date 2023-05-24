using System;
using System.Collections.Generic;
using System.Linq;
using App.Booster.Boosters;
using App.Booster.Boosters.Weapon;
using App.Booster.Config;
using App.Booster.Messages;
using App.Tutorial.Service;
using Feofun.Core.Update;
using Feofun.Extension;
using UniRx;
using Zenject;

namespace App.Booster.Service
{
    public class BoosterService
    {
        private readonly Dictionary<BoosterId, ReactiveProperty<BoosterStateChangedData>> _stateChangedProperty =
            EnumExt.Values<BoosterId>().ToDictionary(it => it, it => new ReactiveProperty<BoosterStateChangedData>());
        private readonly Dictionary<BoosterId, BoosterBase> _currentBoosters = new();
        
        [Inject] private UpdateManager _updateManager;
        [Inject] private DiContainer _container;

        public List<BoosterBase> ActiveBoosters => _currentBoosters.Values.ToList();
        
        public IObservable<BoosterStateChangedData> AnyStateChangedObservable => _stateChangedProperty.Values.Merge()
            .Where(it => it != null);

        public IObservable<BoosterStateChangedData> GetBoosterStateAsObservable(BoosterId boosterId) => _stateChangedProperty[boosterId];

        public BoosterService(TutorialService tutorialService)
        {
            tutorialService.IsAnyScenarioActiveAsObservable.Where(it => it).Subscribe(it => StopAllBoosters());
        }
        
        public bool IsBoosterActivated(BoosterId boosterId) => _currentBoosters.ContainsKey(boosterId);

        public void Start(BoosterId boosterId)
        {
            if (IsBoosterActivated(boosterId)) {
                throw new Exception($"Booster is already active, id:= {boosterId}");
            }

            var config = _container.ResolveId<BoosterConfigBase>(boosterId);
            var booster = config.CreateBooster(_container);
            _currentBoosters[boosterId] = booster;
            _updateManager.StartUpdate(booster.Tick);
            booster.Start();
            _stateChangedProperty[boosterId].SetValueAndForceNotify(new BoosterStateChangedData(booster, BoosterState.Started));
        }

        public void Stop(BoosterId boosterId)
        {
            if (!IsBoosterActivated(boosterId)) {
                throw new Exception($"Booster is not active, id:= {boosterId}");
            }
            var booster = _currentBoosters[boosterId];
            _currentBoosters.Remove(boosterId);
            _updateManager.StopUpdate(booster.Tick);
            booster.Term();
            _stateChangedProperty[boosterId].SetValueAndForceNotify(new BoosterStateChangedData(booster, BoosterState.Stopped));
        }

        public void StopAllBoosters()
        {
            _currentBoosters.Values.ToList().ForEach(it => Stop(it.BoosterId));
            _currentBoosters.Clear();
        }
        
        public void EquipBoosterWeaponIfExists()
        {
            var activeWeaponBooster = ActiveBoosters.OfType<WeaponBooster>().FirstOrDefault();
            activeWeaponBooster?.Equip();
        }
    }
}