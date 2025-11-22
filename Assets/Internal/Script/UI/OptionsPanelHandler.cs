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
        [SerializeField] Slider musicVolumeSlider;
        [SerializeField] Slider sfxVolumeSlider;

        [Header("Graphics Settings")]
        [SerializeField] TMP_Dropdown resolutionDropdown;
        Resolution[] resolutions;

        const string PREF_MASTER_VOLUME = "MasterVolume";
        const string PREF_MUSIC_VOLUME = "MusicVolume";
        const string PREF_SFX_VOLUME = "SFXVolume";
        const string PREF_RESOLUTION = "ResolutionIndex";

        void Start() {
            InitializeResolutionsSettings();
            InitializeAudioSettings();
            WireEvents();
        }

        public void SetMasterVolume(float volume) => SetVolume(volume, PREF_MASTER_VOLUME);
        public void SetMusicVolume(float volume) => SetVolume(volume, PREF_MUSIC_VOLUME);
        public void SetSFXVolume(float volume) => SetVolume(volume, PREF_SFX_VOLUME);

        public void SetResolution(int resolutionIndex) {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

            // TODO: Audio?

            PlayerPrefs.SetInt(PREF_RESOLUTION, resolutionIndex);
            PlayerPrefs.Save();
        }

        void SetVolume(float volume, string parameterName) {
            float dbVolume = Mathf.Log10(volume) * 20;
            if (volume == 0) dbVolume = -80f;

            // TODO: Ensure the name of the parameter matches the one in the AudioMixer
            audioMixer.SetFloat(parameterName, dbVolume);
            // TODO: Consider adding audio feedback for volume change

            PlayerPrefs.SetFloat(parameterName, volume);
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
            float savedMasterVolume = PlayerPrefs.GetFloat(PREF_MASTER_VOLUME, 0.5f);
            masterVolumeSlider.value = savedMasterVolume;
            SetMasterVolume(savedMasterVolume);

            float savedMusicVolume = PlayerPrefs.GetFloat(PREF_MUSIC_VOLUME, 0.5f);
            musicVolumeSlider.value = savedMusicVolume;
            SetMusicVolume(savedMusicVolume);

            float savedSFXVolume = PlayerPrefs.GetFloat(PREF_SFX_VOLUME, 0.5f);
            sfxVolumeSlider.value = savedSFXVolume;
            SetSFXVolume(savedSFXVolume);
        }

        void WireEvents() {
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }

        void OnDestroy() {
            masterVolumeSlider.onValueChanged.RemoveListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.RemoveListener(SetSFXVolume);
            resolutionDropdown.onValueChanged.RemoveListener(SetResolution);
        }

    }
}