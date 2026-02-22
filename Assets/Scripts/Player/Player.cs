using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [HideInInspector] public Client client;
    public TalksThemes _talkTheme;
    public bool help;

    [Header("Scene")]
    //[SerializeField] string _sceneLose, _sceneWin;
    [SerializeField] string _sceneWin;
    [SerializeField] int _cashCondition = 1000;
    public int getDamage = 10;
    public Contador contador;
    private AsyncOperation _cargaEscenaWin;


    [Header("Puntos y Cordura")]
    public int cordura = 100;
    public int _corduraDanio = 10;
    public int _corduraMatarCliente = 15;
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] TMP_Text _selectionText;
    [SerializeField] TMP_Text _corduraText;
    public int _score { get; private set; } = 0;
    public Image _corduraImageFill;



    private void Update()
    {
        if (_score < 0) _score = 0;
        _scoreText.text = "$ " + _score;
    }

    public void MoreMoney(int money)
    {
        _score += money;
        contador.MostrarGanancia(money);
    }

    public void LessMoney(int money)
    {
        _score -= money;
        contador.DescontarGanancia(money);
    }

}
