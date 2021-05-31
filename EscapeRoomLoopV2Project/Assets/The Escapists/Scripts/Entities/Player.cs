using TheEscapists.Core.Manager;
using TheEscapists.Entities.Manager;
using UnityEngine;

namespace TheEscapists.Entities
{
    public class Player : EntityBase
    {
        public Transform Spawn;

        void Start()
        {
            SetSpawn();
            ActionsManager.Instance.StepResetEvent.AddListener(ResetPlayerToCurrentSpawn);
        }

        public void SetNewSpawn(Transform newSpawn)
        {
            Spawn = newSpawn;
        }

        public void ResetPlayerToCurrentSpawn()
        {
            this.isMovingObject = false;
            this.interactionContext = null;
            SetSpawn();
            ShadowManager.Instance.CreateNewShadow(Spawn.position);
        }

        private void SetSpawn()
        {
            if (!Spawn)
            {
                Debug.LogError("Player -> Spawn missing!");
                return;
            }
            this.transform.position = new Vector3(Spawn.position.x, Spawn.position.y, this.transform.position.z);
        }
    }
}