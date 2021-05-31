using TheEscapists.Core.Animation;
using TheEscapists.Core.Audio;
using UnityEngine;

namespace TheEscapists.Entities.Manager
{
    public class MovementManager : MonoBehaviour
    {
        public float MoveDistance = 1f;
        public LayerMask layerMask;
        public SpriteAnimator spriteAnimator;
        public AudioSource audioSource;
        public TriggeredRandomPitch triggeredRandomPitch;
        void Start()
        {

        }

        void Update()
        {

        }

        /// <summary>
        /// Move this object in the given direction if possible.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>Was the movement action successful?</returns>
        public bool Move(Vector3 direction)
        {
            direction = direction.normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, MoveDistance, layerMask);

            if (hit.collider)
            {
                Debug.DrawRay(transform.position - (Vector3.back * 3), direction * hit.distance, Color.green, 1f);
                Debug.Log("MovementManager -> Object hit: " + hit.collider.gameObject.name);
                return false;
            }
            else
            {
                if(spriteAnimator) spriteAnimator.PlayAnimation(direction);
                if(audioSource) audioSource.Play();
                if(triggeredRandomPitch) triggeredRandomPitch.Trigger();
                Vector3 newPosition = this.transform.position + new Vector3(direction.x, direction.y, 0) * MoveDistance;
                this.transform.position = newPosition;
                return true;
            }
        }

        /// <summary>
        /// Move this object to the target.
        /// </summary>
        /// <param name="target"></param>
        public void MoveTo(Vector3 target)
        {
            this.transform.position = new Vector3(target.x, target.y, this.transform.position.z);
        }
    }
}