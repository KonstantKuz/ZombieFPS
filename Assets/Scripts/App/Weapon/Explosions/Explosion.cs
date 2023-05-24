using System;
using System.Collections.Generic;
using System.Linq;
using App.Unit.Component.Attack;
using App.Unit.Component.Health;
using App.Weapon.Service;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using JetBrains.Annotations;
using UnityEngine;

namespace App.Weapon.Explosions
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] 
        private float _maxDamageRadiusPart = 1f;

        public void Explode(float damageRadius, 
            Action<HitInfo> hitCallback, 
            [CanBeNull] GameObject excludedTarget,
            LayerMask layerMask)
        {
            ExplodeInRadius(transform.position, damageRadius, excludedTarget, layerMask, hitCallback);
        }

        private void ExplodeInRadius(Vector3 explosionCenter, 
            float explosionRadius, 
            [CanBeNull] GameObject excludedTarget,
            LayerMask layerMask,
            Action<HitInfo> hitCallback)
        {
            var hitDictionary = GetDistinctHits(explosionCenter, 
                explosionRadius, 
                excludedTarget == null ? null : excludedTarget.GetComponentInParent<IDamageable>(), 
                layerMask);
            hitDictionary.Values.ForEach(it => hitCallback?.Invoke(it));
        }

        private Dictionary<IDamageable, HitInfo> GetDistinctHits(Vector3 explosionCenter, 
            float explosionRadius, 
            [CanBeNull] IDamageable excludedTarget,
            LayerMask layerMask)
        {
            var hitList = Physics.OverlapSphere(explosionCenter, explosionRadius, layerMask);

            var hitDictionary = new Dictionary<IDamageable, HitInfo>();
            foreach (var hit in hitList)
            {
                var damagable = hit.GetComponentInParent<IDamageable>();
                if (damagable == null || damagable == excludedTarget) continue;
                var hitFraction = GetHitFraction(explosionCenter, hit.ClosestPoint(explosionCenter), explosionRadius);
                if (!hitDictionary.ContainsKey(damagable) || hitDictionary[damagable].HitFraction < hitFraction)
                {
                    var hitPos = hit.ClosestPoint(explosionCenter);
                    var hitNormal = (explosionCenter - hitPos).normalized; 
                    hitDictionary[damagable] = new HitInfo(hit.gameObject, hitPos, hitNormal, hitFraction);
                }
            }

            return hitDictionary;
        }

        private float GetHitFraction(Vector3 explosionCenter, Vector3 targetHitPos, float explosionRadius)
        {
            return Mathf.InverseLerp(explosionRadius, 
                explosionRadius * _maxDamageRadiusPart, 
                Vector3.Distance(explosionCenter, targetHitPos));
        }
        
        public static void HitInRadius(HitInfo hitInfo,
            float hitRadius, LayerMask layerMask, 
            ProjectileHitService hitService,
            Action<HitInfo> callback)
        {
            if (hitRadius.IsZero()) return;

            Physics.OverlapSphere(hitInfo.Position, hitRadius, layerMask)
                .Select(it => it.gameObject)
                .Select(hit => hitService.OverrideHit(new HitInfo(hit, hitInfo.Position, hitInfo.Normal)))
                .ForEach(hit => callback?.Invoke(hit));
        }
    }
}