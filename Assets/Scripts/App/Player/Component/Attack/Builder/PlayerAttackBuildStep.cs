using App.Unit.Component.Attack.Builder;
using UnityEngine;

namespace App.Player.Component.Attack.Builder
{
    public abstract class PlayerAttackBuildStep : MonoBehaviour
    { 
        public abstract void Build(AttackBuilder attackBuilder, ReloadableInitData data);
    }
}