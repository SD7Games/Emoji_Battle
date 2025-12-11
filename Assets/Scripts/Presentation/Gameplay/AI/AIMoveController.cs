using System.Collections;
using UnityEngine;

public class AIMoveController : MonoBehaviour
{
    private IAIStrategy strategy;
    private GameFlow flow;
    private float delay = 0.6f;

    public void Init(GameFlow flow, float delay = 0.6f)
    {
        this.flow = flow;
        this.delay = delay;

        LoadStrategyFromSave();
    }

    private void LoadStrategyFromSave()
    {
        var ai = GameDataService.I.Data.AI;
        strategy = AIStrategyFactory.Create(ai.Strategy);
    }

    public void MakeMove(int[] boardState)
    {
        StartCoroutine(AIMoveRoutine(boardState));
    }

    private IEnumerator AIMoveRoutine(int[] board)
    {
        yield return new WaitForSeconds(delay);

        int index = strategy.TryGetMove(board);

        if (index >= 0)
            flow.ProcessMove(index);
    }
}