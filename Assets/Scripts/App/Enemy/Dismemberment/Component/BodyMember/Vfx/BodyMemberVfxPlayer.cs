using System;
using App.Unit.Component.Attack;
using App.Unit.Component.Vfx;
using JetBrains.Annotations;
using Feofun.World.Model;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.Enemy.Dismemberment.Component.BodyMember.Vfx
{
    public class BodyMemberVfxPlayer : MonoBehaviour
    {
        private BodyMemberBehaviour _owner;
        private WorldObject _vfxPrefab;
        [CanBeNull]
        private IVfxPlayer _unitVfxPlayer;  
        
        private IVfxPlayer _selfVfxPlayer;

        public bool IsCurrentVfxSkipped { get; set; }

        private bool IsParentPlayerExists => _unitVfxPlayer != null;

        public void Init(BodyMemberBehaviour owner, 
            WorldObject vfxPrefab, IVfxPlayer selfVfxPlayer, [CanBeNull] IVfxPlayer unitVfxPlayer)
        {
            _owner = owner;
            _vfxPrefab = vfxPrefab;
            _unitVfxPlayer = unitVfxPlayer;
            _selfVfxPlayer = selfVfxPlayer;
            _owner.OnMemberDestroyed += PlayDestroyVfx;
        }

        public void PlayDestroyVfx()
        {
            GetVfxPlayer().Play(new HitInfo(gameObject, transform.position, Vector3.up), _vfxPrefab, IsCurrentVfxSkipped);
        }

        private IVfxPlayer GetVfxPlayer()
        {
            if (IsParentPlayerExists) {
                return _unitVfxPlayer;
            }
            Assert.IsTrue(_selfVfxPlayer != null);
            return _selfVfxPlayer;
        }

        public void Dispose()
        {
            _owner.OnMemberDestroyed -= PlayDestroyVfx;
        }
    }
}