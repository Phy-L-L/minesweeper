using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CellController : MonoBehaviour
{
    [SerializeField]
    private GameRule _gameRule;
    [SerializeField]
    private CellGenerator _cellGenerator;

    public void OnClickCell(Cell cell)
    {
        // 最初の操作で地雷を踏まないようにするための実装
        if(cell.CellType == Cell.CellCategory.FirstTimeEmpty)
        {
            SetupMine(cell);
        }

        OpenCell(cell);
    }

    private void SetupMine(Cell firstTimeCell)
    {
        _cellGenerator.Cells.ForEach(x => x.CellType = Cell.CellCategory.Empty);

        var mineCount = _gameRule.MineCount;
        List<Cell> mineCells = new();

        while (mineCount > 0)
        {
            var index = Random.Range(0, _cellGenerator.Cells.Count);
            var cell = _cellGenerator.Cells.ElementAt(index);

            // もし最初に選択したCellの場合は地雷を設置しない
            if (firstTimeCell == cell)
            {
                continue;
            }

            mineCells.Add(cell);

            if (cell.CellType != Cell.CellCategory.Mine)
            {
                cell.CellType = Cell.CellCategory.Mine;
                --mineCount;
            }
        }

        SetupCellType(mineCells);
    }

    private void SetupCellType(List<Cell> mineCells)
    {
        var rowCount = _gameRule.RowCount;
        var columnCount = _gameRule.ColumnCount;

        for(var i = 0; i < rowCount; i++) 
        {   
            for(var j = 0; j < columnCount; j++) 
            {
                var coordinate = rowCount * i + j;
                var cell = _cellGenerator.Cells[coordinate];

                if (cell.CellType == Cell.CellCategory.Mine)
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
                    var validCoordinates = coordinates.FindAll(x => x >= 0 && x < _cellGenerator.Cells.Count);
                    foreach(var validCoordinate in validCoordinates)
                    {
                        var cellType = _cellGenerator.Cells[validCoordinate].CellType;

                        switch (cellType)
                        {
                            case Cell.CellCategory.Empty :
                            case Cell.CellCategory.One :
                            case Cell.CellCategory.Two :
                            case Cell.CellCategory.Three :
                            case Cell.CellCategory.Four :
                            case Cell.CellCategory.Five :
                            case Cell.CellCategory.Six :
                            case Cell.CellCategory.Seven :
                            // TODO : 怪しいかも？
                            case Cell.CellCategory.Eight :
                                cellType = (Cell.CellCategory)((int)_cellGenerator.Cells[validCoordinate].CellType++);
                                break;
                            case Cell.CellCategory.Mine :
                                break;
                            case Cell.CellCategory.FirstTimeEmpty :
                                Debug.LogWarning("通るはずのない処理FirstTimeEmpty");
                                break;
                        }
                    }
                }
            }
        }
    }

    private void OpenCell(Cell cell)
    {
        if (cell.CellType == Cell.CellCategory.Mine)
        {
            // TODO : GameOver処理
            Debug.LogWarning("GameOver");
            return;
        }

        cell.ChangeCellType(cell.CellType);
    }
}
