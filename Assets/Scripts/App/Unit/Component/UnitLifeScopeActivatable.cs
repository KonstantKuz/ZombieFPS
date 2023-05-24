using App.Unit.Component.Death;
using Dreamteck;
using Feofun.Components;
using Feofun.Components.ComponentMessage;
using UnityEngine;

namespace App.Unit.Component
{
    public class UnitLifeScopeActivatable : MonoBehaviour, IInitializable<Unit>, IMessageListener<UnitDeathComponentMessage>
    {
        [SerializeField] private Collider _rootCollider;
        [SerializeField] private GameObject[] _activatableGameObjects;
        
        public void Init(Unit data)
        {
            SetColliderActive(true);
            _activatableGameObjects.ForEach(it => it.SetActive(true));
        }

        public void OnMessage(UnitDeathComponentMessage msg)
        {
            SetColliderActive(false);
            _activatableGameObjects.ForEach(it => it.SetActive(false));
        }

        private void SetColliderActive(bool value)
        {
            if(_rootCollider == null) return;
            _rootCollider.enabled = value;
        }
    }
}
