using UnityEngine;

public class CellGenerator : MonoBehaviour
{
    [SerializeField]
    private GameRule _gameRule;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Cell _cellPrefab;

    public Cell[,] Cells;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        var position = new Vector2(_canvas.pixelRect.width * 0.5f, _canvas.pixelRect.height * 0.5f);

        var cellRectTransform = _cellPrefab.GetComponent<RectTransform>();
        var width = cellRectTransform.rect.width;
        var height = cellRectTransform.rect.height;

        var columnCount = _gameRule.ColumnCount;
        var rowCount = _gameRule.RowCount;

        Cells = new Cell[columnCount, rowCount];
        for(var c = 0; c < columnCount; c++) 
        {   
            for(var r = 0; r < rowCount; r++) 
            {
                Cells[c, r] = Instantiate(_cellPrefab, new Vector2(position.x + c * width, position.y + r * height), Quaternion.identity, _canvas.transform);
            }
        }
    }
}
