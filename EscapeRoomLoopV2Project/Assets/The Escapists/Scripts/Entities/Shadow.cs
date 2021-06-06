using System.Collections.Generic;
using TheEscapists.Entities.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace TheEscapists.Entities
{
    public class Shadow : EntityBase
    {
        public List<Vector3> Positions = new List<Vector3>();
        public List<bool> Interactions = new List<bool>();
        public Text ShadowCountText;
        public int CurrentPositionIndex = 0;
        public int CurrentInteractionIndex = 0;

        public void MoveToNextPosition()
        {
            if (CurrentPositionIndex >= Positions.Count)
            {
                return;
            }

            Vector3 direction;
            if (CurrentPositionIndex == 0)
            {
                direction = Positions[CurrentPositionIndex] - this.transform.position;
            }
            else
            {
                direction = Positions[CurrentPositionIndex] - Positions[CurrentPositionIndex - 1];
            }

            CurrentPositionIndex++;
            //Debug.Log(direction);

            Move(direction);
        }

        public void PerformInteraction()
        {
            if (CurrentInteractionIndex >= Interactions.Count)
            {
                return;
            }

            if (Interactions[CurrentInteractionIndex])
            {
                if (interactionContext)
                    interactionContext.Execute(transform, this);
            }
            CurrentInteractionIndex++;
        }

        public void ResetShadow(Vector3 resetPositioin)
        {
            this.isMovingObject = false;
            this.interactionContext = null;
            CurrentPositionIndex = 0;
            CurrentInteractionIndex = 0;
            this.transform.position = new Vector3(resetPositioin.x, resetPositioin.y, this.transform.position.z);
        }
    }
}