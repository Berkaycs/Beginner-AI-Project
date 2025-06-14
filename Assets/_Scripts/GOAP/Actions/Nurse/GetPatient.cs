using UnityEngine;

public class GetPatient : GAction
{
    private GameObject resource;

    public override bool PrePerform()
    {
        target = GWorld.Instance.GetQueue("patient").RemoveResource();

        if (target == null) return false;

        resource = GWorld.Instance.GetQueue("cubicle").RemoveResource();

        if (resource != null)
        {
            // nurse has that cubicle
            inventory.AddItem(resource);
        }
        else
        {
            GWorld.Instance.GetQueue("patient").AddResource(target);
            target = null;
            return false;
        }

        GWorld.Instance.GetWorld().ModifyState("FreeCubicle", -1);
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("patientWaiting", -1);

        if (target != null)
        {
            // patient has that cubicle
            target.GetComponent<GAgent>().inventory.AddItem(resource);
        }

        return true;
    }
}
