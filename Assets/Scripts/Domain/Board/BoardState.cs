using System;

public class BoardState
{
    public const int Size = 3;

    private readonly CellState[,] _data = new CellState[Size, Size];

    public CellState[,] Data => (CellState[,]) _data.Clone();

    public bool IsEmpty(int index)
    {
        if (index < 0 || index >= Size * Size)
            throw new ArgumentOutOfRangeException(nameof(index));

        var (x, y) = IndexToCoord(index);
        return _data[x, y] == CellState.Empty;
    }

    public bool TrySet(int index, CellState state)
    {
        if (index < 0 || index >= Size * Size)
            throw new ArgumentOutOfRangeException(nameof(index));

        var (x, y) = IndexToCoord(index);

        if (_data[x, y] != CellState.Empty)
            return false;

        _data[x, y] = state;
        return true;
    }

    public void Set(int index, CellState state)
    {
        if (index < 0 || index >= Size * Size)
            throw new ArgumentOutOfRangeException(nameof(index));

        var (x, y) = IndexToCoord(index);
        _data[x, y] = state;
    }

    public void Reset()
    {
        for (int x = 0; x < Size; x++)
            for (int y = 0; y < Size; y++)
                _data[x, y] = CellState.Empty;
    }

    public int[] AsIntArray()
    {
        int[] arr = new int[Size * Size];

        for (int i = 0; i < arr.Length; i++)
        {
            var (x, y) = IndexToCoord(i);
            arr[i] = (int) _data[x, y];
        }

        return arr;
    }

    public static (int x, int y) IndexToCoord(int index)
        => (index / Size, index % Size);
}