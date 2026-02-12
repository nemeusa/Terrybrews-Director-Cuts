using System.Collections;
using TMPro;
using UnityEngine;

public class Client : MonoBehaviour
{
    FSM<TypeFSM> _fsm;

    public float speed, exitSpeed;

    public bool imposter;


    public string[] charlaGood, charlaBad;


    public Color agua;
    public Color jugo;
    public Color cerveza;
    public Color gaseosa;

    public string[] opciones = { "Agua", "Jugo", "Cerveza", "Gaseosa" };

    [SerializeField] string[] names;
    [SerializeField] string profesion;


    [HideInInspector] public GameObject globoTexto;
    [HideInInspector] public string currentRequest;
    [HideInInspector] public TMP_Text textOrder;
    [HideInInspector] public TMP_Text textCharla;
    [HideInInspector] public TMP_Text textNames;
    [HideInInspector] public TMP_Text textProfesion;

    [HideInInspector]
    public Dialogue dialogue;


    [HideInInspector]
    public Chair chair;

    [HideInInspector]
    public Player player;

    [HideInInspector]
    public BarManager barManager;


    private void Awake()
    {
        dialogue = GetComponent<Dialogue>();

        _fsm = new FSM<TypeFSM>();
        _fsm.AddState(TypeFSM.EnterBar, new EnterBarState(_fsm, this));
        _fsm.AddState(TypeFSM.Order, new OrderState(_fsm, this));

        _fsm.ChangeState(TypeFSM.EnterBar);

    }

    private void Update()
    {
        _fsm.Execute();
    }


    public void RandomImposter()
    {
        if (UnityEngine.Random.Range(0, 101) > 50) imposter = true;
        else imposter = false;
        // Charla();
    }

    public void AssignChair(Chair chair)
    {
        this.chair = chair;
        this.chair.Ocuppy();
        StartCoroutine(GoSeat());
        globoTexto = this.chair.globo;
        textOrder = this.chair.textoPedido;
        textCharla = this.chair.textoCharla;
        textNames = this.chair.textoNames;
        textProfesion = this.chair.textoProfesion;

    }

    IEnumerator GoSeat()
    {
        var dir = transform.position - chair.transform.position;
        while (dir.x < 0.1)
        {
            //transform.forward = dir;
            transform.position -= Vector3.right * speed * Time.deltaTime;
            yield return null;
        }
    }

    public void LeaveChair()
    {
        chair.Free();
    }

    public void colorDrink()
    {
        if (currentRequest == "Agua")
        {
            // Debug.Log("funciona xd");
            textOrder.color = agua;
        }

        if (currentRequest == "Jugo")
        {
            // Debug.Log("funciona xd");
            textOrder.color = jugo;
        }

        if (currentRequest == "Cerveza")
        {
            // Debug.Log("funciona xd");
            textOrder.color = cerveza;
        }

        if (currentRequest == "Gaseosa")
        {
            // Debug.Log("funciona xd");
            textOrder.color = gaseosa;
        }
    }


    public void namesAndOffices()
    {
        names = new string[]
        {
            "Mateo", "Santiago", "Benjamín", "Thiago", "Joaquín",
            "Lucas", "Martín", "Nicolás", "Tomás", "Dylan",
            "Leonardo", "Gabriel", "Matías", "Julián", "Gael",
            "Lautaro", "Bruno", "Emiliano", "Franco", "Andrés",
            "Simón", "Alan", "David", "Iván", "Federico",
            "Juan", "Facundo", "Axel", "Luciano", "Elías",
            "Agustín", "Jeremías", "Samuel", "Aarón", "Lorenzo",
            "Nahuel", "Valentino", "Enzo", "Ezequiel", "Maxi", "Roman", "ciro", "Breck", "becky", "Violeta", "boben",
            "Esteban", "Rodrigo", "Damián", "Leandro", "Sebastián",
            "Pablo", "Ignacio", "Ramiro", "Rafael", "Adrián",
            "Hugo", "Mauro", "Ariel", "Marcelo", "Messi", "Ramiro", "Tomé",
            "Ulises", "Diego", "Amadeo", "Agustin", "Mariano", "Gonzalo",
            "Alonso", "Camilo", "Cristian", "Lisandro", "Guido",
            "Ismael", "Rubén", "Ivano", "Víctor", "Hernán",
            "Fabián", "Lauterio", "César", "Rocco", "Alanis",
            "Teo", "Blas", "Dante", "Emil", "Aarón",
            "Ezra", "Josué", "Dorian", "Axelino", "Iker",
            "Bastián", "Odin", "Baltazar", "Elian", "Joaquim",
            "Salomón", "Lionel", "Jairo", "Ángel", "Kevin",
            "Jonás", "Gaetano", "Eitan", "Lázaro", "Matheo", "Federico", "Agustin"

        };

        textNames.text = names[UnityEngine.Random.Range(0, names.Length)];
        textProfesion.text = profesion;
    }

}


public enum TypeFSM
{
    EnterBar,
    Order,
    ExitBar,
    Imposter,
    Attack,
    Death,
    VIP
}
