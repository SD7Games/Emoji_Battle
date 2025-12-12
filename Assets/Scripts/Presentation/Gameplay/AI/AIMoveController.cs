using System.Collections;
using UnityEngine;

public class AIMoveController : MonoBehaviour
{
    private IAIStrategy _strategy;
    private GameFlow _flow;
    private float _delay = 0.6f;

    public void Init(GameFlow flow, float delay = 0.6f)
    {
        this._flow = flow;
        this._delay = delay;

        LoadStrategyFromSave();
    }

    private void LoadStrategyFromSave()
    {
        var ai = GameDataService.I.Data.AI;
        _strategy = AIStrategyFactory.Create(ai.Strategy);
    }

    public void MakeMove(int[] boardState)
    {
        StartCoroutine(AIMoveRoutine(boardState));
    }

    private IEnumerator AIMoveRoutine(int[] board)
    {
        yield return new WaitForSeconds(_delay);

        int index = _strategy.TryGetMove(board);

        if (index >= 0)
            _flow.ProcessMove(index);
    }
}