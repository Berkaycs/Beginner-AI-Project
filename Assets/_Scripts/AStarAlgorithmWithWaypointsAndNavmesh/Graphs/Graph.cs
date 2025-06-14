using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Graph
{
    private List<Edge> edgeList = new List<Edge>();
    private List<Node> nodesList = new List<Node>();
    public List<Node> pathList = new List<Node>();

    public Graph() { }

    public void AddNode(GameObject id)
    {
        Node node = new Node(id);
        nodesList.Add(node);
    }

    public void AddEdge(GameObject fromNode, GameObject toNode)
    {
        Node from = FindNode(fromNode);
        Node to = FindNode(toNode);

        if (from != null && to != null)
        {
            Edge edge = new Edge(from, to);
            edgeList.Add(edge);
            from.edgeList.Add(edge);
        }
    }

    private Node FindNode(GameObject id)
    {
        foreach (Node node in nodesList)
        {
            if (node.GetId() == id) return node;
        }
        return null;
    }

    public bool AStar(GameObject startId, GameObject endId)
    {
        if (startId == endId)
        {
            pathList.Clear();
            return false;
        }

        Node start = FindNode(startId);
        Node end = FindNode(endId);

        if (start == null || end == null)
        {
            return false;
        }

        List<Node> open = new List<Node>();
        List<Node> close = new List<Node>();

        float tentative_g_score = 0;
        bool tentative_is_better;

        start.G = 0;
        start.H = Distance(start, end);
        start.F = start.H;

        open.Add(start);

        while (open.Count > 0) 
        {
            int i = LowestF(open);
            Node thisNode = open[i];
            if (thisNode.GetId() == endId)
            {
                ReconstructPath(start, end);
                return true;
            }

            open.RemoveAt(i);
            close.Add(thisNode);

            Node neighbour;

            foreach (Edge edge in thisNode.edgeList)
            {
                neighbour = edge.endNode;

                if (close.IndexOf(neighbour) > -1)
                    continue;

                tentative_g_score = thisNode.G + Distance(thisNode, neighbour);

                if (open.IndexOf(neighbour) == -1)
                {
                    open.Add(neighbour);
                    tentative_is_better = true;
                }
                else if (tentative_g_score < neighbour.G)
                {
                    tentative_is_better = true;
                }
                else
                    tentative_is_better = false;

                if (tentative_is_better)
                {
                    neighbour.cameFrom = thisNode;
                    neighbour.G = tentative_g_score;
                    neighbour.H = Distance(thisNode, end);
                    neighbour.F = neighbour.G + neighbour.H;
                }
            }
        }
        return false;
    }

    public void ReconstructPath(Node startId, Node endId)
    {
        pathList.Clear();
        pathList.Add(endId);

        Node camePoint = endId.cameFrom;

        while (camePoint != startId && camePoint != null)
        {
            pathList.Insert(0, camePoint);
            camePoint = camePoint.cameFrom;
        }
        pathList.Insert(0, startId);
    }

    private float Distance(Node a, Node b)
    {
        // sqr magnitude more fast to calculate than magnitude
        return (Vector3.SqrMagnitude(a.GetId().transform.position - b.GetId().transform.position));
    }

    private int LowestF(List<Node> l)
    {
        float lowestF = 0f;
        int count = 0;
        int iteratorCount = 0;

        lowestF = l[0].F;

        for (int i = 1; i < l.Count; i++)
        {
            if (l[i].F < lowestF)
            {
                lowestF = l[i].F;
                iteratorCount = count;
            }

            count++;
        }

        return iteratorCount;
    }
}
