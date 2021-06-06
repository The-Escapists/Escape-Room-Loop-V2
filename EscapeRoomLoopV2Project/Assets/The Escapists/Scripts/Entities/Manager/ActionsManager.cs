using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TheEscapists.Core.Manager
{
    public class ActionsManager : MonoBehaviour
    {
        public static ActionsManager Instance;
        public Text StepCounterText;
        public Text TurnCounterText;
        public int StartActionsCount = 12;
        public float ResetDelay = 1f;
        [HideInInspector]
        public int CurrentActionsCount = 0;
        [HideInInspector]
        public int CurrentTurnCount = 1;

        public UnityEvent StepResetEvent = new UnityEvent();
        public UnityEvent StepCounterDecreasedEvent = new UnityEvent();

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else if (Instance != null)
            {
                Destroy(gameObject);
            }

            CurrentActionsCount = StartActionsCount;
            StepCounterText.text = CurrentActionsCount.ToString();
            TurnCounterText.text = CurrentTurnCount.ToString();
        }

        public bool DecreaseActions()
        {
            if (CurrentActionsCount == 0)
            {
                return false;
            }

            CurrentActionsCount--;
            StepCounterText.text = CurrentActionsCount.ToString();

            if (CurrentActionsCount == 0)
            {
                StartCoroutine(DelayTimeReset());
            }

            StepCounterDecreasedEvent.Invoke();

            return true;
        }

        public void TimeReset()
        {
            CurrentActionsCount = StartActionsCount;
            StepCounterText.text = CurrentActionsCount.ToString();
            StepResetEvent.Invoke();
            //playerInput.blockInput = false;
        }

        private IEnumerator DelayTimeReset()
        {
            //playerInput.blockInput = true;
            yield return new WaitForSeconds(ResetDelay);
            CurrentTurnCount++;
            TurnCounterText.text = CurrentTurnCount.ToString();
            TimeReset();
        }
    }
}