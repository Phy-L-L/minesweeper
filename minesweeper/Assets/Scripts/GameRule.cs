using UnityEngine;

public class GameRule : MonoBehaviour
{
    [SerializeField]
    private int _mineCount = 10;
    public int MineCount => _mineCount;
    [SerializeField]
    private int _columnCount = 9;
    public int ColumnCount => _columnCount;
    [SerializeField]
    private int _rowCount = 9;
    public int RowCount => _rowCount;
}