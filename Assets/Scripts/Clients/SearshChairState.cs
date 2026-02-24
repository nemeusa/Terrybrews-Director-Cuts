using UnityEngine;

public class SearshChairState : State
{
    FSM<TypeFSM> _fsm;
    Client _client;



    public SearshChairState(FSM<TypeFSM> fsm, Client client)
    {
        _fsm = fsm;
        _client = client;
    }

    public void OnEnter()
    {
        _client.player.MoreMoney(100);
        Debug.Log("buscando mesa");
        if (_client.mesaLibre == null) _client.mesaLibre = _client.tableManager.ObtenerMesaLibre();

        if (_client.mesaLibre != null)
        {
            //_client.IrHacia(mesaLibre.seatPoint);
            _client.SetRuta(_client.mesaLibre.rutaSeatPoint);
        }
    }

    public void OnUpdate()
    {
         _client.FollowTarget();
        //if (Vector3.Distance(_client.transform.position, _client.destino.position) < 0.1f) Debug.Log("Sentado");

    }

    public void OnExit()
    {
        //_client.rutaExit = new Queue<Transform>(mesaLibre.rutaExitPoint);
        _client.SetRuta(_client.mesaLibre.rutaExitPoint);
    }
}
