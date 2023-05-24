namespace Feofun.Analytics
{
    public interface IAnalytics
    {
        void Init();
        void ReportAdRewardedRequested(string placementId);

        void ReportAdRewardedShown(string placementId);

        void ReportAdRewardedNotShown(string placementId);
    }
}