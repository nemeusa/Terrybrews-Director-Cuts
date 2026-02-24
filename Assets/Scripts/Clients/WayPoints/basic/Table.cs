using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Transform seatPoint;
    public Transform pointView;
    public List<Transform> rutaSeatPoint;
    public List<Transform> rutaExitPoint;
    public bool ocuppy;

    private void Start()
    {
        rutaSeatPoint.Add(transform);
        seatPoint = transform;
    }

    public void Ocupping()
    {
        ocuppy = true;
    }

    public void Free()
    {
        ocuppy = false;
    }
}
