using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFollowClientsManager : MonoBehaviour
{
    public static SpawnFollowClientsManager instance;
    public PathManager pathManager;
    public List<CustomNodes> allChairs;

    private void Awake()
    {
        instance = this;
    }
}
