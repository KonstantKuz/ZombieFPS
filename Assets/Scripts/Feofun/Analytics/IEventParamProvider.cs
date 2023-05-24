using System.Collections.Generic;

namespace Feofun.Analytics
{
    public interface IEventParamProvider
    {
        Dictionary<string, object> GetParams(IEnumerable<string> paramNames);
    }
}