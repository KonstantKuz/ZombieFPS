using App.Player.Component.StateMachine;
using UnityEngine;

namespace App.Player.Config.StateMachine
{
    public abstract class StateConfigBase : ScriptableObject
    { 
        public abstract PlayerState State { get;}
    }
}