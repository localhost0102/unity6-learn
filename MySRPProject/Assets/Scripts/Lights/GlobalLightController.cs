using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightController : MonoBehaviour
{
    private GlobalLightInput controls;
    private float inputValue;

    private Light2D globalLight;
    private Light2D[] otherLights;
    private float globalLightOriginalIntensity = 1f;

    [SerializeField] private float intensityStep = 0.5f;
    [SerializeField] private float minIntensity = 0.005f;
    [SerializeField] private float maxIntensity = 1f;

    private float[] otherLightsOriginalIntensities;

    private void Awake()
    {
        globalLight = GetComponent<Light2D>();
        globalLightOriginalIntensity = globalLight.intensity;

        var allLights = FindObjectsOfType<Light2D>();
        otherLights = allLights.Where(x => x != globalLight).ToArray();

        // Cache their original intensities
        otherLightsOriginalIntensities = otherLights.Select(l => l.intensity).ToArray();

        controls = new GlobalLightInput();

        controls.LightControllingInput.LightIncreaseDecrease.performed += ctx => inputValue = ctx.ReadValue<float>();
        controls.LightControllingInput.LightIncreaseDecrease.canceled += ctx => inputValue = 0;
    }

    private void OnEnable()
    {
        controls.LightControllingInput.Enable();
    }

    private void OnDisable()
    {
        controls.LightControllingInput.Disable();
    }

    private void Update()
    {
        if (Mathf.Abs(inputValue) > 0.01f)
        {
            AdjustGlobalLight();
        }

        AdjustOtherLights();
    }

    private void AdjustGlobalLight()
    {
        globalLight.intensity += inputValue * intensityStep * Time.deltaTime;
        globalLight.intensity = Mathf.Clamp(globalLight.intensity, minIntensity, maxIntensity);
    }

    private void AdjustOtherLights()
    {
        // Inverse factor: when global light is at max, other lights = 0
        // When global light is at min, other lights = original intensities

        float t = Mathf.InverseLerp(maxIntensity, minIntensity, globalLight.intensity);
        for (int i = 0; i < otherLights.Length; i++)
        {
            otherLights[i].intensity = otherLightsOriginalIntensities[i] * t;
        }
    }
}
