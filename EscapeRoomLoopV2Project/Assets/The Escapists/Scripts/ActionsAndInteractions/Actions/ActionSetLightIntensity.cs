using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace TheEscapists.ActionsAndInteractions.Actions
{
    public class ActionSetLightIntensity : Action
    {
        public Light2D light2D;
        public float onValue;
        public float offValue;

        [PropertySpace(20)]

        public bool condition;

        public override void CheckConditions()
        {

            //condition = ActionAndInteractionManager.instance.GetActorState(actionIndex);

            light2D.intensity = condition ? onValue : offValue;
        }
    }
}