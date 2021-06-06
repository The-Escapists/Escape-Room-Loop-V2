using TheEscapists.Core.Manager;
using TheEscapists.Entities.Manager;
using UnityEngine;

namespace TheEscapists.Entities
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : EntityBase
    {
        private Vector3 spawn;

        private PlayerInput playerInput;

        void Start()
        {
            spawn = this.transform.position;
            playerInput = this.GetComponent<PlayerInput>();
            playerInput.InputEvent.AddListener(Interact);
        }

        public void ResetPlayer()
        {
            this.isMovingObject = false;
            this.interactionContext = null;
            this.transform.position = spawn;
        }

        public void Interact(PlayerInput.InputCommand command)
        {
            switch (command)
            {
                case PlayerInput.InputCommand.UP:
                    Move(new Vector3(0, 1));
                    break;
                case PlayerInput.InputCommand.LEFT:
                    Move(new Vector3(-1, 0));
                    break;
                case PlayerInput.InputCommand.DOWN:
                    Move(new Vector3(0, -1));
                    break;
                case PlayerInput.InputCommand.RIGHT:
                    Move(new Vector3(1, 0));
                    break;
                case PlayerInput.InputCommand.SKIPSTEP:
                    break;
                case PlayerInput.InputCommand.INTERACT:
                    break;
                default:
                    break;
            }
        }
    }
}