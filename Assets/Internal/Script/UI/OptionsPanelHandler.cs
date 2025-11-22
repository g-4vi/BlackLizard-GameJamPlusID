using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace GameJamPlus {
    public class OptionsPanelHandler : MonoBehaviour {

        [Header("Audio Settings")]
        [SerializeField] AudioMixer audioMixer;
        [SerializeField] Slider masterVolumeSlider;

        [Header("Graphics Settings")]
        [SerializeField] TMP_Dropdown resolutionDropdown;
        Resolution[] resolutions;

        const string PREF_VOLUME = "masterVolume";
        const string PREF_RESOLUTION = "resolutionIndex";

        void Start() {
            InitializeResolutionsSettings();
            InitializeAudioSettings();
        }

        public void SetVolume(float volume) {
            float dbVolume = Mathf.Log10(volume) * 20;
            if (volume == 0) dbVolume = -80f;

            // TODO: Ensure the name of the parameter matches the one in the AudioMixer
            audioMixer.SetFloat("MasterVolume", dbVolume);
            // TODO: Consider adding audio feedback for volume change

            PlayerPrefs.SetFloat(PREF_VOLUME, volume);
            PlayerPrefs.Save();
        }

        public void SetResolution(int resolutionIndex) {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

            // TODO: Audio?

            PlayerPrefs.SetInt(PREF_RESOLUTION, resolutionIndex);
            PlayerPrefs.Save();
        }

        void InitializeResolutionsSettings() {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            int savedResolutionIndex = PlayerPrefs.GetInt(PREF_RESOLUTION, -1);

            for (int i = 0; i < resolutions.Length; i++) {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (savedResolutionIndex != -1) {
                    if (i == savedResolutionIndex) {
                        currentResolutionIndex = i;
                    }
                } else {
                    if (resolutions[i].width == Screen.currentResolution.width &&
                        resolutions[i].height == Screen.currentResolution.height) {
                        currentResolutionIndex = i;
                    }
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        void InitializeAudioSettings() {
            float savedVolume = PlayerPrefs.GetFloat(PREF_VOLUME, 1f);
            masterVolumeSlider.value = savedVolume;
            SetVolume(savedVolume);
        }

    }
}