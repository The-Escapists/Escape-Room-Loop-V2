using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace TheEscapists.Core.Manager
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField]
        AudioMixer mixer;
        [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        public AudioClip[] musik;
        public AudioSource source;
        //Only in MainMenu
        [SerializeField, Title("Only Main Menu")]
        Slider audioVolumeSlider;
        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance == this)
            {

            }
            else if (Instance != this)
            {
                Destroy(this);
            }

            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                float audioValue = PlayerPrefs.GetFloat("MasterVolume");
                audioVolumeSlider.value = audioValue;
                audioValue = Mathf.Log10(audioValue) * 20;
                mixer.SetFloat("MasterVolume", audioValue);
            }
            else
            {
                float audioValue = Mathf.Log10(1) * 20;
                mixer.SetFloat("MasterVolume", audioValue);
                PlayerPrefs.SetFloat("MasterVolume", 1);
                PlayerPrefs.Save();

            }
        }

        private void Update()
        {
            if (!source.isPlaying)
            {
                source.clip = musik[Random.Range(0, musik.Length)];
                source.Play();
            }
        }

        /// <summary>
        /// Sets Audio Value in Main Menu
        /// </summary>
        /// <param name="sliderValue"></param>
        //Set Slider Value
        public void ChangeVolumeSliderValue(float sliderValue)
        {
            float audioValue = Mathf.Log10(sliderValue) * 20;
            PlayerPrefs.SetFloat("MasterVolume", sliderValue);
            mixer.SetFloat("MasterVolume", audioValue);
            PlayerPrefs.Save();
        }
    }
}