using UnityEngine;

namespace TheEscapists.ActionsAndInteractions.Actions
{
    public class ActionDebug : Action
    {
        public string DebugMessage = "None";

        public override void TriggerEnable()
        {
                string message = DebugMessage;
                message = message.Replace("$name", actionName.ToString());
                Debug.Log("Debug: " + message);
        }

        public override void TriggerDisable()
        {

        }
    }
}