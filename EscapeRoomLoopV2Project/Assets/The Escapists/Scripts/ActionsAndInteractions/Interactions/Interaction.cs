using Sirenix.OdinInspector;
using System.Collections.Generic;
using TheEscapists.Core;
using TheEscapists.Core.Generated;
using TheEscapists.Core.Manager;
using TheEscapists.Entities;
using UnityEngine;

namespace TheEscapists.ActionsAndInteractions.Interaction
{
    public class Interaction : MonoBehaviour
    {
        //Enum to check the interaction Type
        [BoxGroup("Interaction")]
        public enum InteractionContexts { Passive, Active };
        [EnumToggleButtons, HideLabel]
        public InteractionContexts interactionContext;
        [BoxGroup("Interaction")]
        public enum Interactions {Move, Switch, Trigger};
        [EnumToggleButtons, HideLabel, PropertySpace(SpaceAfter = 5)]
        public Interactions interaction;

        [FoldoutGroup("Interaction Settings")]
        //Settings for the Interaction
        [BoxGroup("Interaction Settings/Tag Filter")]
        public List<UnityTags.eUnityTags> unityTagFilter;
        private List<string> unityTagsStrings;
        [BoxGroup("Interaction Settings/Global/Tags"), Button("Update Tag Cache"), PropertySpace(SpaceAfter = 5)]
        public void updateTags()
        {
            TagsCreator.UpdateTagCacheUnity();
        }
        [BoxGroup("Interaction Settings/Global"), PropertySpace(SpaceBefore = 5)]
        public int executers = 0;
        [BoxGroup("Interaction Settings/Global")]
        public bool isInteractable = true;
        [BoxGroup("Interaction Settings/Global"), PropertySpace(SpaceAfter = 5)]
        public int interactionIndex;
        [ShowIf("@interaction == Interactions.Switch"), BoxGroup("Interaction Settings/Switch")]
        public bool switchState;
        [ShowIf("@interaction == Interactions.Move"), BoxGroup("Interaction Settings/Move")]
        public bool parrented;
        [ShowIf("@interaction == Interactions.Move"), BoxGroup("Interaction Settings/Move")]
        public Transform originalParent;
        [ShowIf("@interaction == Interactions.Move"), BoxGroup("Interaction Settings/Move")]
        Vector3 rootPosition;

        private void Start()
        {
            unityTagsStrings = new List<string>();
            foreach (UnityTags.eUnityTags tag in unityTagFilter)
            {
                unityTagsStrings.Add(tag.ToString());
            }

            rootPosition = transform.position;
            originalParent = transform.parent;
            //ActionsManager.Instance.StepResetEvent.AddListener(Reset);
        }

        public void Reset()
        {
            transform.parent = originalParent;
            transform.position = rootPosition;
            executers = 0;
            isInteractable = true;
            ActionAndInteractionManager.instance.SetInteractionState(interactionIndex, false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (unityTagsStrings.Contains(collision.tag.ToString()))
            {
                EntityBase eBase = collision.GetComponent<EntityBase>();
                if (eBase)
                {
                    if (!eBase.isMovingObject && isInteractable)
                        eBase.interactionContext = this;
                    else
                        isInteractable = false;
                }

                executers++;

                if (interactionContext == InteractionContexts.Passive)
                {


                    if (interaction == Interactions.Switch && executers == 1)
                    {
                        switchState = true;
                        ActionAndInteractionManager.instance.SetInteractionState(interactionIndex, switchState);
                    }

                    if (interaction == Interactions.Trigger && executers == 1)
                    {
                        ActionAndInteractionManager.instance.SetInteractionState(interactionIndex, true);
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (unityTagsStrings.Contains(collision.tag.ToString()))
            {
                if (!parrented)
                {
                    EntityBase eBase = collision.GetComponent<EntityBase>();
                    if (!eBase.isMovingObject && isInteractable)
                    {
                        eBase.interactionContext = null;
                    }
                    else if (eBase.isMovingObject && !isInteractable)
                    {
                        isInteractable = true;
                    }
                }

                executers--;

                if (interactionContext == InteractionContexts.Passive)
                {

                    if (interaction == Interactions.Switch && executers == 0)
                    {
                        switchState = false;
                        ActionAndInteractionManager.instance.SetInteractionState(interactionIndex, switchState);
                    }
                }

                if (interaction == Interactions.Trigger && executers == 0)
                {
                    switchState = false;
                    ActionAndInteractionManager.instance.SetInteractionState(interactionIndex, false);
                }
            }
        }
        /// <summary>
        /// Executes interactions if neccessary and returns the current one
        /// </summary>
        /// <param name="executer"></param>
        /// <returns>the Current Interaction</returns>
        ///
        [Button("Exetute Test")]
        public void Execute(Transform executer, EntityBase eBase)
        {
            if (interactionContext == InteractionContexts.Active)
            {
                switch (interaction)
                {
                    case Interactions.Move:
                        if (parrented)
                        {
                            eBase.isMovingObject = false;
                            parrented = false;
                            transform.parent = originalParent;
                        }
                        else
                        {
                            eBase.isMovingObject = true;
                            parrented = true;
                            transform.parent = executer;
                        }
                        break;
                    case Interactions.Switch:
                        switchState = !switchState;
                        ActionAndInteractionManager.instance.SetInteractionState(interactionIndex, switchState);
                        break;
                    case Interactions.Trigger:
                        ActionAndInteractionManager.instance.SetInteractionState(interactionIndex, true);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}