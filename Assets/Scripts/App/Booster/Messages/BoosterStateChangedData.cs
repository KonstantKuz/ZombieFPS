using System;
using App.Booster.Boosters;
using App.Booster.Config;

namespace App.Booster.Messages
{
    public enum BoosterState
    {
        Started,
        Stopped
    }

    public class BoosterStateChangedData
    {
        public readonly BoosterBase Booster;
        public readonly BoosterState State;

        public BoosterStateChangedData(BoosterBase booster, BoosterState state)
        {
            Booster = booster;
            State = state;
        }
    }
}