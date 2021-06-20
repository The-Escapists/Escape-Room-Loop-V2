using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace TheEscapists.ActionsAndInteractions.Actions
{
    public class ActionColliderVisibility : Action
    {
        [SerializeField]
        Collider2D Collider;

        public override void TriggerEnable()
        {

            Collider.enabled = true;
        }
        public override void TriggerDisable()
        {

            Collider.enabled = false;
        }
    }
}