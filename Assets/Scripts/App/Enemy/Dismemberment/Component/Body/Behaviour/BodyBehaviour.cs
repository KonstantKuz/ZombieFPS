using App.Unit.Component.StateMachine;
using UnityEngine;

namespace App.Enemy.Dismemberment.Component.Body.Behaviour
{
    [RequireComponent(typeof(BodyBehaviourUpdater))]
    public abstract class BodyBehaviour : BehaviorSelector
    {
        public abstract void Init(Unit.Unit unit, BodyMembersInfo membersInfo);
        
    }
}