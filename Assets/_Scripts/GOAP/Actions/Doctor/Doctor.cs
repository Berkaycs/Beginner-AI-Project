using UnityEngine;

public class Doctor : GAgent
{
    private void Start()
    {
        base.Start();

        SubGoal s1 = new SubGoal("Research", 1, false);
        goals.Add(s1, 1);

        SubGoal s2 = new SubGoal("Relief", 1, false);
        goals.Add(s2, 2);

        SubGoal s3 = new SubGoal("GetRest", 1, false);
        goals.Add(s3, 3);

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
