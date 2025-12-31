using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AudioService : MonoBehaviour
{
    public static AudioService I { get; private set; }

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    private Coroutine _musicFadeRoutine;
    private Coroutine _musicQueueRoutine;

    private MusicDefinition _currentMusic;

    private readonly List<AudioClip> _playlist = new();
    private int _playlistIndex;

    private float _musicBaseVolume = 1f;

    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);

        _musicSource.volume = 0f;
        _sfxSource.volume = 1f;

        SettingsService.MusicChanged += RefreshMusicVolume;
    }

    private void OnDestroy()
    {
        if (I == this)
            SettingsService.MusicChanged -= RefreshMusicVolume;
    }

    public void PlayMusic(MusicDefinition music)
    {
        if (music == null || music.Tracks == null || music.Tracks.Length == 0)
            return;

        StopAllMusicRoutines();

        _currentMusic = music;
        _musicBaseVolume = music.Volume;

        BuildPlaylist();
        PlayNextFromPlaylist();
    }

    public void FadeToMusic(MusicDefinition music, float duration)
    {
        if (music == null || music.Tracks == null || music.Tracks.Length == 0)
            return;

        StopAllMusicRoutines();

        _currentMusic = music;
        _musicBaseVolume = music.Volume;

        BuildPlaylist();
        _musicFadeRoutine = StartCoroutine(FadeInNextFromPlaylist(duration));
    }

    public void StopMusic()
    {
        StopAllMusicRoutines();

        _musicSource.Stop();
        _musicSource.clip = null;
        _currentMusic = null;
        _playlist.Clear();
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

        _playlistIndex = 0;
    }

    private void PlayNextFromPlaylist()
    {
        if (_currentMusic == null)
            return;

        if (_playlistIndex >= _playlist.Count)
            BuildPlaylist();

        var clip = _playlist[_playlistIndex++];
        if (clip == null)
            return;

        _musicSource.clip = clip;
        _musicSource.pitch = 1f;

        RefreshMusicVolume();
        _musicSource.Play();

        _musicQueueRoutine = StartCoroutine(WaitForEnd(clip.length));
    }

    private IEnumerator WaitForEnd(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        PlayNextFromPlaylist();
    }

    private IEnumerator FadeInNextFromPlaylist(float duration)
    {
        if (_playlistIndex >= _playlist.Count)
            BuildPlaylist();

        var clip = _playlist[_playlistIndex++];
        if (clip == null)
            yield break;

        _musicSource.clip = clip;
        _musicSource.volume = 0f;
        _musicSource.pitch = 1f;
        _musicSource.Play();

        float target = SettingsService.I.Data.MusicEnabled
            ? _musicBaseVolume * SettingsService.I.Data.MusicVolume
            : 0f;

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            _musicSource.volume = Mathf.Lerp(0f, target, t / duration);
            yield return null;
        }

        _musicSource.volume = target;
        _musicQueueRoutine = StartCoroutine(WaitForEnd(clip.length));
    }

    private void StopAllMusicRoutines()
    {
        if (_musicFadeRoutine != null)
        {
            StopCoroutine(_musicFadeRoutine);
            _musicFadeRoutine = null;
        }

        if (_musicQueueRoutine != null)
        {
            StopCoroutine(_musicQueueRoutine);
            _musicQueueRoutine = null;
        }
    }

    public void RefreshMusicVolume()
    {
        if (_musicSource.clip == null)
            return;

        var data = SettingsService.I.Data;

        _musicSource.volume = data.MusicEnabled
            ? _musicBaseVolume * data.MusicVolume
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

        _sfxSource.PlayOneShot(clip, sound.Volume * data.SfxVolume);
    }
}