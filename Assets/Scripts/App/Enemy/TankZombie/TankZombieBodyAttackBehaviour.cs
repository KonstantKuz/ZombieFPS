using App.Enemy.Dismemberment.Component.Body;
using App.Enemy.Dismemberment.Component.Body.Behaviour;
using App.Enemy.Dismemberment.Model;
using App.Unit.Component.Animation;
using App.Weapon.Projectile.Projectiles;
using App.Weapon.Weapons;
using Feofun.Extension;
using Feofun.Util.SerializableDictionary;
using UnityEngine;

namespace App.Enemy.TankZombie
{
    public class TankZombieBodyAttackBehaviour: BodyBehaviour
    {
        public enum TankZombiAttackMode
        {
            TwoHand,
            LeftHand,
            RightHand,
        }
        
        [SerializeField] 
        private SerializableDictionary<TankZombiAttackMode, AnimationOverridingInfo> _animations;

        [SerializeField] 
        private Projectile _halfProjectile;
        [SerializeField] 
        private Projectile _mainProjectile;
        
        private AnimationOverrideController _animationOverrideController;
        private RangedWeapon _weapon;

        private void Awake()
        {
            _animationOverrideController = gameObject.RequireComponentInChildren<AnimationOverrideController>();
            _weapon = gameObject.RequireComponentInChildren<RangedWeapon>();
        }
        public override void Init(Unit.Unit unit, BodyMembersInfo membersInfo)
        {
            var states = new BodyStateBuilder(membersInfo)
                .NewState(TankZombiAttackMode.TwoHand.ToString())
                    .WhenAvailable(BodyMemberType.LeftHand)
                    .WhenAvailable(BodyMemberType.RightHand)
                    .WhenAvailable(BodyMemberType.Torso)
                    .OnEnterState(OnEnterTwoHandAttackMode)
                    .Register()
                .NewState(TankZombiAttackMode.LeftHand.ToString())
                    .WhenAvailable(BodyMemberType.Torso)
                    .WhenAvailable(BodyMemberType.LeftHand)
                    .OnEnterState(OnEnterLeftHandAttackMode)
                    .Register()
                .NewState(TankZombiAttackMode.RightHand.ToString())
                    .WhenAvailable(BodyMemberType.Torso)
                    .WhenAvailable(BodyMemberType.RightHand)
                    .OnEnterState(OnEnterRightHandAttackMode)
                .Register()
                .BuildStates();
            base.Init(states);
        }
        
        private void OnEnterTwoHandAttackMode()
        {
            _animationOverrideController.Override(_animations[TankZombiAttackMode.TwoHand]);
            ChangeWeaponProjectile(_mainProjectile);
        }
        
        private void OnEnterLeftHandAttackMode()
        {
            _animationOverrideController.Override(_animations[TankZombiAttackMode.LeftHand]);
            ChangeWeaponProjectile(_halfProjectile);
        }
        
        private void OnEnterRightHandAttackMode()
        {
            _animationOverrideController.Override(_animations[TankZombiAttackMode.RightHand]);
            ChangeWeaponProjectile(_halfProjectile);
        }

        private void ChangeWeaponProjectile(Projectile projectile)
        {
            _weapon.Projectile = projectile;
        }
    }
}