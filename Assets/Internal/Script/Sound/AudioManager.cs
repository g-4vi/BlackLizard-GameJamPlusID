using System.Collections;
using System.Collections.Generic;
using GameJamPlus.AudioModules;
using UnityEngine;

/*
    How to use AudioManager:
    Add SFX and MusicTrack ScriptableObjects to the AudioManager's lists in the inspector.
    Then, generate the enums using buttons in the inspector.

    - To play SFX:
        AudioManager.Instance.PlaySFX(SfxID.YourSfxEnum);
    - To play Music:
        AudioManager.Instance.PlayMusic(MusicID.YourMusicEnum);
*/

public class AudioManager : Singleton<AudioManager> {
    private static WaitForSeconds _waitForSeconds1 = new WaitForSeconds(1f);

    // Sources
    [Header("Audio Data")]
    public List<SoundData> sfxs;
    public List<MusicTrackData> musics;

    [SerializeField] MusicID BGM;
    [SerializeField] SfxID buttonSFX;

    public SfxID ButtonSFX => buttonSFX;

    // Pool
    List<AudioSource> sfxSources = new List<AudioSource>();
    Dictionary<SfxID, SoundData> sfxMap = new Dictionary<SfxID, SoundData>();

    AudioSource musicSource;
    Dictionary<MusicID, MusicTrackData> musicMap = new Dictionary<MusicID, MusicTrackData>();
    MusicTrackData currentMusicTrack;
    bool musicLooping;
    double dspLoopEndTime;

    int poolSize = 20;

    protected override void Awake() {
        base.Awake();

        InitializeSFXPool();
        InitializeSFXMaps();

        InitializeMusic();
        InitializeMusicMaps();

        StopAllAudio();
        if (BGM != MusicID.None)
            PlayMusic(BGM);
    }

    void Update() {
        UpdateMusicLoop();
    }

    #region SFX Management
    void InitializeSFXPool() {
        sfxSources = new List<AudioSource>();

        for (int i = 0; i < poolSize; i++) {
            GameObject obj = new GameObject("SFX_Source_" + i);
            obj.transform.SetParent(transform);
            AudioSource source = obj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sfxSources.Add(source);
        }
    }

    void InitializeSFXMaps() {
        foreach (var entry in sfxs) {
            if (System.Enum.TryParse(entry.name, out SfxID idEnum)) {
                if (!sfxMap.ContainsKey(idEnum)) {
                    sfxMap.Add(idEnum, entry);
                }
            }
        }
    }

    /// <summary>
    /// Play SFX using SoundData
    /// </summary>
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

    /// <summary>
    /// Play SFX using enum ID
    /// </summary>
    public void PlaySFX(SfxID sfxID, Vector2 position = default) {
        if (sfxID == SfxID.None) return;
        if (sfxMap.TryGetValue(sfxID, out SoundData soundData)) {
            PlaySFX(soundData, position);
        }
    }

    // Stop specific SFX
    public void StopSFX(SfxID sfxID) {
        if (sfxID == SfxID.None) return;
        if (sfxMap.TryGetValue(sfxID, out SoundData soundData)) {
            foreach (var source in sfxSources) {
                if (source.clip == soundData.GetAudioClip() && source.isPlaying) {
                    source.Stop();
                }
            }
        }
    }

    // Stop all SFX
    public void StopAllSFX() {
        foreach (var source in sfxSources) {
            source.Stop();
        }
    }
    #endregion

    #region Music Management
    void InitializeMusicMaps() {
        foreach (var entry in musics) {
            if (System.Enum.TryParse(entry.name, out MusicID idEnum)) {
                if (!musicMap.ContainsKey(idEnum)) {
                    musicMap.Add(idEnum, entry);
                }
            }
        }
    }

    void InitializeMusic() {
        musicSource = CreateMusicSource("Music_Loop_Source");
    }

    AudioSource CreateMusicSource(string name) {
        var obj = new GameObject(name);
        obj.transform.SetParent(transform);
        var src = obj.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = false;
        return src;
    }

    /// <summary>
    /// Play Music using MusicTrackData
    /// </summary>
    public void PlayMusic(MusicTrackData track) {
        StopMusicImmediate();

        currentMusicTrack = track;
        musicLooping = true;

        // Init
        musicSource.clip = track.clip;
        musicSource.time = 0;
        musicSource.volume = track.volume;
        musicSource.outputAudioMixerGroup = track.mixerGroup;
        musicSource.loop = false;

        if (track.loopEnd <= track.loopStart) {
            // Play music loop normally if no intro/loop defined
            musicSource.loop = true;
            musicSource.Play();
            return;
        }

        musicSource.Play();
        dspLoopEndTime = AudioSettings.dspTime + (track.loopEnd - musicSource.time);
    }

    /// <summary>
    /// Play Music using enum ID
    /// </summary>
    public void PlayMusic(MusicID musicID) {
        if (musicID == MusicID.None) return;
        if (musicMap.TryGetValue(musicID, out MusicTrackData musicData)) {
            PlayMusic(musicData);
        }
    }

    /// <summary>
    /// Go outro after current loop ends
    /// </summary>
    [ContextMenu("Stop Music Loop")]
    public void StopLoopMusic() {
        musicLooping = false;
    }

    /// <summary>
    /// Go outro immediately, without waiting for loop end
    /// </summary>
    [ContextMenu("Stop Music Immediate")]
    public void StopMusicImmediate() {
        musicLooping = false;
        musicSource.Stop();
    }

    // Update music loop
    // It still depends on Update() to be called every frame
    // maybe there's a better way to do this?
    void UpdateMusicLoop() {
        if (musicLooping && musicSource.clip != null) {
            if (AudioSettings.dspTime >= dspLoopEndTime) {
                double offset = currentMusicTrack.loopEnd - currentMusicTrack.loopStart;
                dspLoopEndTime += offset;
                musicSource.time = currentMusicTrack.loopStart;
            }
        }
    }
    #endregion

    /// <summary>
    /// Stop all audio (SFX and Music)
    /// </summary>
    public void StopAllAudio() {
        foreach (var source in sfxSources) {
            source.Stop();
        }
        StopMusicImmediate();
    }

    AudioSource GetAvailableSource() {
        foreach (var source in sfxSources) {
            if (!source.isPlaying) return source;
        }

        return null;
    }

#if UNITY_EDITOR
    [ContextMenu("Run Audio")]
    void RunAudio() {
        StopAllAudio();
        if (BGM != MusicID.None)
            PlayMusic(BGM);
    }
#endif
}