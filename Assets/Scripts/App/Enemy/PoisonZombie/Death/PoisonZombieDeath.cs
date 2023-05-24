using System.Linq;
using App.Enemy.Dismemberment.Component.BodyMember;
using App.Enemy.Dismemberment.Component.BodyMember.Vfx;
using App.Enemy.Dismemberment.Model;
using App.Ragdoll;
using App.Unit.Component.Death;
using Feofun.Extension;
using UnityEngine;

namespace App.Enemy.PoisonZombie.Death
{
    [RequireComponent(typeof(PoisonZombieDeathExplosion))]
    public class PoisonZombieDeath : MonoBehaviour, IUnitDeath
    {
        private IRagdoll _ragdoll;
        private PoisonZombieDeathExplosion _deathExplosion;
        private DismembermentVfxInitializer _dismembermentVfxInitializer;
        
        private BodyMemberBehaviour _torso;
        private BodyMemberHealth _torsoHealth;
        
        private void Awake()
        {
            _ragdoll = gameObject.RequireComponent<IRagdoll>();
            _deathExplosion = gameObject.RequireComponent<PoisonZombieDeathExplosion>();
            _torso = GetComponentsInChildren<BodyMemberBehaviour>()
                .First(it => it.BodyMemberType == BodyMemberType.Torso);
            _torsoHealth = _torso.gameObject.RequireComponent<BodyMemberHealth>();

        }

        public void PlayDeath()
        {
            _ragdoll.Enable();
            ExplodeAndDismember();
        }

        private void ExplodeAndDismember()
        {
            var torsoVfxPlayer = _torso.gameObject.RequireComponent<BodyMemberVfxPlayer>();
            torsoVfxPlayer.IsCurrentVfxSkipped = true;
            _torsoHealth.Kill();
            _deathExplosion.Explode();
        }
        

        private void OnDisable() => _ragdoll.Disable();
    }
}
