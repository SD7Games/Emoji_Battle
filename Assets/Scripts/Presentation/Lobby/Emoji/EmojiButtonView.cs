using UnityEngine;
using UnityEngine.UI;

public class EmojiButtonView : MonoBehaviour
{
    public Button Button;
    public Image Icon;
    public GameObject LockedOverlay;
    public GameObject LockIcon;

    public int EmojiIndex { get; private set; }

    public void Bind(int emojiIndex)
    {
        EmojiIndex = emojiIndex;
    }
}