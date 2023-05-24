using App.Items.Service;
using App.Player.Model;
using App.Player.Model.Attack;
using App.Player.Service;
using App.Unit.Extension;
using App.Weapon.Service;
using EasyButtons;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace App.Player.Component.Attack
{
    public class PlayerAttackCreatorForTest : MonoBehaviour
    {
        [Inject] private PlayerService _playerService;
        [Inject] private WeaponService _weaponService;
        [Inject] private DiContainer _container;

        private RandomInventoryService _randomInventoryService;

        private RandomInventoryService RandomInventoryService =>
            _randomInventoryService ??= _container.Instantiate<RandomInventoryService>();

        [Button]
        public void CreateWeapon(string weaponForCreate)
        {
            _weaponService.EquipForTest(weaponForCreate);
        }

        [Button]
        public void RandomizeWeapons()
        {
            RandomInventoryService.RandomizeEquippedWeapons();
        }
        
        [Button]
        public void RandomizeEquipment()
        {
            RandomInventoryService.RandomizeEquippedItems();
        }
        
        [Button]
        public void DebugStats()
        {
            var player = _playerService.RequirePlayer();
            var playerModel = player.RequireModel<PlayerUnitModel>();
            var attackModel = player.RequireAttackModel<ReloadableWeaponModel>();
            this.Logger().Debug($"Set player model with params :\n " +
                                $"Regeneration := {playerModel.HealthModel.Regeneration}\n " +
                                $"MaxHealth := {playerModel.HealthModel.MaxHealth}\n " +
                                $"AttackDistance := {attackModel.AttackDistance}\n " +
                                $"AttackInterval := {attackModel.AttackInterval}\n " +
                                $"FireRate := {attackModel.FireRate}\n " +
                                $"Accuracy := {attackModel.Accuracy}\n " +
                                $"Control := {attackModel.Control}\n " +
                                $"ClipSize := {attackModel.ClipSize}\n " +
                                $"DamageRadius := {attackModel.DamageRadius}\n " +
                                $"ProjectileSpeed := {attackModel.ProjectileSpeed}\n " +
                                $"ReloadTime := {attackModel.ReloadTime}\n " +
                                $"ShotCount := {attackModel.ShotCount}\n " +
                                $"FullDamage := {attackModel.FullDamage}"
            );
        }
    }
}