using System.Collections;
using UnityEngine;

public sealed class AudioService : MonoBehaviour
{
    public static AudioService I { get; private set; }

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    private Coroutine _musicFadeRoutine;
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
        if (music == null || music.Clip == null)
            return;

        StopFade();

        _musicBaseVolume = music.Volume;

        _musicSource.clip = music.Clip;
        _musicSource.loop = music.Loop;
        _musicSource.pitch = 1f;

        RefreshMusicVolume();
        _musicSource.Play();
    }

    public void FadeToMusic(MusicDefinition music, float duration)
    {
        if (music == null || music.Clip == null)
            return;

        StopFade();
        _musicFadeRoutine = StartCoroutine(FadeRoutine(music, duration));
    }

    public void StopMusic()
    {
        StopFade();
        _musicSource.Stop();
        _musicSource.clip = null;
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

    private IEnumerator FadeRoutine(MusicDefinition music, float duration)
    {
        _musicBaseVolume = music.Volume;

        float start = _musicSource.isPlaying ? _musicSource.volume : 0f;

        if (_musicSource.clip != music.Clip)
        {
            _musicSource.clip = music.Clip;
            _musicSource.loop = music.Loop;
            _musicSource.pitch = 1f;
            _musicSource.volume = 0f;
            _musicSource.Play();
        }

        float target = SettingsService.I.Data.MusicEnabled
            ? _musicBaseVolume * SettingsService.I.Data.MusicVolume
            : 0f;

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            _musicSource.volume = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }

        _musicSource.volume = target;
    }

    private void StopFade()
    {
        if (_musicFadeRoutine == null)
            return;

        StopCoroutine(_musicFadeRoutine);
        _musicFadeRoutine = null;
    }
}