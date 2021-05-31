using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TheEscapists.Core.Audio;
using UnityEngine;

namespace TheEscapists.Core.Animation
{
    public class SpriteAnimator : MonoBehaviour
    {
        public enum AnimationStates { Idle, WalkUp, WalkDown, WalkLeft, WalkRight, Done }
        SpriteRenderer sRenderer;
        //[TableList]
        public List<SpriteAnimationCollection> animationPerStyle;
        int Style = 0;
        public float ResetDelay = 1f;

        private void Start()
        {
            sRenderer = GetComponent<SpriteRenderer>();
            //ActionsManager.Instance.StepResetEvent.AddListener(PlayDoneAnimation);
        }

        public void setStyle(int styleIndex)
        {
            Style = styleIndex;
        }

        public void PlayAnimation(Vector2 direction)
        {
            if (direction.y > 0)
            {
                PlayAnimation(AnimationStates.WalkUp);
            }
            else if (direction.y < 0)
            {
                PlayAnimation(AnimationStates.WalkDown);
            }
            else if (direction.x < 0)
            {
                PlayAnimation(AnimationStates.WalkLeft);
            }
            else if (direction.x > 0)
            {
                PlayAnimation(AnimationStates.WalkRight);
            }
            StartCoroutine("AnimationDelayTimeReset");
        }

        public void PlayAnimation(AnimationStates state)
        {
            switch (state)
            {
                case AnimationStates.Idle:
                    sRenderer.sprite = animationPerStyle[Style].Idle;
                    break;
                case AnimationStates.WalkUp:
                    sRenderer.sprite = animationPerStyle[Style].WalkUp;
                    break;
                case AnimationStates.WalkDown:
                    sRenderer.sprite = animationPerStyle[Style].WalkDown;
                    break;
                case AnimationStates.WalkLeft:
                    sRenderer.sprite = animationPerStyle[Style].WalkLeft;
                    break;
                case AnimationStates.WalkRight:
                    sRenderer.sprite = animationPerStyle[Style].WalkRight;
                    break;
                case AnimationStates.Done:
                    sRenderer.sprite = animationPerStyle[Style].Done;
                    break;
                default:
                    break;
            }
        }

        private IEnumerator AnimationDelayTimeReset()
        {
            yield return new WaitForSeconds(ResetDelay);
            PlayAnimation(AnimationStates.Idle);
        }

        public void PlayDoneAnimation()
        {
            PlayAnimation(AnimationStates.Done);
        }

    }

    [System.Serializable]
    public struct SpriteAnimationCollection
    {
        [PreviewField, HorizontalGroup("v1")]
        public Sprite Idle;
        [PreviewField, HorizontalGroup("v1")]
        public Sprite Done;
        [PreviewField, HorizontalGroup("v2")]
        public Sprite WalkUp;
        [PreviewField, HorizontalGroup("v2")]
        public Sprite WalkDown;
        [PreviewField, HorizontalGroup("v3")]
        public Sprite WalkLeft;
        [PreviewField, HorizontalGroup("v3")]
        public Sprite WalkRight;
    }
}