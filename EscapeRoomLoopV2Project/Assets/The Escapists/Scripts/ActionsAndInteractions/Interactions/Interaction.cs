using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheEscapists.ActionsAndInteractions.Interactions
{
    public class Interaction : MonoBehaviour
    {
        public bool isInteractable = true;
        public string interactionName = "";
        
        public void Reset()
        {
            isInteractable = true;
        }

        public void TriggerInteraction(NotifyType notifyType)
        {
            if (!isInteractable)
                return;

            ActionsAndInteractionsManager.instance.Notify(interactionName, notifyType);
        }
    }
}
