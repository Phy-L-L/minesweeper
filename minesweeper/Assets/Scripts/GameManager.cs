using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 最初の操作で地雷を踏まないようにするための実装
    public bool FirstTime { get; set; } = true;
    public bool IsGameOver { get; set; } = false;

    [SerializeField]
    private int _landmineCount = 10;
    public int LandmineCount => _landmineCount;
    [SerializeField]
    private int _rowCount = 9;
    public int RowCount => _rowCount;
    [SerializeField]
    private int _columnCount = 9;
    public int ColumnCount => _columnCount;
}