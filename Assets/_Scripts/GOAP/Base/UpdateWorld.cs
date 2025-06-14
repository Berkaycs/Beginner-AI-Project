using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateWorld : MonoBehaviour
{
    public TMP_Text statesTxt;

    private void LateUpdate()
    {
        Dictionary<string, int> worldStates = GWorld.Instance.GetWorld().GetStates();

        statesTxt.text = "";

        foreach (KeyValuePair<string, int> state in worldStates)
        {
            statesTxt.text += state.Key + ", " + state.Value + "\n";
        }
    }
}
