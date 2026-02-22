using System.Collections;
using TMPro;
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
    public Coroutine pathRoutine;


    private void Awake()
    {
        followClientsManager = SpawnFollowClientsManager.instance;
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

    public void FollowTarget(Transform target)
    {
        Vector2 dir = target.transform.position - transform.position;

        dir.Normalize();

        Vector3 movement = new Vector3(dir.x, dir.y, 0f) * speed * Time.deltaTime;
        transform.position += movement;
        //rb.MovePosition(movement);
    }

    public bool Mindistance(Transform target, float minDistance)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (target.position - transform.position).normalized, minDistance);
        if (hit.collider != null)
            return false;

        return Vector2.Distance(transform.position,
            target.position) < minDistance;
    }


    public CustomNodes FindClosestNode()
    {
        CustomNodes closest = null;
        float minDist = Mathf.Infinity;
        foreach (var node in pathManager.pathfinding.GetAllNodes())
        {
            float dist = Vector3.Distance(transform.position, node.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = node;
            }
        }
        return closest;
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
