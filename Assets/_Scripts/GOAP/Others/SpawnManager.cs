using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject patientPrefab;
    public int numPatients;

    private void Start()
    {
        for (int i = 0; i < numPatients; i++)
        {
                       
        }

        Invoke(nameof(SpawnPatient), 5);
    }

    private void SpawnPatient()
    {
        Instantiate(patientPrefab, transform.position, Quaternion.identity);
        Invoke(nameof(SpawnPatient), Random.Range(2, 10));
    }
}
