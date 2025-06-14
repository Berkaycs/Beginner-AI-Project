using UnityEngine;
using UnityEngine.UI;

public class FollowWP : MonoBehaviour
{
    public GameObject wpManager;

    public Button ruinBtn;
    public Button helicopterBtn;
    public Button factoryBtn;

    private GameObject[] wps;
    private GameObject currentNode;

    private Graph graph;

    private Transform goal;

    private int currentWP = 0;

    private float speed = 5f;
    private float accuracy = 5f;
    private float rotSpeed = 2f;

    private void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        graph = wpManager.GetComponent<WPManager>().graph;
        currentNode = wps[0];

        ButtonEvents();

        Invoke(nameof(GoToRuin), 2);
    }

    private void LateUpdate()
    {
        if (graph.pathList.Count == 0 || currentWP == graph.pathList.Count) return;

        if (Vector3.Distance(graph.pathList[currentWP].GetId().transform.position, 
                            transform.position) < accuracy)
        {
            currentNode = graph.pathList[currentWP].GetId();
            currentWP++;
        }

        if (currentWP < graph.pathList.Count)
        {
            goal = graph.pathList[currentWP].GetId().transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x, transform.position.y, goal.position.z);

            Vector3 direction = lookAtGoal - transform.position;

            transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                  Quaternion.LookRotation(direction),
                                                  rotSpeed * Time.deltaTime);

            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }

    private void ButtonEvents()
    {
        ruinBtn.onClick.RemoveAllListeners();
        ruinBtn.onClick.AddListener(() =>
        {
            GoToRuin();
        });

        helicopterBtn.onClick.RemoveAllListeners();
        helicopterBtn.onClick.AddListener(() =>
        {
            GoToHelicopter();
        });

        factoryBtn.onClick.RemoveAllListeners();
        factoryBtn.onClick.AddListener(() =>
        {
            GoToFactory();
        });
    }

    public void GoToHelicopter()
    {
        graph.AStar(currentNode, wps[0]);
        currentWP = 0;
    }

    public void GoToRuin()
    {
        graph.AStar(currentNode, wps[8]);
        currentWP = 0;
    }

    public void GoToFactory()
    {
        graph.AStar(currentNode, wps[7]);
        currentWP = 0;
    }
}
