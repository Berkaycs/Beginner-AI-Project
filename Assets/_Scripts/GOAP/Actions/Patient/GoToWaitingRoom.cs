public class GoToWaitingRoom : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("patientWaiting", 1);
        GWorld.Instance.GetQueue("patient").AddResource(gameObject);
        beliefs.ModifyState("atHospital", 1);
        return true;
    }
}
