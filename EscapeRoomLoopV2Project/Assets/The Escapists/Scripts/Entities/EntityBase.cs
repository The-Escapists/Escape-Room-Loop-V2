using TheEscapists.ActionsAndInteractions.Interaction;
using UnityEngine;

namespace TheEscapists.Entities
{
    public class EntityBase : MonoBehaviour
    {
        [HideInInspector]
        public Interaction interactionContext;
        [HideInInspector]
        public bool isMovingObject;
    }
}