using UnityEngine;

namespace TheEscapists.ActionsAndInteractions.Actions
{
    public class ActionDebug : Action
    {
        public string DebugMessage = "None";
        public bool condition;

        public override void CheckConditions()
        {
            condition = ActionAndInteractionManager.instance.GetActorState(actionIndex);


            if (condition)
            {
                string message = DebugMessage;
                message = message.Replace("$index", actionIndex.ToString());
                Debug.Log("Debug: " + message);
            }
        }
    }
}