using System;

public abstract class ResultPopup : PopupBase
{
    public event Action Closed;

    public override void Hide()
    {
        base.Hide();
        Closed?.Invoke();
    }
}