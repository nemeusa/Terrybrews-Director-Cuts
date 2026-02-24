using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class TableManager : MonoBehaviour
{
    public static TableManager instance;
    public List<Table> table;
    public List<Transform> rutaEnter;
    public List<Transform> rutaExit;


    private void Awake()
    {
        instance = this;
    }
    public Table ObtenerMesaLibre()
    {
        List<Table> mesasLibres = new List<Table>();

        foreach (Table m in table)
        {
            if (!m.ocuppy)
            {
                mesasLibres.Add(m);
            }
        }

        if (mesasLibres.Count == 0)
            return null;

        int randomIndex = Random.Range(0, mesasLibres.Count);
        Table mesaElegida = mesasLibres[randomIndex];

        mesaElegida.Ocupping();
        return mesaElegida;


        //foreach (Table m in table)
        //{
        //    if (!m.ocuppy)
        //    {
        //        m.Ocupping();
        //        return m;
        //    }
        //}

        //return null;
    }
}
