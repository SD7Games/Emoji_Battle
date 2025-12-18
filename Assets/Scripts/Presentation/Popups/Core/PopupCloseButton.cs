using UnityEngine;
using UnityEngine.UI;

public sealed class PopupCloseButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake()
    {
        if (_button == null)
            _button = GetComponent<Button>();

        _button.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        PopupService.I.HideCurrent();
    }
}