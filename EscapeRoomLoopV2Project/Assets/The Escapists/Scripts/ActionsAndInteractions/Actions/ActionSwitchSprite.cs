using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace TheEscapists.ActionsAndInteractions.Actions
{
    public class ActionSwitchSprite : Action
    {
        public SpriteCollection spriteCollection;

        public bool condition;

        public override void CheckConditions()
        {

            //condition = ActionAndInteractionManager.instance.GetActorState(actionIndex);

            spriteCollection.spriteStateRenderer.sprite = condition ? spriteCollection.spriteStateOpen : spriteCollection.spriteStateClosed;
        }
    }

    [Serializable]
    public class SpriteCollection
    {
        [PreviewField(50, ObjectFieldAlignment.Center)]
        public SpriteRenderer spriteStateRenderer;
        [PreviewField(50, ObjectFieldAlignment.Center)]
        public Sprite spriteStateClosed;
        [PreviewField(50, ObjectFieldAlignment.Center)]
        public Sprite spriteStateOpen;
    }
}