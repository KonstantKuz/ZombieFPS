using UnityEngine;
using System.Collections;
using UnityEngine.Profiling;

public class FPSLightCurves : MonoBehaviour
{
    public AnimationCurve LightCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float GraphTimeMultiplier = 1, GraphIntensityMultiplier = 1;

    private bool canUpdate;
    private float startTime;
    private Light lightSource;

    private void Awake()
    {
        lightSource = GetComponent<Light>();
        lightSource.intensity = LightCurve.Evaluate(0);
    }

    private void OnEnable()
    {
        Profiler.BeginSample("FPSLightCurves.OnEnable");
        startTime = Time.time;
        canUpdate = true;
        lightSource.enabled = true;
        Profiler.EndSample();
    }

    private void Update()
    {
        var time = Time.time - startTime;
        if (canUpdate) {
            var eval = LightCurve.Evaluate(time / GraphTimeMultiplier) * GraphIntensityMultiplier;
            lightSource.intensity = eval;
        }

        if (time >= GraphTimeMultiplier)
        {
            canUpdate = false;
            lightSource.enabled = false;
        }
    }
}