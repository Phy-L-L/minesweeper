using UnityEngine;
using System.Collections.Generic;

public class CellGenerator : MonoBehaviour
{
    [SerializeField]
    private GameRule _gameRule;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Cell _cellPrefab;

    public List<Cell> Cells = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        var position = new Vector2(_canvas.pixelRect.width * 0.5f, _canvas.pixelRect.height * 0.5f);

        var cellRectTransform = _cellPrefab.GetComponent<RectTransform>();
        var width = cellRectTransform.rect.width;
        var height = cellRectTransform.rect.height;

        for(var i = 0; i < _gameRule.RowCount; i++) 
        {   
            for(var j = 0; j < _gameRule.ColumnCount; j++) 
            {
                Cells.Add(Instantiate(_cellPrefab, new Vector2(position.x + i * width, position.y + j * height), Quaternion.identity, _canvas.transform));
            }
        }
    }
}
