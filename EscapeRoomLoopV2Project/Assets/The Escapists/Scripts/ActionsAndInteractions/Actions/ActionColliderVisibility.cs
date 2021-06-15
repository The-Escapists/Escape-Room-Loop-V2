using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace TheEscapists.ActionsAndInteractions.Actions
{
    public class ActionColliderVisibility : Action
    {
        [SerializeField]
        Collider2D Collider;

        public bool condition;

        public override void CheckConditions()
        {

            //condition = ActionAndInteractionManager.instance.GetActorState(actionIndex);

            Collider.enabled = condition;
        }
    }
}