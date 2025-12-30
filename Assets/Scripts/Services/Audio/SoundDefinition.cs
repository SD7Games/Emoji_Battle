using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Sound")]
public sealed class SoundDefinition : ScriptableObject
{
    public AudioClip[] Clips;

    [Range(0f, 1f)]
    public float Volume = 1f;

    public Vector2 PitchRange = Vector2.one;

    public AudioClip GetClip()
    {
        if (Clips == null || Clips.Length == 0)
            return null;

        return Clips.Length == 1
            ? Clips[0]
            : Clips[Random.Range(0, Clips.Length)];
    }
}