using UnityEngine;

namespace App.Enemy.Dismemberment.Component.BodyMember.HitReaction
{
    public class HitReactionRaycastTest : MonoBehaviour
    {
        [SerializeField] private float _force;

        private void Update()
        {
            if(!UnityEngine.Input.GetMouseButtonDown(0)) return;
            var ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            var reactionMember = hit.collider.GetComponentInParent<MemberHitRagdollReaction>();
            if(reactionMember == null) return;
            reactionMember.PlayHitReaction(hit.point, ray.direction, _force);
        }
    }
}
