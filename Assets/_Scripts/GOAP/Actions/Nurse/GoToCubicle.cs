using UnityEngine;

public class GoToCubicle : GAction
{
    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag("cubicle");

        if (target == null) return false;

        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("TreatingPatient", 1);

        GWorld.Instance.GetQueue("cubicle").AddResource(target);

        inventory.RemoveItem(target);

        GWorld.Instance.GetWorld().ModifyState("FreeCubicle", 1);

        // Set rest goal state to false so nurse will rest next
        GWorld.Instance.GetWorld().ModifyState("Rested", 0);

        return true;
    }
}
