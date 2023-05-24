using UnityEngine;

namespace App.Unit.Component.Attack
{
    public readonly struct AttackComponentInitData
    {
        public readonly Unit Unit;  
        public readonly Transform AttackRoot;

        public AttackComponentInitData(Unit unit, Transform attackRoot)
        {
            Unit = unit;
            AttackRoot = attackRoot;
        }
    }
}