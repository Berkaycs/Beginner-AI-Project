using UnityEngine;
using UnityEngine.AI;

public class AgentManager : MonoBehaviour
{
    public NavMeshAgent[] agents;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                foreach (var agent in agents)
                {
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}
