using App.Config;
using App.Unit.Component.Health;
using Feofun.Extension;
using Zenject;

namespace App.Weapon.Projectile
{
    public class ProjectileHealth : Health
    {
        private Projectiles.Projectile _projectile;

        [Inject] private ConstantsConfig _constants;

        private Projectiles.Projectile Projectile => _projectile ??= gameObject.RequireComponent<Projectiles.Projectile>();

        private void OnEnable()
        {
            // todo: add projectile health config by id if there is more than one
            Init(_constants.TankProjectileHealth);
            OnZeroHealth += Projectile.Destroy;
        }

        private void OnDisable()
        {
            OnZeroHealth -= Projectile.Destroy;
        }
    }
}
