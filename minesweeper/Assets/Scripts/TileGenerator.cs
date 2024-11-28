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
  
    [SerializeField]
    private float _tileSize = 20f;
    public List<Tile> Tiles = new List<Tile>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        var position = new Vector2(_canvas.pixelRect.width * 0.5f, _canvas.pixelRect.height * 0.5f);

        for(int i = 0; i < _gameManager.RowCount; i++) 
        {   
            for(int j = 0; j < _gameManager.ColumnCount; j++) 
            {
                Tiles.Add(Instantiate(_tilePrefab, new Vector2(position.x + i * _tileSize, position.y + j * _tileSize), Quaternion.identity, _canvas.transform));
            }
        }
    }
}
