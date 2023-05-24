using Feofun.ABTest.Providers;
using Zenject;

namespace Feofun.ABTest.Installer
{
    public static class ABTestServicesInstaller
    {
        public static void Install(DiContainer container, string controlVariant, IABTestProvider provider = null)
        {
            container.Bind<ABTest>().AsSingle();
            container.Bind<IABTestProvider>()
                .To<OverrideABTestProvider>()
                .AsSingle()
                .WithArguments(provider, controlVariant);
        }

        public static void Install<T>(DiContainer container, T controlVariant, IABTestProvider provider = null) where T: struct //enum
        {
            Install(container, ToCamelCase(controlVariant), provider);
        }
        
        private static string ToCamelCase<T>(T enumValue) where T: struct
        {
            var variantId = enumValue.ToString();
            return char.ToLower(variantId[0]) + variantId.Substring(1);
        }
    }
}