using Feofun.Advertisment.Data;

namespace App.Advertisment.Extension
{
    public static class AdsResultExt
    {
        public static bool ShouldShowNoAdsNotification(this AdsResult adsResult)
        {
            return !adsResult.Success && (adsResult.AdFail.Status == AdFailStatus.NotAvailable ||
                                          adsResult.AdFail.Status == AdFailStatus.NotInitialized);
        }
    }
}