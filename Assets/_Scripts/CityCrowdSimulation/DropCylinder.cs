using UnityEngine;

public class DropCylinder : MonoBehaviour
{
    public GameObject obstacle;
    private GameObject[] agents;

    private void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("agent");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction ,out hitInfo))
            {
                Instantiate(obstacle, hitInfo.point, obstacle.transform.rotation);

                foreach (GameObject a in agents)
                {
                    a.GetComponent<CrowdAIControl>().DetectNewObstacle(hitInfo.point);
                }
            }
        }
    }
}
