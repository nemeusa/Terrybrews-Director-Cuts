using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class Client : MonoBehaviour
{
    FSM<TypeFSM> _fsm;



    public Color agua;
    public Color jugo;
    public Color cerveza;
    public Color gaseosa;

    public string[] opciones = { "Agua", "Jugo", "Cerveza", "Gaseosa" };


    [Header("Client dates")]
    [SerializeField] string[] names;
    [SerializeField] string profesion;
    public float speed, exitSpeed;
    [Range(0, 100)][SerializeField] int _imposterPorcentaje; 
    public bool imposter;
    public bool isDeath;
    [HideInInspector] public TMP_Text textNames;
    [HideInInspector] public TMP_Text textProfesion;


    [Header("Order")]
    public string[] charlaGood, charlaBad;
    [HideInInspector] public bool goodOrder, badOrder;
    [HideInInspector] public GameObject globoTexto;
    [HideInInspector] public string currentRequest;
    [HideInInspector] public TMP_Text textOrder;
    [HideInInspector] public TMP_Text textCharla;


    public ParticleSystem goodClientParticles, badClientParticles;
    [HideInInspector] public Dialogue dialogue;
    [HideInInspector] public Chair chair;
    [HideInInspector] public Player player;
    [HideInInspector] public BarManager barManager;

    [Header("Waypoints")]
    [HideInInspector] public PathManager pathManager;
    [HideInInspector]public SpawnFollowClientsManager followClientsManager;
    [HideInInspector]public TableManager tableManager;
    public Coroutine pathRoutine;
    public Queue<Transform> ruta = new Queue<Transform>();
    public Queue<Transform> rutaExit = new Queue<Transform>();
    public Queue<Transform> rutaEnterBar = new Queue<Transform>();
    public Queue<Transform> rutaExitBar = new Queue<Transform>();
    public Transform destino;
    public bool onTable;
    public Table mesaLibre;



    private void Awake()
    {
        followClientsManager = SpawnFollowClientsManager.instance;
        tableManager = TableManager.instance;
        pathManager = followClientsManager.pathManager;
        dialogue = GetComponent<Dialogue>();

        _fsm = new FSM<TypeFSM>();
        _fsm.AddState(TypeFSM.EnterBar, new EnterBarState(_fsm, this));
        _fsm.AddState(TypeFSM.Order, new OrderState(_fsm, this));
        _fsm.AddState(TypeFSM.ExitBar, new ExitBarState(_fsm, this));
        _fsm.AddState(TypeFSM.Attack, new AttackState(_fsm, this));
        _fsm.AddState(TypeFSM.Death, new DeathState(_fsm, this));
        _fsm.AddState(TypeFSM.Searsh, new SearshChairState(_fsm, this));

        _fsm.ChangeState(TypeFSM.EnterBar);
        //_fsm.ChangeState(TypeFSM.Searsh);
    }

    private void Update()
    {
        _fsm.Execute();


        if (Input.GetButtonDown("Jump")) _fsm.ChangeState(TypeFSM.Searsh);

        if (isDeath)
        {
            _fsm.ChangeState(TypeFSM.Death);
        }
    }


    public void RandomImposter()
    {
        if (UnityEngine.Random.Range(0, 101) < _imposterPorcentaje) imposter = true;
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

    public void TextColor()
    {
        if (player.help)
        {
            if (!imposter) textCharla.color = UnityEngine.Color.green;
            else textCharla.color = UnityEngine.Color.red;
        }

        else textCharla.color = UnityEngine.Color.black;
    }

    public IEnumerator IsDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }




    public void namesAndOffices()
    {
        names = new string[]
        {
            "Mateo", "Santiago", "Benjamú‹", "Thiago", "Joaquú‹",
            "Lucas", "Martú‹", "Nicolás", "Tomás", "Dylan",
            "Leonardo", "Gabriel", "Matú}s", "Julián", "Gael",
            "Lautaro", "Bruno", "Emiliano", "Franco", "Andrés",
            "Simón", "Alan", "David", "Iván", "Federico",
            "Juan", "Facundo", "Axel", "Luciano", "Elú}s",
            "Agustú‹", "Jeremú}s", "Samuel", "Aarón", "Lorenzo",
            "Nahuel", "Valentino", "Enzo", "Ezequiel", "Maxi", "Roman", "ciro", "Breck", "becky", "Violeta", "boben",
            "Esteban", "Rodrigo", "Damián", "Leandro", "Sebastián",
            "Pablo", "Ignacio", "Ramiro", "Rafael", "Adrián",
            "Hugo", "Mauro", "Ariel", "Marcelo", "Messi", "Ramiro", 
            "Ulises", "Diego", "Amadeo", "Agustin", "Mariano", "Gonzalo",
            "Alonso", "Camilo", "Cristian", "Lisandro", "Guido",
            "Ismael", "Rubén", "Ivano", "Vú€tor", "Hernán",
            "Fabián", "Lauterio", "César", "Rocco", "Alanis",
            "Teo", "Blas", "Dante", "Emil", "Aarón",
            "Ezra", 
            "Bastián", "Odin", "Baltazar", "Elian", "Joaquim",
            "Salomón", "Lionel", "Jairo", "Ángel", "Kevin",
            "Jonás", "Gaetano", "Eitan", "Lázaro", "Matheo", "Federico", "Agustin"

        };

        textNames.text = names[UnityEngine.Random.Range(0, names.Length)];
        textProfesion.text = profesion;
    }
     
    public void FollowTarget()
    {

        transform.position = Vector3.MoveTowards(
        transform.position,
        destino.position,
          exitSpeed * Time.deltaTime
        );

        Vector3 direccion;
        if (!onTable) direccion = (destino.position - transform.position).normalized;
        else direccion = (mesaLibre.pointView.position - transform.position).normalized;
        transform.forward = -direccion;
        if (Vector3.Distance(transform.position, destino.position) < 0.1f) SiguientePunto();
    }

    public void SetRuta(List<Transform> puntos)
    {
        ruta = new Queue<Transform>(puntos);
        SiguientePunto();
    }

    void SiguientePunto()
    {
        if (ruta.Count == 0)
        {
            //moviendose = false;
            //Sentarse();
            Debug.Log("Sentado");
            StartCoroutine(ExitBar());
            return;
        }

        destino = ruta.Dequeue();
        //moviendose = true;
    }

    IEnumerator ExitBar()
    {
        onTable = true;
        yield return new WaitForSeconds(Random.Range(30, 61));
        onTable = false;
        _fsm.ChangeState(TypeFSM.ExitBar);
    }


    public void IrHacia(Transform nuevoDestino)
    {
        destino = nuevoDestino;
        Debug.Log("nuevo destino");
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
    Searsh,
    VIP
}
