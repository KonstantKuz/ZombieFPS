using App.Unit.Component.Death;
using Dreamteck;
using Feofun.Components;
using Feofun.Components.ComponentMessage;
using UnityEngine;

namespace App.Enemy.Dismemberment.Component.BodyMember
{
    [RequireComponent(typeof(BodyMemberBehaviour))]
    public class BodyMemberElementsDisabler : MonoBehaviour, IMessageListener<UnitDeathComponentMessage>, IInitializable<Unit.Unit>
    {
        [SerializeField]
        private Transform[] _elements;

        private BodyMemberBehaviour _bodyMemberBehaviour;

        public void Awake()
        {
            _bodyMemberBehaviour = GetComponent<BodyMemberBehaviour>();
            _bodyMemberBehaviour.OnMemberDetached += OnMemberDetached;
        }

        public void Init(Unit.Unit data) => SetElementsActive(true);
        private void OnMemberDetached(BodyMemberBehaviour obj) => SetElementsActive(false);
        public void OnMessage(UnitDeathComponentMessage msg) => SetElementsActive(false);
        private void SetElementsActive(bool isActive) => _elements.ForEach(it=>it.gameObject.SetActive(isActive));

        private void OnDestroy()
        {
            _bodyMemberBehaviour.OnMemberDetached -= OnMemberDetached;
        }
    }
}