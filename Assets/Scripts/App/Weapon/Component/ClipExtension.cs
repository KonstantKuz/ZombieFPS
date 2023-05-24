namespace App.Weapon.Component
{
    public static class ClipExtension
    {
        public static bool HasAmmo(this IClip clip)
        {
            return clip.AmmoCount.Value > 0;
        }

        public static bool IsFull(this IClip clip)
        {
            return clip.AmmoCount.Value >= clip.Size;
        }
    }
}