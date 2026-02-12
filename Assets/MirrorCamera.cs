using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
    public Transform mainCamera;
    public Transform mirrorPlane;

    void LateUpdate()
    {
        Vector3 toCamera = mainCamera.position - mirrorPlane.position;

        Vector3 reflectedPosition = Vector3.Reflect(toCamera, mirrorPlane.forward);
        transform.position = mirrorPlane.position + reflectedPosition;
        Vector3 forward = Vector3.Reflect(mainCamera.forward, mirrorPlane.forward);
        Vector3 up = Vector3.Reflect(mainCamera.up, mirrorPlane.forward);
        transform.rotation = Quaternion.LookRotation(forward, up);
    }
}
