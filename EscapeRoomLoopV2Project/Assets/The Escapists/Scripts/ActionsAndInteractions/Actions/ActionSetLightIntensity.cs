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

        public override void TriggerDisable()
        {
            light2D.intensity = offValue;
        }

        public override void TriggerEnable()
        {
            light2D.intensity = onValue;
        }
    }
}