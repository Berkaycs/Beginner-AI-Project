using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FollowWPNavmesh : MonoBehaviour
{
    public GameObject wpManager;
    private GameObject[] wps;
    private GameObject currentNode;
    public NavMeshAgent agent;
    public Button ruinBtn;
    public Button factoryBtn;
    public Button helicopterBtn;

    private void Start()
    {
        ButtonEvents();
    }

    private void LateUpdate()
    {
        
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

    private void GoToHelicopter()
    {
        agent.SetDestination(wps[0].transform.position);
    }

    private void GoToFactory()
    {
        agent.SetDestination(wps[7].transform.position);
    }

    private void GoToRuin()
    {
        agent.SetDestination(wps[8].transform.position);
    }
}
