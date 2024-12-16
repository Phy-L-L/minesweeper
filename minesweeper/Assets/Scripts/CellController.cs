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
    private List<int> _mineIndexes;

    // TODO : Debug
    private void Update()
    {
        if(Input.GetMouseButton(1))
        {
            foreach(var cell in _cellGenerator.Cells)
            {
                cell.OpenCell(cell.CellType);
            }
        }
    }

    public void OnClickCell(Cell cell)
    {
        // 最初の操作で地雷を踏まないようにするための実装
        if(cell.CellType == Cell.CellCategory.FirstTimeEmpty)
        {
            SetupMine(cell);
        }

        OpenCell(cell);
    }

    private void SetupMine(Cell cellSelectedInFirst)
    {
        _cellGenerator.Cells.ForEach(x => x.CellType = Cell.CellCategory.Empty);
        var mineCount = _gameRule.MineCount;

        while (mineCount > 0)
        {
            var index = Random.Range(0, _cellGenerator.Cells.Count);
            var cell = _cellGenerator.Cells.ElementAt(index);

            // もし最初に選択したCellの場合または既に地雷に指定したCellには設置しない
            if (cellSelectedInFirst == cell || cell.CellType == Cell.CellCategory.Mine)
            {
                continue;
            }

            cell.CellType = Cell.CellCategory.Mine;
            _mineIndexes.Add(index);
            --mineCount;
        }

        SetupCellType();
    }

    private void SetupCellType()
    {
        var columnCount = _gameRule.ColumnCount;
        var rowCount = _gameRule.RowCount;

        foreach(var mineIndex in _mineIndexes) 
        {
            int? upperLeft = mineIndex - columnCount - 1;
            int? upperCenter = mineIndex - columnCount;
            int? upperRight = mineIndex - columnCount + 1;

            int? bottomLeft = mineIndex + columnCount - 1;
            int? bottomCenter = mineIndex + columnCount;
            int? bottomRight = mineIndex + columnCount + 1;

            int? left = mineIndex - 1;
            int? right = mineIndex + 1;

            var coordinates = new List<int?> { upperLeft, upperCenter, upperRight, bottomLeft, bottomCenter, bottomRight, left, right };
            var cellCount = _cellGenerator.Cells.Count;

            IEnumerable<int?> ie = coordinates.Select(x =>
            {
                if(x < 0 || x > cellCount) { x = null; }
                // 最左列には左にCellが存在しないためnullとする
                if(mineIndex % columnCount == 0) { x = null; }
                // 最右列には右にCellが存在しないためnullとする
                if(mineIndex % columnCount == columnCount - 1) { x = null; }

                return x;
            });

            foreach (var coordinate in ie)
            {
                if(!coordinate.HasValue) { continue; }

                var cellType = _cellGenerator.Cells[coordinate.Value].CellType;

                if (cellType == Cell.CellCategory.Mine)
                {
                    continue;
                }

                _cellGenerator.Cells[coordinate.Value].CellType = (Cell.CellCategory)((int)++cellType);
            }
        }
    }

    private void OpenCell(Cell cell)
    {
        var cellType = cell.CellType;
        cell.OpenCell(cellType);

        if (cellType == Cell.CellCategory.Mine)
        {
            Debug.LogWarning("GameOver");
        }

        //if (cellType == Cell.CellCategory.Empty)
        //{
        //    var emptyIndex = _cellGenerator.Cells.FindIndex(x => x == cell);

        //    var columnCount = _gameRule.ColumnCount;
        //    var rowCount = _gameRule.RowCount;

        //    int? upperLeft = emptyIndex - columnCount - 1;
        //    int? upperCenter = emptyIndex - columnCount;
        //    int? upperRight = emptyIndex - columnCount + 1;
        //    if (upperLeft.Value < 0) { upperLeft = null; }
        //    if (upperCenter.Value < 0) { upperCenter = null; }
        //    if (upperRight.Value < 0) { upperRight = null; }

        //    int? bottomLeft = emptyIndex + columnCount - 1;
        //    int? bottomCenter = emptyIndex + columnCount;
        //    int? bottomRight = emptyIndex + columnCount + 1;
        //    var cellCount = _cellGenerator.Cells.Count;
        //    if (bottomLeft.Value > cellCount) { bottomLeft = null; }
        //    if (bottomCenter.Value > cellCount) { bottomCenter = null; }
        //    if (bottomRight.Value > cellCount) { bottomRight = null; }

        //    int? left = emptyIndex - 1;
        //    int? right = emptyIndex + 1;
        //    // 最左列には左にCellが存在しないためnullとする
        //    if (emptyIndex % columnCount == 0) { left = null; }
        //    // 最右列には右にCellが存在しないためnullとする
        //    if (emptyIndex % columnCount == columnCount - 1) { right = null; }

        //    var coordinates = new List<int?> { upperLeft, upperCenter, upperRight, bottomLeft, bottomCenter, bottomRight, left, right };

        //    foreach (var coordinate in coordinates)
        //    {
        //        if (!coordinate.HasValue) { continue; }

        //        if (_cellGenerator.Cells[coordinate.Value].CellType == Cell.CellCategory.Empty)
        //        {
        //            OpenCell(_cellGenerator.Cells[coordinate.Value]);
        //        }
        //    }
        //}
    }
}
