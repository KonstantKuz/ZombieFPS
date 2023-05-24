﻿using App.Player.Progress.Model;
using Feofun.Repository;

namespace App.Player.Progress.Service
{
    public class PlayerProgressRepository : LocalPrefsSingleRepository<PlayerProgress>
    {
        protected PlayerProgressRepository() : base("PlayerProgress_v1")
        {
        }
    }
}