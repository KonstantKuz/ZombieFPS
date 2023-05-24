using System;
using System.Linq;
using App.Enemy.Dismemberment.Component.Body.Behaviour;
using App.Enemy.Dismemberment.Component.BodyMember;
using App.Unit.Component.Death;
using Dreamteck;
using Feofun.Components;
using Feofun.Components.ComponentMessage;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace App.Enemy.Dismemberment.Component.Body
{
    public class BodyBehaviourUpdater : MonoBehaviour, IInitializable<Unit.Unit>, IMessageListener<UnitDeathComponentMessage>
    {
        [SerializeField]
        private BodyBehaviour[] _bodyBehaviours;

        private BodyMemberBehaviour[] _bodyMembers;
        private BodyMembersInfo _bodyMembersInfo;

        public void Awake()
        {
            _bodyMembers = gameObject.GetComponentsInChildren<BodyMemberBehaviour>(true);
            _bodyBehaviours = GetComponentsInChildren<BodyBehaviour>();
        }
        
        public void Init(Unit.Unit unit)
        {
            _bodyMembersInfo = new BodyMembersInfo(_bodyMembers);
            _bodyMembersInfo.OnMembersChanged += UpdateBehaviour;
            _bodyBehaviours.ForEach(it => it.Init(unit, _bodyMembersInfo));
        }

        private void UpdateBehaviour()
        {
            _bodyBehaviours.ForEach(it => it.UpdateStates());
        }
        
        public void OnMessage(UnitDeathComponentMessage msg) => Dispose();
        
        private void OnDisable() => Dispose();

        private void Dispose()
        {
            if (_bodyMembersInfo != null) {
                _bodyMembersInfo.OnMembersChanged -= UpdateBehaviour;
            }
            _bodyBehaviours.OfType<IDisposable>().ForEach(it => it.Dispose());
        }


    }
}