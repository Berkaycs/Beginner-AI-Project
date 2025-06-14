using UnityEngine;

public class Patient : GAgent
{
    new void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        goals.Add(s1, 3);

        SubGoal s2 = new SubGoal("isTreated", 1, true);
        goals.Add(s2, 5);

        SubGoal s3 = new SubGoal("isHome", 1, false);
        goals.Add(s3, 1);

        SubGoal s4 = new SubGoal("Relief", 1, false);
        goals.Add(s4, 2);

        Invoke(nameof(NeedRelief), Random.Range(2, 5));
    }

    private void NeedRelief()
    {
        beliefs.ModifyState("relief", 0);
        Invoke(nameof(NeedRelief), Random.Range(2, 5));
    }
}
