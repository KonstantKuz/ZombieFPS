namespace App.ABTest
{
    public static class ABTestExt
    {
        public static bool Control(this Feofun.ABTest.ABTest abTest) =>
                abTest.CurrentVariantId.Equals(ABTestVariantId.Control.ToCamelCase());
    }
}