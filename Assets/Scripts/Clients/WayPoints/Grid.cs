using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public List<CustomNodes> neighbours = new List<CustomNodes>();

    public CustomNodes prefab;
    CustomNodes[,] _grid;
    public int width, height;
    public float offset;

    private void Start()
    {
        _grid = new CustomNodes[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var node = Instantiate(prefab);
                node.transform.position = new Vector3(x, 0, y) * offset;
                node.Initialize(this, x, y);
                _grid[x, y] = node;
            }
        }
    }

    public void ResetNodes()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!_grid[x, y].Block)
                {
                    _grid[x, y].GetComponent<MeshRenderer>().material.color = Color.white;
                }
            }
        }
    }

    public CustomNodes GetNode(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
            return null;

        return _grid[x, y];
    }

}
