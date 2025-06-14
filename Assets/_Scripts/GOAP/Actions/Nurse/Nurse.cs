using UnityEngine;

public class Nurse : GAgent
{
    private void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("treatPatient", 1, false);
        goals.Add(s1, 3);

        SubGoal s2 = new SubGoal("getRest", 1, false);
        goals.Add(s2, 1);

        SubGoal s3 = new SubGoal("Relief", 1, false);
        goals.Add(s3, 2);

        Invoke(nameof(GetTired), Random.Range(2, 5));
        Invoke(nameof(NeedRelief), Random.Range(2, 5));
    }

    private void GetTired()
    {
        beliefs.ModifyState("exhausted", 0);
        Invoke(nameof(GetTired), Random.Range(2, 5));
    }

    private void NeedRelief()
    {
        beliefs.ModifyState("relief", 0);
        Invoke(nameof(NeedRelief), Random.Range(2, 5));
    }
}
