using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private GameObject _tilePrefab;

    [SerializeField]
    private int _rowCount = 9;
    [SerializeField]
    private int _columnCount = 9;
    [SerializeField]
    private float _tileSize = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var position = new Vector2(_canvas.pixelRect.width * 0.5f, _canvas.pixelRect.height * 0.5f);

        for(int i = 0; i < _rowCount; i++) 
        {   
            for(int j = 0; j < _columnCount; j++) 
            {
                GameObject.Instantiate(_tilePrefab, new Vector2(position.x + i * _tileSize, position.y + j * _tileSize), Quaternion.identity, _canvas.transform);
            }
        }
    }
}
