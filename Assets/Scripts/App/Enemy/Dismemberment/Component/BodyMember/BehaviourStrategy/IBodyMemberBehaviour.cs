using App.Enemy.Dismemberment.Model;
using Feofun.Components;
using Feofun.Extension;
using UnityEngine;

namespace App.Enemy.Dismemberment.Component.BodyMember.BehaviourStrategy
{
    public interface IBodyMemberBehaviour : IInitializable<Unit.Unit>
    {
        bool ShouldDetach { get; }

        void Kill();

        void Dismember();
        
        public static IBodyMemberBehaviour CreateBehaviour(GameObject root, BodyMemberType bodyMemberType)
        {
            return bodyMemberType == BodyMemberType.Torso
                ? root.RequireComponent<TorsoBehaviour>()
                : root.GetOrAddComponent<LimbsBehaviour>();
        }
        
    }
}