using System.Collections.Generic;
using Feofun.Util;
using Newtonsoft.Json;

namespace App.Tutorial.Model
{
    public class ScenarioState
    {
        public bool IsCompleted;
        [JsonConverter(typeof(CustomHashSetConverter<string>))]
        public HashSet<string> CompletedSteps = new();
    }
}