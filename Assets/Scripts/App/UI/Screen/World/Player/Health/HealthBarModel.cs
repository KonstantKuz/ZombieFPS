using System;
using App.Unit.Component.Health;
using App.Unit.Component.Health.Extension;
using Feofun.Extension;
using UniRx;

namespace App.UI.Screen.World.Player.Health
{
    public class HealthBarModel
    {
        public readonly IObservable<float> Percent;

        public HealthBarModel(IHealthOwner owner) => Percent = owner.GetHealthPercent();
    }
}