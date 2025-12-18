using System.Collections;
using UnityEngine;

public sealed class GameResultController
{
    private readonly WinLineView _lines;
    private readonly GameRewardService _rewards;
    private readonly MonoBehaviour _coroutineRunner;

    private const float DRAW_DELAY = 0.8f;

    public GameResultController(
        WinLineView lines,
        GameRewardService rewards,
        MonoBehaviour coroutineRunner
    )
    {
        _lines = lines;
        _rewards = rewards;
        _coroutineRunner = coroutineRunner;
    }

    public void HandleGameOver(
        CellState winner,
        WinLineView.WinLineType? line
    )
    {
        _rewards.OnWin(winner);

        if (line.HasValue)
        {
            _lines.ShowWinLine(
                line.Value,
                () => ShowResultPopup(winner)
            );
        }
        else
        {
            _coroutineRunner.StartCoroutine(ShowDrawDelayed(winner));
        }
    }

    private IEnumerator ShowDrawDelayed(CellState winner)
    {
        yield return new WaitForSeconds(DRAW_DELAY);
        ShowResultPopup(winner);
    }

    private void ShowResultPopup(CellState winner)
    {
        PopupId id = winner switch
        {
            CellState.Player => PopupId.Victory,
            CellState.AI => PopupId.Defeat,
            _ => PopupId.Draw
        };

        PopupService.I.Show(id);
    }
}