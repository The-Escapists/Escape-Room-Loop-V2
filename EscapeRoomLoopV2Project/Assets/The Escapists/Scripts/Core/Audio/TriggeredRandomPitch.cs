using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace TheEscapists.Core.Audio
{
    public class TriggeredRandomPitch : MonoBehaviour
    {

        enum PitchTarget {AudioSource, AudioMixerGroup};
        [SerializeField, EnumToggleButtons]
        PitchTarget pitchTarget;
        [ShowIf("@pitchTarget == PitchTarget.AudioMixerGroup"), SerializeField]
        AudioMixerGroup group;
        [ShowIf("@pitchTarget == PitchTarget.AudioMixerGroup"), SerializeField]
        AudioMixer mixer;
        [ShowIf("@pitchTarget == PitchTarget.AudioSource"), SerializeField]
        AudioSource audioSource;

        [MinMaxSlider(1, 200, true), SerializeField]
        Vector2 randomPitchRange = new Vector2(1, 200);

        /// <summary>
        /// Sets Random Pitch when Triggered
        /// </summary>
        public void Trigger()
        {
            if (pitchTarget == PitchTarget.AudioMixerGroup)
                mixer.SetFloat(group.name + "Pitch", Random.Range(randomPitchRange.x, randomPitchRange.y));
            else if (pitchTarget == PitchTarget.AudioSource)
                audioSource.pitch = Random.Range(randomPitchRange.x, randomPitchRange.y);

        }
    }
}