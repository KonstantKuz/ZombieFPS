using App.Unit.Component.Attack;
using Feofun.Util.Timer;
using UniRx;

namespace App.Player.Component.Attack.Reloader
{
    public interface IWeaponReloader : IAttackComponent
    {
        IReactiveProperty<ITimer> ReloadingTimer { get; }
        void StartReload();
        void StopReload();
    }
}