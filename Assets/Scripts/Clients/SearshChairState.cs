using System.Collections;
using System.Collections.Generic;
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
        // _enemyCat.GetComponent<MeshRenderer>().material.color = Color.white;

        Debug.Log("Returning ENTER");

        // buscar el nodo de patrulla más cercano
        CustomNodes nearestPatrolNode = null;
        float minDist = Mathf.Infinity;
        foreach (var patrolNode in _client.followClientsManager.allChairs)
        {
            float dist = Vector3.Distance(_client.transform.position, patrolNode.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestPatrolNode = patrolNode;
            }
        }

        if (nearestPatrolNode != null)
        {
            // calcular A*
            var path = _client.pathManager.pathfinding.CalculateAStar(
                _client.FindClosestNode(), nearestPatrolNode);

            if (_client.pathRoutine != null)
                _client.StopCoroutine(_client.pathRoutine);

            _client.pathRoutine = _client.StartCoroutine(FollowReturnPath(path));
        }
    }

    public void OnUpdate()
    {
      
    }

    public void OnExit()
    {
        if (_client.pathRoutine != null)
            _client.StopCoroutine(_client.pathRoutine);
    }

    IEnumerator FollowReturnPath(List<CustomNodes> path)
    {
        if (path == null || path.Count == 0)
        {
            //_fsm.ChangeState(TypeFSM.Walk);
            yield break;
        }

        path.RemoveAt(0);

        while (path.Count > 0)
        {
            if (_fsm.CurrentStateKey != TypeFSM.Searsh)
                yield break;

            _client.FollowTarget(path[0].transform);

            if (_client.Mindistance(path[0].transform, 0.2f))
                path.RemoveAt(0);

            yield return null;
        }

        //_fsm.ChangeState(TypeFSM.Walk);
    }
}
