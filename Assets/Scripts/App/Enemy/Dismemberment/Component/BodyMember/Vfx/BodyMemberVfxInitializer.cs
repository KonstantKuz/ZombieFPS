using App.Enemy.Dismemberment.Config;
using App.Unit.Component.Vfx;
using JetBrains.Annotations;
using Zenject;

namespace App.Enemy.Dismemberment.Component.BodyMember.Vfx
{
    public class BodyMemberVfxInitializer
    {
        private readonly BodyMemberBehaviour _bodyMember;
        private readonly BodyMemberVfxPlayer _vfxPlayer;
        private readonly IVfxPlayer _selfPlayer;

        public BodyMemberVfxInitializer(DiContainer container, BodyMemberBehaviour bodyMember)
        {
            _bodyMember = bodyMember;
            _vfxPlayer = container.InstantiateComponent<BodyMemberVfxPlayer>(bodyMember.gameObject);
            _selfPlayer =  container.InstantiateComponent<PlaySingleVfxOnHit>(bodyMember.gameObject);
        }
            
        public void InitVfxPlayer([CanBeNull] IVfxPlayer unitVfxPlayer, DismembermentVfxConfig config)
        {
            var vfx = config.GetDestroyVfx(_bodyMember.BodyMemberType);
            _vfxPlayer.Init(_bodyMember, vfx, _selfPlayer, unitVfxPlayer);
        }

        public void Dispose()
        {
            _vfxPlayer.Dispose();
        }
    }
}