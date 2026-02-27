using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("Límites de rotación (grados)")]
    public float limitLeft = 10f;
    public float limitRight = 10f;
    public float limitUp = 7f;
    public float limitDown = 7f;

    [Header("Sensibilidad")]
    public float sensitivity = 2f;
    public float smoothSpeed = 6f;
    [Header("Tecla Reinicio")]
    public KeyCode resetCamera = KeyCode.LeftControl;    

    private Vector2 currentRotation;
    private Vector2 targetRotation;

    Vector3 startRot;

    public ShowShop _shop;

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        startRot = transform.localEulerAngles;
        currentRotation = new Vector2(startRot.y, startRot.x);
        targetRotation = currentRotation;
    }

    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        if (_shop.useShop)
        {
            targetRotation = startRot;
        }
        else
        {

            targetRotation.x += mouseX;
            targetRotation.y -= mouseY;
        }

        targetRotation.x = Mathf.Clamp(targetRotation.x,-limitLeft,limitRight);

        targetRotation.y = Mathf.Clamp(targetRotation.y, -limitUp, limitDown );

        if (Input.GetKeyDown(resetCamera) || Input.GetKeyDown(resetCamera)) {targetRotation = Vector2.zero;}

        currentRotation = Vector2.Lerp(currentRotation,targetRotation,Time.deltaTime * smoothSpeed);
        transform.localRotation = Quaternion.Euler(currentRotation.y,currentRotation.x,0f);
    }
}
