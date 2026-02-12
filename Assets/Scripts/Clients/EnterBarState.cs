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
    }

    public void OnUpdate()
    {
        Debug.Log("entrando");
        var dir = _client.chair.transform.position + new Vector3(1, 0, 0) - _client.transform.position;
        _client.transform.forward = -dir;
        _client.transform.position += (dir * _client.speed * Time.deltaTime);

        if (MathF.Abs(dir.x - 1) < 0.1f)
        {
            _fsm.ChangeState(TypeFSM.Order);
        }

    }

    public void OnExit()
    {
    }


}
