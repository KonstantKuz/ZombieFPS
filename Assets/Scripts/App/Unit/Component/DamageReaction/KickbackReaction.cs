using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace App.Unit.Component.DamageReaction
{
    public class KickbackReaction : MonoBehaviour
    {
        private Tween _kickBack;

        public static void TryExecuteOn(GameObject target, Vector3 direction, KickbackReactionParams reactionParams)
        {
            var kickbackReaction = target.GetComponentInParent<KickbackReaction>();
            if(kickbackReaction == null) return;
            kickbackReaction.OnKickback(direction, reactionParams);
        }

        private void OnKickback(Vector3 direction, KickbackReactionParams reactionParams)
        {
            Dispose();
            
            var resultPosition = transform.position + direction.normalized * reactionParams.Distance;
            var canKickback = NavMesh.SamplePosition(resultPosition, out var navMeshHit, reactionParams.Distance, NavMesh.AllAreas);
            if (!canKickback) return;
            
            _kickBack = transform.DOMove(navMeshHit.position, reactionParams.Duration).SetEase(Ease.Linear);
        }

        private void OnDisable() => Dispose();
        private void OnDestroy() => Dispose();

        private void Dispose()
        {
            _kickBack?.Kill(); 
        }
    }
}