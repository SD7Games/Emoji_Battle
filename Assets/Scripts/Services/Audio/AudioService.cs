using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AudioService : MonoBehaviour
{
    public static AudioService I { get; private set; }

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    private MusicDefinition _currentMusic;

    private readonly List<AudioClip> _playlist = new();
    private int _playlistIndex;

    private Coroutine _musicRoutine;
    private float _baseMusicVolume = 1f;

    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);

        _musicSource.loop = false;
        SettingsService.MusicChanged += RefreshMusicVolume;
    }

    private void OnDestroy()
    {
        if (I == this)
            SettingsService.MusicChanged -= RefreshMusicVolume;
    }

    public void PlayMusicIfDifferent(MusicDefinition music)
    {
        if (music == null || music.Tracks == null || music.Tracks.Length == 0)
            return;

        if (_currentMusic == music && _musicSource.isPlaying)
            return;

        PlayMusic(music);
    }

    public void PlayMusic(MusicDefinition music)
    {
        if (music == null || music.Tracks == null || music.Tracks.Length == 0)
            return;

        StopMusicInternal();

        _currentMusic = music;
        _baseMusicVolume = music.Volume;

        BuildPlaylist();
        _musicRoutine = StartCoroutine(PlaylistRoutine());
    }

    public void StopMusic()
    {
        StopMusicInternal();
        _currentMusic = null;
    }

    public void RefreshMusicVolume()
    {
        if (_musicSource.clip == null)
            return;

        var data = SettingsService.I.Data;

        _musicSource.volume = data.MusicEnabled
            ? _baseMusicVolume * data.MusicVolume
            : 0f;
    }

    public void Play(SoundDefinition sound)
    {
        if (sound == null)
            return;

        var data = SettingsService.I.Data;
        if (!data.SfxEnabled)
            return;

        var clip = sound.GetClip();
        if (clip == null)
            return;

        _sfxSource.pitch = sound.PitchRange == Vector2.one
            ? 1f
            : Random.Range(sound.PitchRange.x, sound.PitchRange.y);

        _sfxSource.PlayOneShot(
            clip,
            sound.Volume * data.SfxVolume
        );
    }

    private void StopMusicInternal()
    {
        if (_musicRoutine != null)
        {
            StopCoroutine(_musicRoutine);
            _musicRoutine = null;
        }

        _musicSource.Stop();
        _musicSource.clip = null;

        _playlist.Clear();
        _playlistIndex = 0;
    }

    private void BuildPlaylist()
    {
        _playlist.Clear();
        _playlist.AddRange(_currentMusic.Tracks);

        // Fisher–Yates shuffle
        for (int i = 0; i < _playlist.Count; i++)
        {
            int j = Random.Range(i, _playlist.Count);
            (_playlist[i], _playlist[j]) = (_playlist[j], _playlist[i]);
        }

        _playlistIndex = Random.Range(0, _playlist.Count);
    }

    private IEnumerator PlaylistRoutine()
    {
        while (_currentMusic != null)
        {
            if (_playlistIndex >= _playlist.Count)
                BuildPlaylist();

            var clip = _playlist[_playlistIndex++];
            if (clip == null)
                continue;

            _musicSource.clip = clip;
            _musicSource.pitch = 1f;

            RefreshMusicVolume();
            _musicSource.Play();

            yield return new WaitWhile(() => _musicSource.isPlaying);
        }
    }
}