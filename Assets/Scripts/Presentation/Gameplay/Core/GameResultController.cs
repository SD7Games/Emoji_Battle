using System;
using System.Collections;
using UnityEngine;

public sealed class GameResultController
{
    private readonly WinLineView _lines;
    private readonly GameRewardService _rewards;
    private readonly MonoBehaviour _coroutineRunner;
    private readonly InputController _input;

    private const float DRAW_DELAY = 0.8f;
    private const float POPUP_BLOCK_TIME = 0.3f;

    public event Action<CellState, GameRewardResult> ResultReady;

    public GameResultController(
        WinLineView lines,
        GameRewardService rewards,
        MonoBehaviour coroutineRunner,
        InputController input)
    {
        _lines = lines;
        _rewards = rewards;
        _coroutineRunner = coroutineRunner;
        _input = input;
    }

    public void HandleGameOver(CellState winner, WinLineView.WinLineType? line)
    {
        _input.Block();

        var diff = GameDataService.I.Data.AI.Strategy;

        GameRewardResult reward =
            _rewards.OnWin(winner, diff, InternetService.IsOnline);

        if (line.HasValue)
        {
            _lines.ShowWinLine(
                line.Value,
                () => NotifyResultReady(winner, reward)
            );
            return;
        }

        _coroutineRunner.StartCoroutine(ShowDrawDelayed(winner, reward));
    }

    private IEnumerator ShowDrawDelayed(CellState winner, GameRewardResult reward)
    {
        yield return new WaitForSeconds(DRAW_DELAY);
        NotifyResultReady(winner, reward);
    }

    private void NotifyResultReady(CellState winner, GameRewardResult reward)
    {
        _coroutineRunner.StartCoroutine(
            _input.BlockForSeconds(POPUP_BLOCK_TIME)
        );

        ResultReady?.Invoke(winner, reward);
    }
}