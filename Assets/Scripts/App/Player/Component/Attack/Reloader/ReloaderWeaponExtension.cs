namespace App.Player.Component.Attack.Reloader
{
    public static class ReloaderWeaponExtension
    { 
        public static bool IsReloading(this IWeaponReloader weaponReloader) => weaponReloader.ReloadingTimer.Value != null;
    }
}