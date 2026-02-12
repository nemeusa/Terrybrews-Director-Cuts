using UnityEngine;
using System.Collections;

public class ScreenEffectController : MonoBehaviour
{
    [Header("Material del Shader Fullscreen")]
    public Material screenMaterial;

    [Header("Parametros del efecto")]
    public float maxIntensity = 1f;
    public float duration = 0.15f;

    int intensityID;

    void Awake()
    {
        intensityID = Shader.PropertyToID("_Intensity");
        screenMaterial.SetFloat(intensityID, 0);
    }

    public void TriggerEffect()
    {
        StartCoroutine(EffectRoutine());
    }

    IEnumerator EffectRoutine()
    {
        float t = 0;
        screenMaterial.SetFloat(intensityID, maxIntensity);

        while (t < duration)
        {
            t += Time.deltaTime;
            float value = Mathf.Lerp(maxIntensity, 0, t / duration);
            screenMaterial.SetFloat(intensityID, value);
            yield return null;
        }

        screenMaterial.SetFloat(intensityID, 0);
    }
}
