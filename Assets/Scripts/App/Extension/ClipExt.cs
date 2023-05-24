using App.Weapon.Component;

namespace App.Extension
{
    public static class ClipExt
    {

        public static int GetAmmoCountForFullLoad(this IClip clip)
        {
            return clip.Size - clip.AmmoCount.Value;
        }
    }
}