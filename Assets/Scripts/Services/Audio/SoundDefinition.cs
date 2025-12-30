using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Sound")]
public sealed class SoundDefinition : ScriptableObject
{
    [Header("Routing")]
    public AudioChannel Channel;

    [Header("Clips")]
    public AudioClip[] Clips;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float Volume = 1f;

    [Tooltip("Use 1,1 for no pitch random")]
    public Vector2 PitchRange = Vector2.one;

    public AudioClip GetClip()
    {
        if (Clips == null || Clips.Length == 0)
            return null;

        if (Clips.Length == 1)
            return Clips[0];

        return Clips[Random.Range(0, Clips.Length)];
    }
}