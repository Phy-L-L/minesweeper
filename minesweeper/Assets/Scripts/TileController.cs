using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileController : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private TileGenerator _tileGenerator;

    public void OpenTile(Tile tile)
    {
        // 最初の操作で地雷を踏まないようにするための実装
        if(tile.TileType == Tile.TileCategory.FirstTimeEmpty)
        {
            SetupMine(tile);
        }

        MineDecision(tile);
    }

    private void SetupMine(Tile firstTimeTile)
    {
        _tileGenerator.Tiles.ForEach(x => x.TileType = Tile.TileCategory.Empty);

        var mineCount = _gameManager.MineCount;
        List<Tile> mineTiles = new();

        while (true)
        {
            var index = Random.Range(0, _tileGenerator.Tiles.Count);
            var tile = _tileGenerator.Tiles.ElementAt(index);

            // もし最初に選択したTileの場合は地雷を設置しない
            if (firstTimeTile == tile)
            {
                Debug.LogWarning("初手で地雷を踏んだレアケース");
                continue;
            }

            mineTiles.Add(tile);

            if (tile.TileType != Tile.TileCategory.Mine)
            {
                tile.TileType = Tile.TileCategory.Mine;
                --mineCount;
            }

            if (mineCount <= 0)
            {
                SetupTileType(mineTiles);
                break;
            }
        }
    }

    private void SetupTileType(List<Tile> mineTiles)
    {
        var rowCount = _gameManager.RowCount;
        var columnCount = _gameManager.ColumnCount;

        for(var i = 0; i < rowCount; i++) 
        {   
            for(var j = 0; j < columnCount; j++) 
            {
                var coordinate = rowCount * i + j;
                var tile = _tileGenerator.Tiles[coordinate];

                if (tile.TileType == Tile.TileCategory.Mine)
                {
                    // TODO : 四隅ではない場合
                    var upperLeft = coordinate - columnCount - 1;
                    var upperCenter = coordinate - columnCount;
                    var upperRight = coordinate - columnCount + 1;
                    var left = coordinate - 1;
                    var right = coordinate + 1;
                    var bottomLeft = coordinate + columnCount - 1;
                    var bottomCenter = coordinate + columnCount;
                    var bottomRight = coordinate + columnCount + 1;

                    var coordinates = new List<int> { upperLeft, upperCenter, upperRight, left, right, bottomLeft, bottomCenter, bottomRight };
                    var validCoordinates = coordinates.FindAll(x => x >= 0 && x < _tileGenerator.Tiles.Count);
                    foreach(var validCoordinate in validCoordinates)
                    {
                        var tileType = _tileGenerator.Tiles[validCoordinate].TileType;

                        switch (tileType)
                        {
                            case Tile.TileCategory.Empty :
                                break;
                            case Tile.TileCategory.One :
                            case Tile.TileCategory.Two :
                            case Tile.TileCategory.Three :
                            case Tile.TileCategory.Four :
                            case Tile.TileCategory.Five :
                            case Tile.TileCategory.Six :
                            case Tile.TileCategory.Seven :
                            // TODO : 怪しいかも？
                            case Tile.TileCategory.Eight :
                                tileType = (Tile.TileCategory)((int)_tileGenerator.Tiles[validCoordinate].TileType++);
                                break;
                            case Tile.TileCategory.Mine :
                                break;
                            case Tile.TileCategory.FirstTimeEmpty :
                                Debug.LogWarning("通るはずのない処理FirstTimeEmpty");
                                break;
                        }
                    }
                }
            }
        }
    }

    private void MineDecision(Tile tile)
    {
        if (tile.TileType == Tile.TileCategory.Mine)
        {
            // TODO : GameOver処理
            Debug.LogWarning("GameOver");
            return;
        }

        tile.ChangeTileType(tile.TileType);
    }
}
