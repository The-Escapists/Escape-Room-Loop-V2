using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace TheEscapists.ActionsAndInteractions.Actions
{
    public class ActionSwitchSprite : Action
    {
        public SpriteCollection spriteCollection;

        public override void TriggerDisable()
        {
            spriteCollection.spriteStateRenderer.sprite = spriteCollection.spriteStateOff;
        }

        public override void TriggerEnable()
        {
            spriteCollection.spriteStateRenderer.sprite = spriteCollection.spriteStateOn;
        }
    }

    [Serializable]
    public class SpriteCollection
    {
        [PreviewField(50, ObjectFieldAlignment.Center)]
        public SpriteRenderer spriteStateRenderer;
        [PreviewField(50, ObjectFieldAlignment.Center)]
        public Sprite spriteStateOff;
        [PreviewField(50, ObjectFieldAlignment.Center)]
        public Sprite spriteStateOn;
    }
}