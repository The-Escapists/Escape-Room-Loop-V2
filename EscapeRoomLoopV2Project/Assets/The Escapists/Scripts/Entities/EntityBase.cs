using TheEscapists.ActionsAndInteractions.Interaction;
using TheEscapists.Core.Animation;
using TheEscapists.Core.Audio;
using UnityEngine;

namespace TheEscapists.Entities
{
    public class EntityBase : MonoBehaviour
    {
        [HideInInspector]
        public Interaction interactionContext;
        [HideInInspector]
        public bool isMovingObject;

        public LayerMask CollisionMask;

        private SpriteAnimator spriteAnimator;
        private AudioSource audioSource;
        private TriggeredRandomPitch triggeredRandomPitch;
        private const float movementDistance = 1f;

        private void Start()
        {
            spriteAnimator = this.GetComponent<SpriteAnimator>();
            audioSource = this.GetComponent<AudioSource>();
            triggeredRandomPitch = this.GetComponent<TriggeredRandomPitch>();
        }

        /// <summary>
        /// Move this object in the given direction if possible.
        /// </summary>
        /// <param name="direction">The normalized direction vector.</param>
        /// <returns>Was the movement action successful?</returns>
        public bool Move(Vector3 direction)
        {
            direction = direction.normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, movementDistance, CollisionMask);

            if (!hit.collider)
            {
                if(spriteAnimator) spriteAnimator.PlayAnimation(direction);
                if (audioSource) audioSource.Play();
                if (triggeredRandomPitch) triggeredRandomPitch.Trigger();

                this.transform.position += new Vector3(direction.x, direction.y, 0) * movementDistance;
            }

            return (!hit.collider);
        }
    }
}