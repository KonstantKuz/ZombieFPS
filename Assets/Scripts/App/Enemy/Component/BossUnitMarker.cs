using App.Enemy.Service;
using Feofun.Components;
using UnityEngine;
using Zenject;

namespace App.Enemy.Component
{
    public class BossUnitMarker : MonoBehaviour, IInitializable<Unit.Unit>
    {
        [Inject] private BossFightService _bossFightService;
        
        public void Init(Unit.Unit owner)
        {
            _bossFightService.Init(owner);
        }
    }
}