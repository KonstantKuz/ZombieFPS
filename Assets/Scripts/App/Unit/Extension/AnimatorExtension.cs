using System.Linq;
using UnityEngine;

namespace App.Unit.Extension
{
    public static class AnimatorExtension
    {
        private static readonly int _isFallingHash = Animator.StringToHash("IsFalling");

        public static bool IsInFallingState(this Animator animator)
        {
            return animator.HasParameter(_isFallingHash) && animator.GetBool(_isFallingHash);
        }
        public static bool HasParameter(this Animator animator, int paramHash)
        {
            return animator.parameters.Any(it => it.nameHash.Equals(paramHash));
        }
        
    }
}