using JetBrains.Annotations;

namespace Feofun.Advertisment.Data
{
    public class AdsResult
    {
        public bool Success => AdFail == null;
      
        [CanBeNull]
        public AdFail AdFail { get; }
        
        private AdsResult([CanBeNull] AdFail adFail = null)
        {
            AdFail = adFail;
        }
        public static AdsResult CreateSuccess() => new();

        public static AdsResult CreateFail(string message, AdFailStatus status)
        {
            return new AdsResult(new AdFail(message, status));
        }

    }
}