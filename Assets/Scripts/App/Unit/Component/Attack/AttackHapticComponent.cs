using System;
using App.Enemy.Model;
using App.Player.Model.Attack;
using App.Unit.Extension;
using App.Vibration;
using Feofun.Components;
using Zenject;

namespace App.Unit.Component.Attack
{
    public class AttackHapticComponent: AttackComponent, IInitializable<AttackComponentInitData>, IDisposable
    {
        private readonly VibrationManager _vibrationManager;
        
        private VibrationType _vibrationType;

        public AttackHapticComponent(VibrationManager vibrationManager)
        {
            _vibrationManager = vibrationManager;
        }

        public void Init(AttackComponentInitData data)
        {
            _vibrationType = data.Unit.RequireAttackModel<ReloadableWeaponModel>().Vibration;
            Mediator.OnFire += Vibrate;
        }

        private void Vibrate()
        {
            _vibrationManager.Vibrate(_vibrationType);
        }

        public void Dispose()
        {
            Mediator.OnFire -= Vibrate;
        }
    }
}