using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Edge> edgeList = new List<Edge>();

    public Node path = null;
    public Node cameFrom;

    public float xPos;
    public float yPos;
    public float zPos;

    public float G; // cost from start
    public float H; // cost to goal
    public float F; // total cost

    private GameObject id;


    public Node(GameObject i)
    {
        id = i;
        path = null;
    }

    public GameObject GetId()
    {
        return id;
    }
}
