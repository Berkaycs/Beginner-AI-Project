using UnityEngine;

public class GetRest : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        beliefs.RemoveState("exhausted");
        return true;
    }
}
