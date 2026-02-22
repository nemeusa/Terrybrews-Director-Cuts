using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public List<CustomNodes> allNodes;

    public List<CustomNodes> GetAllNodes()
    {
        allNodes = new List<CustomNodes>(FindObjectsOfType<CustomNodes>());
        return allNodes;
        //return new List<Nodes>(FindObjectsOfType<Nodes>());

    }

    public List<CustomNodes> CalculateAStar(CustomNodes start, CustomNodes goal)
    {
        var frontier = new PriorityQueue<CustomNodes>();
        frontier.Enqueue(start, 0);

        var cameFrom = new Dictionary<CustomNodes, CustomNodes>();
        cameFrom.Add(start, null);
        var costSoFar = new Dictionary<CustomNodes, float>();
        costSoFar.Add(start, 0);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            //current.GetComponent<MeshRenderer>().material.color = Color.blue;
            if (current == goal)
            {
                List<CustomNodes> path = new List<CustomNodes>();

                while (current != null)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }

                path.Reverse();

                return path;
            }
            foreach (var node in current.Neighbours)
            {
                if (node.Block) continue;

                float newCost = costSoFar[current] + node.Cost;
                float priority = newCost + Vector2.Distance(node.transform.position, goal.transform.position);

                if (!cameFrom.ContainsKey(node))
                {
                    frontier.Enqueue(node, priority);
                    cameFrom.Add(node, current);
                    costSoFar.Add(node, newCost);
                }

                if (costSoFar.ContainsKey(node) && costSoFar[node] > newCost)
                {
                    frontier.Enqueue(node, priority);
                    cameFrom[node] = current;
                    costSoFar[node] = newCost;
                }
            }
        }

        return new List<CustomNodes>();
    }
}
