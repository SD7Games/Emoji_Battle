using System.Collections;
using UnityEngine;

public sealed class AudioService : MonoBehaviour
{
    public static AudioService I { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource _music;

    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioSource _ui;

    private Coroutine _musicFadeRoutine;

    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(SoundDefinition sound)
    {
        if (sound == null)
            return;

        var clip = sound.GetClip();
        if (clip == null)
            return;

        var source = sound.Channel == AudioChannel.Ui ? _ui : _sfx;

        source.pitch = sound.PitchRange == Vector2.one
            ? 1f
            : Random.Range(sound.PitchRange.x, sound.PitchRange.y);

        source.PlayOneShot(clip, sound.Volume);
    }

    public void PlayMusic(AudioClip clip, float volume = 0.4f, bool loop = true)
    {
        if (clip == null)
            return;

        if (_music.clip == clip && _music.isPlaying)
            return;

        StopFade();

        _music.clip = clip;
        _music.volume = volume;
        _music.loop = loop;
        _music.pitch = 1f;
        _music.Play();
    }

    public void PlayMusic(MusicDefinition music)
    {
        if (music == null || music.Clip == null)
            return;

        PlayMusic(music.Clip, music.Volume, music.Loop);
    }

    public void FadeToMusic(MusicDefinition music, float fadeDuration = 0.5f)
    {
        if (music == null || music.Clip == null)
            return;

        StopFade();
        _musicFadeRoutine = StartCoroutine(FadeRoutine(music, fadeDuration));
    }

    public void StopMusic(float fadeDuration = 0f)
    {
        StopFade();

        if (fadeDuration <= 0f)
        {
            _music.Stop();
            _music.clip = null;
        }
        else
        {
            _musicFadeRoutine = StartCoroutine(FadeOutRoutine(fadeDuration));
        }
    }

    private IEnumerator FadeRoutine(MusicDefinition music, float duration)
    {
        float startVolume = _music.isPlaying ? _music.volume : 0f;

        if (_music.clip != music.Clip)
        {
            _music.clip = music.Clip;
            _music.loop = music.Loop;
            _music.volume = 0f;
            _music.Play();
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            _music.volume = Mathf.Lerp(startVolume, music.Volume, t / duration);
            yield return null;
        }

        _music.volume = music.Volume;
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        float startVolume = _music.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            _music.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        _music.Stop();
        _music.clip = null;
    }

    private void StopFade()
    {
        if (_musicFadeRoutine != null)
        {
            StopCoroutine(_musicFadeRoutine);
            _musicFadeRoutine = null;
        }
    }
}