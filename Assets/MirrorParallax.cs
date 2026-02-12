using UnityEngine;

public class MirrorParallax : MonoBehaviour
{
    public Transform playerCamera;

    [Header("Rotation")]
    public float rotationMultiplier = 0.2f;
    public float smooth = 8f;

    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        // Rotación horizontal del jugador (yaw)
        float playerYaw = playerCamera.localEulerAngles.y;

        // Convertir 0–360 a -180–180
        if (playerYaw > 180f)
            playerYaw -= 360f;

        // Invertimos la rotación
        float mirrorYaw = -playerYaw * rotationMultiplier;

        Quaternion targetRotation =
            startRotation * Quaternion.Euler(0f, mirrorYaw, 0f);

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * smooth
        );
    }
}
