using UnityEngine;
using UnityEngine.UI;

public sealed class VictoryPopup : ResultPopup, IEmojiResolverConsumer
{
    public override PopupId Id => PopupId.Victory;

    [SerializeField] private Image _emojiImage;
    [SerializeField] private int _previewColorId = 0;

    private EmojiResolver _resolver;

    public void Construct(EmojiResolver resolver)
    {
        _resolver = resolver;
    }

    public override void Show()
    {
        base.Show();
        UpdateEmojiPreview();
    }

    private void UpdateEmojiPreview()
    {
        var progress = GameDataService.I.Data.Progress;
        int index = progress.LastUnlockedGlobalIndex;

        if (index < 0)
        {
            _emojiImage.enabled = false;
            return;
        }

        _emojiImage.sprite = _resolver.Get(_previewColorId, index);
        _emojiImage.enabled = true;
    }
}