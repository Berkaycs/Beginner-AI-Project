using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathMarker
{
    public MapLocation location;
    public float G;
    public float H;
    public float F;
    public GameObject marker;
    public PathMarker parent;

    public PathMarker(MapLocation location, float g, float h, float f, GameObject marker, PathMarker parent)
    {
        this.location = location;
        G = g; // cost from start
        H = h; // cost to goal 
        F = f; // total cost  ( g + h )
        this.marker = marker;
        this.parent = parent;
    }

    public override bool Equals(object marker)
    {
        if ((marker == null) || !this.GetType().Equals(marker.GetType()))
            return false;
        else
            return location.Equals(((PathMarker) marker).location);
    }

    public override int GetHashCode()
    {
        return 0;
    }
}

public class FindPathAStar : MonoBehaviour
{
    public Maze maze;

    public GameObject startPoint;
    public GameObject endPoint;
    public GameObject pathPoint;

    public Material openedMaterial;
    public Material closedMaterial;

    private PathMarker startNode;
    private PathMarker goalNode;
    private PathMarker lastPos;

    private bool isFinished = false;
    private bool hasStarted = false;

    private List<PathMarker> openMarkers = new List<PathMarker>();
    private List<PathMarker> closedMarkers = new List<PathMarker>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) BeginSearch();
        if (Input.GetKeyDown(KeyCode.C) && !isFinished) Search(lastPos);
        if (Input.GetKeyDown(KeyCode.M)) GetPath();
    }

    private void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
        foreach (GameObject m in markers)
        {
            Destroy(m);
        }
    }

    private void BeginSearch()
    {
        isFinished = false;
        RemoveAllMarkers();

        List<MapLocation> locations = new List<MapLocation>();

        for (int z = 1; z < maze.depth - 1; ++z)
        {
            for (int x = 1; x < maze.width - 1; ++x)
            {
                if (maze.map[x, z] != 1) // in 1 there are walls.
                {
                    locations.Add(new MapLocation(x, z));
                }
            }
        }

        locations.Shuffle();

        Vector3 startLocation = new Vector3(locations[0].x * maze.scale, 0, locations[0].z * maze.scale);
        startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z), 0, 0, 0,
                                   Instantiate(startPoint, startLocation, Quaternion.identity), null);


        Vector3 goalLocation = new Vector3(locations[1].x * maze.scale, 0, locations[1].z * maze.scale);
        goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z), 0, 0, 0,
                                   Instantiate(endPoint, goalLocation, Quaternion.identity), null);

        openMarkers.Clear();
        closedMarkers.Clear();

        openMarkers.Add(startNode);
        lastPos = startNode;
    }

    private void Search(PathMarker thisNode)
    {
        if (thisNode == null) return;

        if(thisNode.Equals(goalNode))
        {
            isFinished = true;
            return;
        }

        foreach (MapLocation dir in maze.directions)
        {
            MapLocation neighbour = dir + thisNode.location;

            // it's wall
            if (maze.map[neighbour.x, neighbour.z] == 1) continue; 

            // it's outside of the area
            if (neighbour.x < 1 || neighbour.x >= maze.width || neighbour.z < 1 || neighbour.z >= maze.depth) continue;

            // it's closed
            if (IsClosed(neighbour)) continue;

            float G = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G; // cost from start
            float H = Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector()); // cost to goal
            float F = G + H; // total cost

            GameObject pathBlock = Instantiate(pathPoint, new Vector3(neighbour.x * maze.scale, 0, neighbour.z * maze.scale),
                                               Quaternion.identity);

            TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();
            values[0].text = "G: " + G.ToString("0.00");
            values[1].text = "H: " + H.ToString("0.00");
            values[2].text = "F: " + F.ToString("0.00");

            if (!UpdateMarker(neighbour,G, H, F, thisNode))
            {
                openMarkers.Add(new PathMarker(neighbour, G, H, F, pathBlock, thisNode));
            }
        }


        // linq usage for ranking the open markes list based on the F value, if it's the same then ranking by H value
        openMarkers = openMarkers.OrderBy(p => p.F).ThenBy(n => n.H).ToList<PathMarker>();
        PathMarker pm = openMarkers.ElementAt(0);
        closedMarkers.Add(pm);

        openMarkers.RemoveAt(0);
        pm.marker.GetComponent<Renderer>().material = closedMaterial;

        lastPos = pm;
    }

    private void GetPath()
    {
        RemoveAllMarkers();
        PathMarker begin = lastPos;

        while (!startNode.Equals(begin) && begin != null)
        {
            Instantiate(pathPoint, new Vector3(begin.location.x * maze.scale, 0, begin.location.z * maze.scale), 
                        Quaternion.identity);

            begin = begin.parent;
        }

        Instantiate(pathPoint, new Vector3(startNode.location.x * maze.scale, 0, startNode.location.z * maze.scale),
                    Quaternion.identity);
    }

    private bool UpdateMarker(MapLocation location, float g, float h,  float f, PathMarker parent)
    {
        foreach (PathMarker marker in openMarkers)
        {
            if (marker.location.Equals(location))
            {
                marker.G = g;
                marker.H = h;
                marker.F = f;
                marker.parent = parent;
                return true;
            }
        }
        return false;
    }

    private bool IsClosed(MapLocation marker)
    {
        foreach (PathMarker path in closedMarkers)
        {
            if (path.location.Equals(marker)) return true;
        }

        return false;
    }
}   
