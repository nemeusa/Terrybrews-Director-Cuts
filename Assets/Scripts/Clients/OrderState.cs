using System.Collections;
using UnityEngine;

public class OrderState : State
{
    FSM<TypeFSM> _fsm;
    Client _client;

    public OrderState(FSM<TypeFSM> fsm, Client client)
    {
        _fsm = fsm;
        _client = client;
    }

    public void OnEnter()
    {
        //_client.chair.Ocuppy();
        //_client.soundEfects.PlaySoundFromGroup(2);
        NuevaPeticion();
        _client.dialogue.CharlaThemes();

        _client.globoTexto.SetActive(true);
        _client.namesAndOffices();
        _client.textOrder.text = $"<color=black>Quiero </color> {_client.currentRequest}";
        _client.textCharla.text = _client.dialogue.currentDialogue;
        _client.colorDrink();
        var dir = _client.player.transform.position - _client.transform.position;
        _client.transform.forward = new Vector3(-dir.x, 0, -dir.z);
    }

    public void OnUpdate()
    {
        _client.transform.position = _client.chair.transform.position;

        

        //var globoDir = _client.player.transform.position - _client.transform.position;
        //_client.globoTexto.transform.forward = -globoDir;

        //_client.dialogue.Verification();


        Debug.Log("Order");
        _client.TextColor();


        if (_client.goodOrder || _client.badOrder)
        {
            Debug.Log("Entregado");
            if (_client.imposter) _fsm.ChangeState(TypeFSM.Attack);
            else
            {
                if (_client.goodOrder) _fsm.ChangeState(TypeFSM.Searsh);
                if (_client.badOrder) _fsm.ChangeState(TypeFSM.ExitBar);
            }
        }


    }

    public void OnExit()
    {
        _client.globoTexto.SetActive(false);
        _client.LeaveChair();
    }

    IEnumerator niceOrder()
    {
        _client.GetComponent<MeshRenderer>().material.color = Color.green;
        yield return new WaitForSeconds(0.1f);
    }


    public void NuevaPeticion()
    {
        if (_client.barManager.tutorial)
        {
            _client.currentRequest = _client.opciones[_client.barManager.indexBebida];


            // Incrementa el índice y reinicia si se pasa del final
            _client.barManager.indexBebida++;
            if (_client.barManager.indexBebida >= _client.opciones.Length)
            {
                _client.barManager.indexBebida = 0;
            }
        }

        else
            _client.currentRequest = _client.opciones[UnityEngine.Random.Range(0, _client.opciones.Length)];
    }
}
