using UnityEngine;

public class Janitor : GAgent
{
    private void Start()
    {
        base.Start();

        SubGoal s1 = new SubGoal("Clean", 1, false);
        goals.Add(s1, 1);
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
