using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEscapists.ActionsAndInteractions.Interactions
{
    public class InteractionTrigger : Interaction
    {

        private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
        {
            TriggerInteraction(NotifyType.TriggerEnter);
        }

        private void OnTriggerExit2D(UnityEngine.Collider2D collision)
        {
            TriggerInteraction(NotifyType.TriggerExit); 
        }

    }
}
