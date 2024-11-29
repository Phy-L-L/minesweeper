using UnityEngine;
using System.Collections.Generic;

public class TileGenerator : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Tile _tilePrefab;

    public List<Tile> Tiles = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // TODO : à íuí≤êÆ
        var position = new Vector2(_canvas.pixelRect.width * 0.5f, _canvas.pixelRect.height * 0.5f);

        var tileRectTransform = _tilePrefab.GetComponent<RectTransform>();
        var width = tileRectTransform.rect.width;
        var height = tileRectTransform.rect.height;

        for(var i = 0; i < _gameManager.RowCount; i++) 
        {   
            for(var j = 0; j < _gameManager.ColumnCount; j++) 
            {
                Tiles.Add(Instantiate(_tilePrefab, new Vector2(position.x + i * width, position.y + j * height), Quaternion.identity, _canvas.transform));
            }
        }
    }
}
