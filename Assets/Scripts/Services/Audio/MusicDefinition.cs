using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Music")]
public sealed class MusicDefinition : ScriptableObject
{
    public AudioClip Clip;

    [Range(0f, 1f)]
    public float Volume = 0.4f;

    public bool Loop = true;
}