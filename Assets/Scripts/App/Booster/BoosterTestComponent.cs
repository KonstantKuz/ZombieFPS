using App.Booster.Config;
using App.Booster.Service;
using EasyButtons;
using UnityEngine;
using Zenject;

namespace App.Booster
{
    public class BoosterTestComponent : MonoBehaviour
    {
        [Inject]
        public BoosterService _boosterService;
        
        [Button]
        public void StartBooster(BoosterId id) => _boosterService.Start(id);

        [Button]
        public void StopBooster(BoosterId id) => _boosterService.Stop(id);
    }
}