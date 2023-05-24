using System;

namespace App.Unit.Component.StateMachine
{
    public class StateInfo
    {
        public string Name { get; set; }
        public Func<bool> EnterCondition { get; set; }

        public Action OnEnterState { get; set;}

        public bool? IsInitialState { get; set;}
        
    }
}