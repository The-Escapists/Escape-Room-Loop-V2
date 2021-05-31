
using UnityEngine;

namespace TheEscapists.ActionsAndInteractions.Actions
{
    public abstract class Action : MonoBehaviour
    {
        public int actionIndex = 0;
        private void Start()
        {
            ActionAndInteractionManager.instance.updateActions.AddListener(CheckConditions);
        }
        public abstract void CheckConditions();
    }
}
