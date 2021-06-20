using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEscapists.Core.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace TheEscapists.ActionsAndInteractions
{
    [Flags]
    public enum NotifyType 
    {
        None            = 0,
        ActiveTrigger   = 1 << 0,
        TriggerEnter    = 1 << 1,
        TriggerExit     = 1 << 2,
        All             = ActiveTrigger|TriggerEnter|TriggerExit
    };
    class ActionsAndInteractionsManager : MonoBehaviour
    {
        public static ActionsAndInteractionsManager instance;

        public MapInteractionNodeGraph mapInteractionNodeGraph;
        //public Dictionary<string, Actions.Action> actions;
        public UnityEvent<Dictionary<string, bool>> actorsChanged;

        private void Awake()
        {
            if (!instance)
                instance = this;
            else
                Destroy(gameObject);
        }
        void Start()
        {
            ActionsManager.Instance.StepCounterDecreasedEvent.AddListener(RunGraph);
            ActionsManager.Instance.StepResetEvent.AddListener(ResetGraph);
        }

        public void Notify(string interactionName, NotifyType nofityType)
        {
            mapInteractionNodeGraph.Notify(interactionName, nofityType);
        }

        public void RunGraph()
        {
            actorsChanged.Invoke(mapInteractionNodeGraph.Run());
        }

        public void ResetGraph()
        {
            mapInteractionNodeGraph.Reset();
        }
    }
}
