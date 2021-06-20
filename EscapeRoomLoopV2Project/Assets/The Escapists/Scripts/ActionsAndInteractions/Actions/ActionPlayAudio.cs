using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TheEscapists.ActionsAndInteractions.Actions
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(AudioClip))]
    public class ActionPlayAudio : Action
    {

        AudioSource source;
        AudioClip audioClip;

        private void Start()
        {
            audioClip = GetComponent<AudioClip>();
            source = GetComponent<AudioSource>();
        }

        public override void TriggerEnable()
        {
                if(source.clip != audioClip)
                    source.clip = audioClip;
                if(!source.isPlaying)
                    source.Play();
        }

        public override void TriggerDisable()
        {
            if (source.isPlaying)
                source.Stop();
        }
    }
}