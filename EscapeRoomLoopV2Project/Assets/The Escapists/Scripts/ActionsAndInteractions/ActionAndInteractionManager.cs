using Bolt;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace TheEscapists.ActionsAndInteractions
{
    [RequireComponent(typeof(FlowMachine))]
    public class ActionAndInteractionManager : MonoBehaviour
    {
        public static ActionAndInteractionManager instance;
        [OnValueChanged("UpdateMacro")]
        public RoomDescriptionSO roomDescription;
        public FlowMachine flow;

        [HideInInspector]
        public UnityEvent updateActions;

        public void UpdateMacro()
        {
            if (roomDescription)
            {
                if (flow == null)
                    TryGetComponent<FlowMachine>(out flow);
                if (flow != null)
                {
                    flow.nest.SwitchToMacro(roomDescription.flowMacro);
                }
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetInteractionState(int index, bool state)
        {
            Bolt.Variables.Graph(flow.GetReference()).Set(roomDescription.interactorList[index].name, state);
            Bolt.CustomEvent.Trigger(instance.gameObject, "InteractionUpdate", null);
        }

        public bool GetActorState(int index)
        {
            return Bolt.Variables.Graph(flow.GetReference()).Get<bool>(roomDescription.actorList[index].name);
        }

        public int test1;
        public void UpdateActors()
        {
            updateActions.Invoke();
        }
    }
}