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

        while (mineCount > 0)
        {
            var index = Random.Range(0, _cellGenerator.Cells.Count);

            var cell = _cellGenerator.Cells.ElementAt(index);
            // もし最初に選択したCellの場合または既に地雷に指定したCellには設置しない
            if (firstTimeCell == cell || cell.CellType == Cell.CellCategory.Mine)
            {
                continue;
            }

            cell.CellType = Cell.CellCategory.Mine;
            --mineCount;
        }

        SetupCellType();
    }

    private void SetupCellType()
    {
        var rowCount = _gameRule.RowCount;
        var columnCount = _gameRule.ColumnCount;

        for(var i = 0; i < rowCount; i++) 
        {   
            for(var j = 0; j < columnCount; j++) 
            {
                var coordinate = rowCount * i + j;
                var cell = _cellGenerator.Cells[coordinate];

                if (cell.CellType != Cell.CellCategory.Mine)
                {
                    continue;
                }

                int? upperLeft = coordinate - columnCount - 1;
                int? upperCenter = coordinate - columnCount;
                int? upperRight = coordinate - columnCount + 1;

                int? left = coordinate - 1;
                int? right = coordinate + 1;

                int? bottomLeft = coordinate + columnCount - 1;
                int? bottomCenter = coordinate + columnCount;
                int? bottomRight = coordinate + columnCount + 1;

                // 最上列には上にCellが存在しないためnullとする
                if (i == 0)
                {
                    upperLeft = null;
                    upperCenter = null;
                    upperRight = null;
                }

                // 最下列には下にCellが存在しないためnullとする
                if (i == rowCount - 1)
                {
                    bottomLeft = null;
                    bottomCenter = null;
                    bottomRight = null;
                }

                // 最左列には左にCellが存在しないためnullとする
                if(j == 0)
                {
                    left = null;
                }

                // 最右列には右にCellが存在しないためnullとする
                if(j == columnCount - 1)
                {
                    right = null;
                }

                var coordinates = new List<int?> { upperLeft, upperCenter, upperRight, left, right, bottomLeft, bottomCenter, bottomRight };
                // TODO : nullであるものをこの状態で省くと不具合の原因になる可能性がある
                var validCoordinates = coordinates.FindAll(x => x != null);
                foreach(var validCoordinate in validCoordinates)
                {
                    var cellType = _cellGenerator.Cells[validCoordinate.Value].CellType;
                    cellType = (Cell.CellCategory)((int)_cellGenerator.Cells[validCoordinate.Value].CellType++);
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
