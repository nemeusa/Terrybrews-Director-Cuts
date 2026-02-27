using UnityEngine;

public class BarMonster : MonoBehaviour
{
    FSM<MonsterState> _fsm;

    public Transform player;
    public float speed = 1.5f;
    public float hideSpeed = 4f;
    public float lookThreshold = 0.6f;

    public Transform[] hideSpots; // objetos detrás de los que se esconde

    private MonsterState currentState;
    private Vector3 spawnPosition;
    private Transform currentHideSpot;

    void Start()
    {
        spawnPosition = transform.position;
        currentState = MonsterState.Idle;
    }

    private void Awake()
    {

        //_fsm = new FSM<MonsterState>();
        //_fsm.AddState(MonsterState.Idle, new IdleState(_fsm, this));
        //_fsm.AddState(MonsterState.Stalking, new StalkingState(_fsm, this));
        //_fsm.AddState(MonsterState.Hiding, new HidingState(_fsm, this));
        //_fsm.AddState(MonsterState.Retreating, new RetreatingState(_fsm, this));
     
        //_fsm.ChangeState(MonsterState.Idle);
    }

    void Update()
    {
    }
}

public enum MonsterState
{
    Idle,
    Stalking,
    Hiding,
    Retreating
}