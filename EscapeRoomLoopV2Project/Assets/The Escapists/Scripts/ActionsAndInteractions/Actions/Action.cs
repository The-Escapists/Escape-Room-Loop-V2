
using System.Collections.Generic;
using UnityEngine;

namespace TheEscapists.ActionsAndInteractions.Actions
{
    public abstract class Action : MonoBehaviour
    {
        public string actionName;
        bool state;

        private void Start()
        {
            ActionsAndInteractionsManager.instance.actorsChanged.AddListener(ActorChanged);
        }

        public void ActorChanged(Dictionary<string, bool> actorsStates)
        {
            if(actorsStates.ContainsKey(actionName))
            {
                if (state != actorsStates[actionName])
                {
                    state = actorsStates[actionName];
                    
                    if (state)
                    {
                        TriggerEnable();
                    }
                    else
                    {
                        TriggerDisable();
                    }
                }
            }
        }

        public abstract void TriggerEnable();
        public abstract void TriggerDisable();
    }
}
