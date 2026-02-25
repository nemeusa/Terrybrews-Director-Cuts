using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
        //gameObject.SetActive(false);
        transform.LookAt(transform.position + mainCamera.forward);

    }

    //void LateUpdate() // Usamos LateUpdate para que la cámara ya se haya movido
    //{
    //    // Hace que el objeto mire hacia la cámara
    //    transform.LookAt(transform.position + mainCamera.forward);

    //    // Opcional: Si quieres que el globo aparezca/desaparezca con un efecto
    //    // podrías añadir un pequeño código de escala aquí.
    //}
}
