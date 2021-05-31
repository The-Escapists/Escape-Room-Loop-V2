using TheEscapists.Core.Manager;
using TheEscapists.Entities;
using TheEscapists.Entities.Manager;
using UnityEngine;

namespace TheEscapists
{
    public class PlayerInput : MonoBehaviour
    {
        public MovementManager Movement;
        EntityBase eBase;

        [HideInInspector]
        public bool blockInput;

        void Start()
        {
            eBase = GetComponent<EntityBase>();
        }

        void Update()
        {
            if (blockInput)
                return;

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (Movement.Move(Vector2.up))
                    ActionsManager.Instance.DecreaseActions();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Movement.Move(Vector2.left))
                    ActionsManager.Instance.DecreaseActions();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (Movement.Move(Vector2.down))
                    ActionsManager.Instance.DecreaseActions();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (Movement.Move(Vector2.right))
                    ActionsManager.Instance.DecreaseActions();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && eBase.interactionContext != null && !eBase.isMovingObject)
            {
                if (ActionsManager.Instance.DecreaseActions())
                {
                    eBase.interactionContext.Execute(transform, eBase);
                    ShadowManager.Instance.SetLastInteractionTrue();
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) && eBase.interactionContext != null && eBase.interactionContext.parrented)
            {
                if (ActionsManager.Instance.DecreaseActions())
                {
                    eBase.interactionContext.Execute(transform, eBase);
                    ShadowManager.Instance.SetLastInteractionTrue();
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ActionsManager.Instance.DecreaseActions();
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ActionsManager.Instance.TimeReset();
            }
        }
    }
}