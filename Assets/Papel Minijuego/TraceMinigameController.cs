using UnityEngine;

public class TraceMinigameController : MonoBehaviour
{
    public Canvas canvas;
    public LineRenderer shapeLine;
    public LineRenderer playerLine;
    public MonoBehaviour mouseLook;

    private TraceShapeData currentShape;
    private bool isDrawing = false;

    public void StartMinigame(TraceShapeData shape)
    {
        Debug.Log("StartMinigame llamado");

        if (canvas == null)
        {
            Debug.LogError("Canvas no asignado");
            return;
        }

        if (shapeLine == null || playerLine == null)
        {
            Debug.LogError("LineRenderers no asignados");
            return;
        }

        if (mouseLook == null)
        {
            Debug.LogError("MouseLook no asignado");
            return;
        }

        currentShape = shape;

        canvas.gameObject.SetActive(true);
        Debug.Log("Canvas activado");

        mouseLook.enabled = false;
        Debug.Log("MouseLook desactivado");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Cursor liberado");

        DrawShape();

        playerLine.positionCount = 0;
        playerLine.startColor = shape.traceColor;
        playerLine.endColor = shape.traceColor;

        isDrawing = true;
        Debug.Log("Minijuego iniciado correctamente");
    }

    void Update()
    {
        if (!isDrawing) return;

        if (Input.GetMouseButton(0))
        {
            Debug.Log("Dibujando...");
            AddPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse soltado, verificando dibujo");
            CheckCompletion();
        }
    }

    void AddPoint(Vector2 screenPos)
    {
        int index = playerLine.positionCount;
        playerLine.positionCount++;
        playerLine.SetPosition(index, screenPos);
    }

    void DrawShape()
    {
        if (currentShape.points == null || currentShape.points.Length == 0)
        {
            Debug.LogError("La forma no tiene puntos configurados");
            return;
        }

        shapeLine.positionCount = currentShape.points.Length;

        for (int i = 0; i < currentShape.points.Length; i++)
        {
            shapeLine.SetPosition(i, currentShape.points[i]);
        }

        Debug.Log("Forma dibujada con " + currentShape.points.Length + " puntos");
    }

    void CheckCompletion()
    {
        if (playerLine.positionCount < 5)
        {
            Debug.Log("Muy pocos puntos dibujados");
            playerLine.positionCount = 0;
            return;
        }

        int correctPoints = 0;

        for (int i = 0; i < playerLine.positionCount; i++)
        {
            Vector3 playerPoint = playerLine.GetPosition(i);

            foreach (Vector2 shapePoint in currentShape.points)
            {
                float dist = Vector2.Distance(playerPoint, shapePoint);

                if (dist <= currentShape.allowedError)
                {
                    correctPoints++;
                    break;
                }
            }
        }

        float accuracy = (float)correctPoints / playerLine.positionCount;

        Debug.Log("Precisión: " + accuracy);

        if (accuracy > 0.7f)
        {
            Debug.Log("Dibujo correcto");
            EndMinigame();
        }
        else
        {
            Debug.Log("Dibujo incorrecto, reiniciando");
            playerLine.positionCount = 0;
        }
    }

    void EndMinigame()
    {
        isDrawing = false;

        canvas.gameObject.SetActive(false);
        Debug.Log("Canvas desactivado");

        mouseLook.enabled = true;
        Debug.Log("MouseLook reactivado");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Minijuego terminado correctamente");
    }
}
