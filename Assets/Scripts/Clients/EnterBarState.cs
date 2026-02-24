using System;
using UnityEngine;

public class EnterBarState : State
{
    FSM<TypeFSM> _fsm;
    Client _client;
    public EnterBarState(FSM<TypeFSM> fsm, Client client)
    {
        _fsm = fsm;
        _client = client;
    }

    public void OnEnter()
    {
        _client.SetRuta(_client.tableManager.rutaEnter);
    }

    public void OnUpdate()
    {
        Debug.Log("entrando");
        if (_client.ruta.Count != 0)
        _client.FollowTarget();

        if (_client.ruta.Count == 0)
        {
            Debug.Log("frente la barra");
            var dir = _client.chair.transform.position + new Vector3(1, 0, 0) - _client.transform.position;
            _client.transform.forward = -dir;
            _client.transform.position += (dir * _client.speed * Time.deltaTime);

            if (MathF.Abs(dir.x - 1) < 0.1f) _fsm.ChangeState(TypeFSM.Order);
            
        }


    }

    public void OnExit()
    {
    }


}
