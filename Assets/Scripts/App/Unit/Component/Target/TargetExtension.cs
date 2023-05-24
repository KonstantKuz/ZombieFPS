using JetBrains.Annotations;

namespace App.Unit.Component.Target
{
    public static class TargetExtension
    {
        public static bool IsValid([CanBeNull] this ITarget target)
        {
            return target is { IsValid: true };
        }
    }
}