using UnityEngine;

public class DestroyShell : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 3);
    }
}
