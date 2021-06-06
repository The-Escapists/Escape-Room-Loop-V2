using TheEscapists.Core.Manager;
using TheEscapists.Entities;
using TheEscapists.Entities.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace TheEscapists
{
    public class PlayerInput : MonoBehaviour
    {
        public UnityEvent<InputCommand> InputEvent = new UnityEvent<InputCommand>();
        
        [HideInInspector]
        public static PlayerInput Instance;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != null)
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                InputEvent.Invoke(InputCommand.UP);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                InputEvent.Invoke(InputCommand.LEFT);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                InputEvent.Invoke(InputCommand.DOWN);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                InputEvent.Invoke(InputCommand.RIGHT);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                InputEvent.Invoke(InputCommand.INTERACT);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                InputEvent.Invoke(InputCommand.SKIPSTEP);
            }
        }

        public enum InputCommand
        {
            UP, LEFT, DOWN, RIGHT,
            SKIPSTEP, INTERACT
        }
    }
}