using Unity.AI.Navigation;
using UnityEngine;

public class UIWorld : MonoBehaviour
{
    private ResourceData focusRD;

    private GameObject focusObject;
    public GameObject hospital;
    public GameObject newResourcePrefab;
    public NavMeshSurface surface;
    private Vector3 goalPos;

    private Vector3 clickOffset = Vector3.zero;
    private bool offsetCalc = false;

    private bool deleteResource = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit)) return;

            offsetCalc = false;
            clickOffset = Vector3.zero;

            Resource resource = hit.transform.gameObject.GetComponent<Resource>();

            if (resource != null)
            {
                focusObject = hit.transform.gameObject;
                focusRD = resource.data;
            }

            else
            {
                goalPos = hit.point;

                focusObject = Instantiate(newResourcePrefab, goalPos, Quaternion.identity);
            }

            focusObject.GetComponent<Collider>().enabled = false;
        }

        else if (Input.GetMouseButtonUp(0) && focusObject)
        {
            if (deleteResource)
            {
                GWorld.Instance.GetQueue(focusRD.resourceQueue).RemoveResource(focusObject);
                GWorld.Instance.GetWorld().ModifyState(focusRD.resourceState, -1);
                Destroy(focusObject);
            }
            else
            {
                focusObject.transform.parent = hospital.transform;
                GWorld.Instance.GetQueue(focusRD.resourceQueue).AddResource(focusObject);
                GWorld.Instance.GetWorld().ModifyState(focusRD.resourceState, 1);
                focusObject.GetComponent<Collider>().enabled = true;
            }

            surface.BuildNavMesh();
            focusObject = null;
        }

        else if (Input.GetMouseButton(0) && focusObject)
        {
            Ray rayMove = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitMove;

            if (!Physics.Raycast(rayMove, out hitMove)) return;

            if (!offsetCalc)
            {
                clickOffset = hitMove.point - focusObject.transform.position;
                offsetCalc = true;
            }

            goalPos = hitMove.point - clickOffset;

            focusObject.transform.position = goalPos;
        }

        if (focusObject && Input.GetKeyDown(KeyCode.E))
        {
            focusObject.transform.Rotate(0, 90, 0);
        }

        if (focusObject && Input.GetKeyDown(KeyCode.Q))
        {
            focusObject.transform.Rotate(0, -90, 0);
        }
    }

    public void MouseOnHoverTrash()
    {
        deleteResource = true;
    }

    public void MouseOutHoverTrash()
    {
        deleteResource = false;
    }
}
