using TMPro;
using UnityEngine;

public class ContadorEffect : MonoBehaviour
{
    TMP_Text textComponent;

    public float floatSpeed = 1f;
    public float fadeDuration = 1f;

    //CanvasGroup canvasGroup;
    Vector3 moveDirection = Vector3.up;
    float timer;

    void Awake()
    {
        textComponent = GetComponentInChildren<TMP_Text>();
        Destroy(gameObject, 2);
        //canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        transform.position += moveDirection * floatSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        //canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

        //if (canvasGroup.alpha <= 0f)
        //{
        //    Destroy(gameObject);
        //}
    }

    public void SetText(string moneyText, Color color)
    {
        textComponent.text = moneyText;
        textComponent.color = color;
    }

}
