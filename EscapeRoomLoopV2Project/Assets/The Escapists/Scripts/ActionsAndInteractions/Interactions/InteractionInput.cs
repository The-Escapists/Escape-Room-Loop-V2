using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEscapists.ActionsAndInteractions.Interactions
{
    public class InteractionInput : Interaction
    {
        public void Execute()
        {
            TriggerInteraction(NotifyType.ActiveTrigger);
        }

        private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
        {
            Entities.EntityBase entityBase = collision.GetComponent<Entities.EntityBase>();
            if(entityBase)
            entityBase.interactionContext = this;
        }

        private void OnTriggerExit2D(UnityEngine.Collider2D collision)
        {
            Entities.EntityBase entityBase = collision.GetComponent<Entities.EntityBase>();
            if (entityBase)
                entityBase.interactionContext = null;
        }
    }
}
