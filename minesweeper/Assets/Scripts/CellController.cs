using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CellController : MonoBehaviour
{
    [SerializeField]
    private GameRule _gameRule;
    [SerializeField]
    private CellGenerator _cellGenerator;
    [SerializeField]
    private List<Cell> _mineCells;

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
            _mineCells.Add(cell);
            --mineCount;
        }

        // TODO: 削除する
        Debug.LogWarning($"_mineCells: {_mineCells.Count}");
        SetupCellType();
    }

    private void SetupCellType()
    {
        foreach (var mineCell in _mineCells) 
        {

        }

        var rowCount = _gameRule.RowCount;
        var columnCount = _gameRule.ColumnCount;

        for(var i = 0; i < rowCount; i++) 
        {   
            for(var j = 0; j < columnCount; j++) 
            {
                var coordinate = rowCount * i + j;
                //var cell = _cellGenerator.Cells[coordinate];

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

                
                var coordinates = new List<int?> { upperLeft, upperCenter, upperRight, bottomLeft, bottomCenter, bottomRight, left, right };
                coordinates = coordinates.FindAll(x =>  x >= 0 && x < _cellGenerator.Cells.Count);
                for (var k = 0; k < coordinates.Count; k++)
                {
                    var x = coordinates[k];
                    if(!x.HasValue)
                    {
                        continue;
                    }

                    Debug.Log(x.Value);
                    var cellType = _cellGenerator.Cells[x.Value].CellType;
                    if(cellType == Cell.CellCategory.Mine || cellType == Cell.CellCategory.Flag)
                    {
                        continue;
                    }

                    cellType = (Cell.CellCategory)((int)_cellGenerator.Cells[x.Value].CellType++);
                }
            }
        }
    }

    private void OpenCell(Cell cell)
    {
        cell.ChangeCellType(cell.CellType);

        if (cell.CellType == Cell.CellCategory.Mine)
        {
            Debug.LogWarning("GameOver");
        }
    }
}
