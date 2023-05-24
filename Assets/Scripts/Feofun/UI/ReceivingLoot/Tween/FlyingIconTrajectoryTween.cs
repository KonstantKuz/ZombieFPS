﻿using DG.Tweening;
using Feofun.UI.ReceivingLoot.Model;
using UnityEngine;

namespace Feofun.UI.ReceivingLoot.Tween
{
    public static class FlyingIconTrajectoryTween
    {
        public static DG.Tweening.Tween Play(FlyingIconTrajectory trajectory, RectTransform rectTransform)
        {
            return DOTween.To(() => 0, elapsedTime => { UpdatePosition(rectTransform, elapsedTime, trajectory); }, trajectory.Time, trajectory.Time)
                          .SetEase(Ease.Linear);
        }

        private static void UpdatePosition(RectTransform rectTransform, float elapsedTime, FlyingIconTrajectory trajectory)
        {
            rectTransform.position = CalcParabolaWithHeight(trajectory.StartPosition, trajectory.RemovePosition, trajectory.Height,
                                                            elapsedTime / trajectory.Time);
        }

        private static Vector2 CalcParabolaWithHeight(Vector2 start, Vector2 end, float height, float t)
        {
            return new Vector2(((Vector3) Vector2.Lerp(start, end, t)).x, Func(t) + Mathf.Lerp(start.y, end.y, t));
            float Func(float x) => (float) (-4.0 * height * Mathf.Pow(x, 2f) + 4.0 * height * x);
        }
    }
}