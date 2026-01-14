using UnityEngine;
using UnityEngine.UI;

public sealed class VictoryPopup : ResultPopup, IEmojiResolverConsumer
{
    public override PopupId Id => PopupId.Victory;

    [SerializeField] private Image _emojiImage;
    [SerializeField] private int _previewColorId = 0;
    [SerializeField] private ParticleSystem _poofFx;

    private EmojiResolver _resolver;

    public void Construct(EmojiResolver resolver)
    {
        _resolver = resolver;
    }

    public override void Show()
    {
        base.Show();
        _poofFx.Clear();
        _poofFx.Play();
        UpdateEmojiPreview();
    }

    private void UpdateEmojiPreview()
    {
        if (_emojiImage == null || _resolver == null)
            return;

        var progress = GameDataService.I.Data.Progress;

        if (!progress.TryGetLastUnlockedEmoji(out var emojiId))
        {
            _emojiImage.enabled = false;
            return;
        }

        var sprite = _resolver.Get(_previewColorId, emojiId);
        _emojiImage.sprite = sprite;
        _emojiImage.enabled = sprite != null;
    }
}