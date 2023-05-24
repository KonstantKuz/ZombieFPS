using App.Unit.Component.Attack;
using App.Weapon.Projectile.Data;
using Feofun.Extension;
using Feofun.Util;
using Feofun.Vfx;
using Feofun.World.Factory.ObjectFactory;
using Feofun.World.Model;
using UnityEngine;
using Zenject;

namespace App.Unit.Component.Vfx
{
    public class PlaySingleVfxOnHit : MonoBehaviour, IVfxPlayer
    {
        [Inject(Id = ObjectFactoryType.Pool)] 
        private IObjectFactory _factory;
        private ParticleSystem _currentVfx;
        
        private bool IsCurrentVfxPlaying => _currentVfx != null 
                                            && _currentVfx.isPlaying
                                            && _currentVfx.IsAlive(true);

        
        public void Play(HitInfo hitInfo, WorldObject vfxPrefab, bool isCurrentVfxSkipped = false,
            bool isAttachedToTarget = false)
        {
            if (IsCurrentVfxPlaying && !isCurrentVfxSkipped) return;
            
            _currentVfx = _factory.Create<ParticleSystem>(vfxPrefab.ObjectId);
            var hitVfxTransform = _currentVfx.transform;
            hitVfxTransform.SetPositionAndRotation(hitInfo.Position, VectorExt.OrientationByNormal(hitInfo.Normal));
            if (isAttachedToTarget)
            {
                hitVfxTransform.gameObject.GetOrAddComponent<PseudoAttach>()
                    .Attach(hitInfo.Target.transform);
            }
        }
    }
}