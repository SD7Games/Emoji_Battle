using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum AvatarOwner
{ Player, AI }

[RequireComponent(typeof(Image))]
public class DissolveLobby : MonoBehaviour
{
    [SerializeField] private float _dissolveTime = 0.75f;
    [SerializeField] private AvatarOwner _owner;

    private Image _image;
    private Material _material;
    private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private Coroutine _routine;

    private void Awake()
    {
        _image = GetComponent<Image>();

        var baseMat = _image.material != null
            ? _image.material
            : Graphic.defaultGraphicMaterial;

        _material = new Material(baseMat);
        _image.material = _material;
    }

    private void OnDisable()
    {
        if (_routine != null)
            StopCoroutine(_routine);
    }

    public void SetSprite(Sprite sprite)
    {
        _image.sprite = sprite;

        if (_image.material != _material)
            _image.material = _material;
    }

    public void PlayForOwner(AvatarOwner owner)
    {
        if (_owner != owner)
            return;

        Play();
    }

    private void Play()
    {
        if (_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(DissolveRoutine());
    }

    private IEnumerator DissolveRoutine()
    {
        float t = 0f;
        _material.SetFloat(DissolveAmount, 1.1f);

        while (t < _dissolveTime)
        {
            t += Time.deltaTime;
            float v = Mathf.Lerp(1.1f, 0f, t / _dissolveTime);
            _material.SetFloat(DissolveAmount, v);
            yield return null;
        }

        _material.SetFloat(DissolveAmount, 0f);
    }
}