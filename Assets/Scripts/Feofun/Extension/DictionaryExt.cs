using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Feofun.Extension
{
    public static class DictionaryExt
    {
        public static Dictionary<T1, T2> UnionWith<T1, T2>(this Dictionary<T1, T2> dict1, Dictionary<T1, T2> dict2)
        {
            return dict1.Union(dict2).ToDictionary(it => it.Key, it => it.Value);
        }
        
        public static string ConvertToJson(this Dictionary<string, object> parameters)
        {
            return JsonConvert.SerializeObject(parameters, Formatting.Indented);
        }
    }
}