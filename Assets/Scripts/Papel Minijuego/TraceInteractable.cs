using UnityEngine;

public class TraceInteractable : MonoBehaviour
{
    public TraceShapeData[] possibleShapes;
    public TraceMinigameController controller;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click detectado");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast golpeó: " + hit.collider.name);

                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Se hizo click en el papel correcto");

                    if (possibleShapes.Length == 0)
                    {
                        Debug.LogError("No hay formas asignadas en el Inspector");
                        return;
                    }

                    if (controller == null)
                    {
                        Debug.LogError("No hay controller asignado");
                        return;
                    }

                    TraceShapeData randomShape =
                        possibleShapes[Random.Range(0, possibleShapes.Length)];

                    Debug.Log("Iniciando minijuego con forma: " + randomShape.name);

                    controller.StartMinigame(randomShape);
                }
                else
                {
                    Debug.Log("Click no fue en el papel");
                }
            }
            else
            {
                Debug.Log("Raycast no golpeó nada");
            }
        }
    }
}
