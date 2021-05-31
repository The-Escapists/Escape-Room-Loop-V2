using System.Collections.Generic;
using TheEscapists.Core.Manager;
using UnityEngine;

namespace TheEscapists.Entities.Manager
{
    public class ShadowManager : MonoBehaviour
    {
        public static ShadowManager Instance;
        public Shadow ShadowPrefab;
        public List<Shadow> Shadows = new List<Shadow>();
        public List<Vector3> newPositions = new List<Vector3>();
        public List<bool> newInteractions = new List<bool>();
        private Player player;

        private void Awake()
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
        }

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            ActionsManager.Instance.StepCounterDecreasedEvent.AddListener(AddNewPosition);
            ActionsManager.Instance.StepCounterDecreasedEvent.AddListener(MoveShadows);
        }

        public void AddNewPosition()
        {
            newPositions.Add(player.transform.position);
            AddInteraction();
        }

        public void AddInteraction()
        {
            newInteractions.Add(false);
        }

        public void SetLastInteractionTrue()
        {
            if (newInteractions.Count > 0)
            {
                newInteractions[newInteractions.Count - 1] = true;
            }
        }

        public void MoveShadows()
        {
            foreach (Shadow shadow in Shadows)
            {
                shadow.MoveToNextPosition();
                shadow.PerformInteraction();
            }
        }


        public void CreateNewShadow(Vector3 initialPosition)
        {
            Shadows.Add(Instantiate(ShadowPrefab, initialPosition, new Quaternion()));
            Shadows[Shadows.Count - 1].Positions.AddRange(newPositions);
            Shadows[Shadows.Count - 1].Interactions.AddRange(newInteractions);
            Shadows[Shadows.Count - 1].ShadowCountText.text = Shadows.Count.ToString();
            //ResetShadows();
            for (int i = 0; i < Shadows.Count; i++)
            {
                Shadows[i].ResetShadow(initialPosition);
            }
            newPositions = new List<Vector3>();
            newInteractions = new List<bool>();
        }

        private void ClearShadows()
        {
            Shadows = new List<Shadow>();
            newPositions = new List<Vector3>();
            newInteractions = new List<bool>();
        }
    }
}