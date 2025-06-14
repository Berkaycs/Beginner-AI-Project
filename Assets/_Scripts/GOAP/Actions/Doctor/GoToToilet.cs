using UnityEngine;

public class GoToToilet : GAction
{
    public override bool PrePerform()
    {
        target = GWorld.Instance.GetQueue("toilet").RemoveResource();

        if (target == null) return false;

        inventory.AddItem(target);
        GWorld.Instance.GetWorld().ModifyState("FreeToilet", -1);
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetQueue("toilet").AddResource(target);
        inventory.RemoveItem(target);
        GWorld.Instance.GetWorld().ModifyState("FreeToilet", 1);
        beliefs.RemoveState("relief");
        return true;
    }
}
