using Feofun.UI.Components;
using UnityEngine;

namespace App.UI.Screen.World.Player.Health
{
    public class HealthPresenter : MonoBehaviour, IUiInitializable<Unit.Unit>
    {
        [SerializeField] private HealthBarView _healthBarView;

        public void Init(Unit.Unit owner)
        {
            var model = new HealthBarModel(owner.Health);
            _healthBarView.Init(model);
        }
    }
}
