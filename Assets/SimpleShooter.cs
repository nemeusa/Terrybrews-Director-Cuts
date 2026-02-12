using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class SimpleShooter : MonoBehaviour
{
    [Header("Disparo")]
    public float fireRate = 0.2f;
    private float nextFireTime;

    [Header("Post Processing")]
    public Volume globalVolume;
    private ColorAdjustments colorAdjustments;
    public float flashIntensity = 3f;
    public float flashDuration = 1f;
    public ScreenEffectController screenEffect;

    [Header("Retroceso")]
    public Transform weaponTransform;
    public Vector3 recoilOffset = new Vector3(0, 0, -0.15f);
    public float recoilTime = 0.08f;

    private Vector3 weaponStartPos;

    void Start()
    {
        weaponStartPos = weaponTransform.localPosition;

        if (globalVolume.profile.TryGet(out colorAdjustments) == false)
        {
            Debug.LogError("Color Adjustments no encontrado en el Volume");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        screenEffect.TriggerEffect();

        Debug.Log("PUM");

        StartCoroutine(FlashScreen());
        StartCoroutine(Recoil());
    }

    IEnumerator FlashScreen()
    {
        colorAdjustments.postExposure.value = flashIntensity;

        float t = 0;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            colorAdjustments.postExposure.value = Mathf.Lerp(
                flashIntensity,
                0,
                t / flashDuration
            );
            yield return null;
        }

        colorAdjustments.postExposure.value = 0;
    }

    IEnumerator Recoil()
    {
        float t = 0;
        Vector3 targetPos = weaponStartPos + recoilOffset;

        // Retroceso hacia atrás
        while (t < recoilTime)
        {
            t += Time.deltaTime;
            weaponTransform.localPosition = Vector3.Lerp(
                weaponStartPos,
                targetPos,
                t / recoilTime
            );
            yield return null;
        }

        t = 0;

        // Vuelta suave
        while (t < recoilTime)
        {
            t += Time.deltaTime;
            weaponTransform.localPosition = Vector3.Lerp(
                targetPos,
                weaponStartPos,
                t / recoilTime
            );
            yield return null;
        }

        weaponTransform.localPosition = weaponStartPos;
    }
}
