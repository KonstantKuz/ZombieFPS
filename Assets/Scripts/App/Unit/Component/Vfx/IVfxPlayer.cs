using App.Unit.Component.Attack;
using Feofun.World.Model;

namespace App.Unit.Component.Vfx
{
    public interface IVfxPlayer
    { 
        void Play(HitInfo hitInfo, WorldObject vfxPrefab, bool isCurrentVfxSkipped = false,
            bool isAttachedToTarget = false);
    }
}