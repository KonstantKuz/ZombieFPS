using System;
using Feofun.Extension;
using UniRx;

namespace App.Unit.Component.Health.Extension
{
    public static class HealthExtension
    {
        public static IObservable<float> GetHealthPercent(this IHealthOwner owner)
        {
            return owner.Current.Select(it => {
                if (owner.Max.IsZero()) {
                    return 0;
                }
                return 1.0f * it / owner.Max;
            });
        }
    }
}