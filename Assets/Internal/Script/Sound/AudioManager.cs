using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GameJamPlus;
using UnityEngine;

/*
    How to use AudioManager:
    1. Add SoundData assets (ScriptableObject) to the AudioManager's sfxs and musics lists in the inspector.
    2. Generate the Audio Enums by clicking the "Generate Audio Enums" button in the AudioManager inspector.
    3. Use AudioManager.Instance.PlaySFX(SfxID.YourSfxName) or AudioManager.Instance.PlayMusic(MusicID.YourMusicName) to play sounds.
*/

public class AudioManager : Singleton<AudioManager> {

    // Sources
    [Header("Audio Data")]
    public List<SoundData> sfxs;
    public List<SoundData> musics;

    [SerializeField] MusicID BGM;
    [SerializeField] SfxID buttonSFX;

    public SfxID ButtonSFX => buttonSFX;


    // Pool
    List<AudioSource> sfxSources = new List<AudioSource>();
    AudioSource musicSource;
    Dictionary<SfxID, SoundData> sfxMap = new Dictionary<SfxID, SoundData>();
    Dictionary<MusicID, SoundData> musicMap = new Dictionary<MusicID, SoundData>();

    int poolSize = 20;

    protected override void Awake() {
        base.Awake();
        
        InitializePool();
        InitializeMaps();
        StopAllAudio();
        if (BGM != MusicID.None)
            PlayMusic(BGM);
    }

 

    void InitializePool() {
        sfxSources = new List<AudioSource>();

        // For SFX
        for (int i = 0; i < poolSize; i++) {
            GameObject obj = new GameObject("SFX_Source_" + i);
            obj.transform.SetParent(transform);
            AudioSource source = obj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sfxSources.Add(source);
        }

        // For Music
        GameObject musicObj = new GameObject("Music_Source");
        musicObj.transform.SetParent(transform);
        musicSource = musicObj.AddComponent<AudioSource>();
        musicSource.loop = true;
    }

    void InitializeMaps() {
        foreach (var entry in sfxs) {
            if (System.Enum.TryParse(entry.name, out SfxID idEnum)) {
                if (!sfxMap.ContainsKey(idEnum)) {
                    sfxMap.Add(idEnum, entry);
                }
            }
        }

        foreach (var entry in musics) {
            if (System.Enum.TryParse(entry.name, out MusicID idEnum)) {
                if (!musicMap.ContainsKey(idEnum)) {
                    musicMap.Add(idEnum, entry);
                }
            }
        }
    }

    // Play by SoundData
    public void PlaySFX(SoundData soundData, Vector2 position = default) {
        if (soundData == null) return;

        AudioSource source = GetAvailableSource();
        if (source == null) return;

        // audio setup
        source.transform.position = position;
        source.clip = soundData.GetAudioClip();
        source.outputAudioMixerGroup = soundData.mixerGroup;
        source.volume = soundData.volume;
        source.spatialBlend = soundData.spatialBlend;

        if (soundData.useRandomPitch) {
            source.pitch = soundData.pitch + Random.Range(-soundData.randomPitchRange, soundData.randomPitchRange);
        } else {
            source.pitch = soundData.pitch;
        }

        source.Play();
    }

   

    // Play by enum ID
    public void PlaySFX(SfxID sfxID, Vector2 position = default) {
        if (sfxID == SfxID.None) return;
        if (sfxMap.TryGetValue(sfxID, out SoundData soundData)) {
            PlaySFX(soundData, position);
        }
    }



    // Play Music by SoundData
    public void PlayMusic(SoundData musicData) {
        if (musicData == null) return;

        if (musicSource.clip == musicData.clips[0] && musicSource.isPlaying) return; // already playing this music

        musicSource.clip = musicData.GetAudioClip();
        musicSource.outputAudioMixerGroup = musicData.mixerGroup;
        musicSource.volume = musicData.volume;
        musicSource.pitch = musicData.pitch;
        musicSource.Play();
    }

    // Play Music by enum ID
    public void PlayMusic(MusicID musicID) {
        if (musicID == MusicID.None) return;
        if (musicMap.TryGetValue(musicID, out SoundData musicData)) {
            PlayMusic(musicData);
        }
    }

    AudioSource GetAvailableSource() {
        foreach (var source in sfxSources) {
            if (!source.isPlaying) return source;
        }

        return null;
    }

}