using UnityEngine;

public sealed class LobbyService
{
    private readonly EmojiSelectionService _emojiService;
    private readonly AISelectionService _aiService;

    public LobbyService(EmojiSelectionService emojiService, AISelectionService aiService)
    {
        _emojiService = emojiService;
        _aiService = aiService;
    }

    public string GenerateAIName()
        => _aiService.GenerateRandomName();

    public string GetAIName()
        => _aiService.GetCurrentName();

    public EmojiViewData[] GetEmojiItems(int colorId)
        => _emojiService.BuildEmojiList(colorId);

    public Sprite SelectPlayerEmoji(int colorId, int emojiIndex)
        => _emojiService.SelectPlayerEmoji(colorId, emojiIndex);

    public Sprite EnsureValidAIEmoji()
        => _aiService.EnsureValidAIEmoji();
}