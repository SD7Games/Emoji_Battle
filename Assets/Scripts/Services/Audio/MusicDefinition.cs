using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Music")]
public sealed class MusicDefinition : ScriptableObject
{
    public AudioClip[] Tracks;

    [Range(0f, 1f)]
    public float Volume = 1f;
}