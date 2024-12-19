using UnityEngine;

public class CellController : MonoBehaviour
{
    [SerializeField]
    private GameRule _gameRule;
    [SerializeField]
    private CellGenerator _cellGenerator;

    public void OnClickCell(Cell cell)
    {
        // 最初の操作で地雷を踏まないようにするための実装
        if(cell.CellType == Cell.CellCategory.Empty)
        {
            SetupMine(cell);
        }

        OpenCell(cell);
    }

    private void SetupMine(Cell cellSelectedInFirst)
    {
        var columnCount = _gameRule.ColumnCount;
        var rowCount = _gameRule.RowCount;

        for(var c = 0; c < columnCount; c++) 
        {
            for(var r = 0; r < rowCount; r++)
            {
                _cellGenerator.Cells[c, r].CellType = Cell.CellCategory.Zero;
            }
        }

        var mineCount = _gameRule.MineCount;

        while (mineCount > 0)
        {
            var columnIndex = Random.Range(0, columnCount);
            var rowIndex = Random.Range(0, rowCount);

            var cell = _cellGenerator.Cells[columnIndex, rowIndex];

            // もし最初に選択したCellの場合または既に地雷に指定したCellには設置しない
            if (cellSelectedInFirst == cell || cell.CellType == Cell.CellCategory.Mine)
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
        var columnCount = _gameRule.ColumnCount;
        var rowCount = _gameRule.RowCount;

        for(var c = 0; c < columnCount; c++) 
        {
            for(var r = 0; r < rowCount; r++)
            {
                var cell = _cellGenerator.Cells[c, r];

                // 地雷のときのみ継続して処理をする
                if (cell.CellType != Cell.CellCategory.Mine)
                {
                    continue;
                }

                bool TryIncrementMineNumber(int columnIndex, int rowIndex)
                {
                    var cell = _cellGenerator.Cells[columnIndex, rowIndex];
                    // 地雷の場合は処理を行わない
                    if(cell.CellType == Cell.CellCategory.Mine) { return false; }

                    cell.CellType = (Cell.CellCategory)((int)++cell.CellType);
                    return true;
                }

                var isCellExistTopLeft = c - 1 >= 0 && r - 1 >= 0;
                var isCellExistTopCenter = c - 1 >= 0;
                var isCellExistTopRight = c - 1 >= 0 && r + 1 < rowCount;
                if(isCellExistTopLeft) { TryIncrementMineNumber(c - 1, r - 1); }
                if(isCellExistTopCenter) { TryIncrementMineNumber(c - 1, r); }
                if(isCellExistTopRight) { TryIncrementMineNumber(c - 1, r + 1); }

                var isCellExistBottomLeft = c + 1 < columnCount && r - 1 >= 0;
                var isCellExistBottomCenter = c + 1 < columnCount;
                var isCellExistBottomRight = c + 1 < columnCount && r + 1 < rowCount;
                if(isCellExistBottomLeft) { TryIncrementMineNumber(c + 1, r - 1); }
                if(isCellExistBottomCenter) { TryIncrementMineNumber(c + 1, r); }
                if(isCellExistBottomRight) { TryIncrementMineNumber(c + 1, r + 1); }

                var isCellExistLeft = r - 1 >= 0;
                var isCellExistRight = r + 1 < rowCount;
                if(isCellExistLeft) { TryIncrementMineNumber(c, r - 1); }
                if(isCellExistRight) { TryIncrementMineNumber(c, r + 1); }
            }
        }
    }

    private void OpenCell(Cell cell)
    {
        var cellType = cell.CellType;
        cell.OpenCell(cellType);

        if(cellType == Cell.CellCategory.Zero)
        {
            OpenZeroSurroundingCell(cell);
            OpenEmptySurroundingCell();
        }

        if (cellType == Cell.CellCategory.Mine)
        {
            Debug.LogWarning("GameOver");
        }

        if (IsClearedGame())
        {
            Debug.LogWarning("GameClear");
        }
    }

    private void OpenZeroSurroundingCell(Cell zeroCell)
    {
        var columnCount = _gameRule.ColumnCount;
        var rowCount = _gameRule.RowCount;

        for (var c = 0; c < columnCount; c++)
        {
            for (var r = 0; r < rowCount; r++)
            {
                if (zeroCell == _cellGenerator.Cells[c, r])
                {
                    OpenZeroSurroundingCell(c, r);
                }
            }
        }
    }

    private void OpenZeroSurroundingCell(int columnIndex, int rowIndex)
    {
        var columnCount = _gameRule.ColumnCount;
        var rowCount = _gameRule.RowCount;

        bool TryOpenZeroCell(int columnIndex, int rowIndex)
        {
            var cell = _cellGenerator.Cells[columnIndex, rowIndex];
            var cellType = cell.CellType;

            if (cellType != Cell.CellCategory.Zero) { return false; }

            cell.OpenCell(cellType);

            var isCellExistTopLeft = columnIndex - 1 >= 0 && rowIndex - 1 >= 0;
            var isCellExistTopCenter = columnIndex - 1 >= 0;
            var isCellExistTopRight = columnIndex - 1 >= 0 && rowIndex + 1 < rowCount;
            if (isCellExistTopLeft) { OpenZeroSurroundingCell(columnIndex - 1, rowIndex - 1); }
            if (isCellExistTopCenter) { OpenZeroSurroundingCell(columnIndex - 1, rowIndex); }
            if (isCellExistTopRight) { OpenZeroSurroundingCell(columnIndex - 1, rowIndex + 1); }

            var isCellExistBottomLeft = columnIndex + 1 < columnCount && rowIndex - 1 >= 0;
            var isCellExistBottomCenter = columnIndex + 1 < columnCount;
            var isCellExistBottomRight = columnIndex + 1 < columnCount && rowIndex + 1 < rowCount;
            if (isCellExistBottomLeft) { OpenZeroSurroundingCell(columnIndex + 1, rowIndex - 1); }
            if (isCellExistBottomCenter) { OpenZeroSurroundingCell(columnIndex + 1, rowIndex); }
            if (isCellExistBottomRight) { OpenZeroSurroundingCell(columnIndex + 1, rowIndex + 1); }

            var isCellExistLeft = rowIndex - 1 >= 0;
            var isCellExistRight = rowIndex + 1 < rowCount;
            if (isCellExistLeft) { OpenZeroSurroundingCell(columnIndex, rowIndex - 1); }
            if (isCellExistRight) { OpenZeroSurroundingCell(columnIndex, rowIndex + 1); }

            return true;
        }

        var isCellExistTopLeft = columnIndex - 1 >= 0 && rowIndex - 1 >= 0;
        var isCellExistTopCenter = columnIndex - 1 >= 0;
        var isCellExistTopRight = columnIndex - 1 >= 0 && rowIndex + 1 < rowCount;
        if (isCellExistTopLeft) { TryOpenZeroCell(columnIndex - 1, rowIndex - 1); }
        if (isCellExistTopCenter) { TryOpenZeroCell(columnIndex - 1, rowIndex); }
        if (isCellExistTopRight) { TryOpenZeroCell(columnIndex - 1, rowIndex + 1); }

        var isCellExistBottomLeft = columnIndex + 1 < columnCount && rowIndex - 1 >= 0;
        var isCellExistBottomCenter = columnIndex + 1 < columnCount;
        var isCellExistBottomRight = columnIndex + 1 < columnCount && rowIndex + 1 < rowCount;
        if (isCellExistBottomLeft) { TryOpenZeroCell(columnIndex + 1, rowIndex - 1); }
        if (isCellExistBottomCenter) { TryOpenZeroCell(columnIndex + 1, rowIndex); }
        if (isCellExistBottomRight) { TryOpenZeroCell(columnIndex + 1, rowIndex + 1); }

        var isCellExistLeft = rowIndex - 1 >= 0;
        var isCellExistRight = rowIndex + 1 < rowCount;
        if (isCellExistLeft) { TryOpenZeroCell(columnIndex, rowIndex - 1); }
        if (isCellExistRight) { TryOpenZeroCell(columnIndex, rowIndex + 1); }
    }

    private void OpenEmptySurroundingCell()
    {
        void OpenCell(int columnIndex, int rowIndex)
        { 
            var cell = _cellGenerator.Cells[columnIndex, rowIndex];
            var cellType = cell.CellType;
            cell.OpenCell(cellType);
        }

        var columnCount = _gameRule.ColumnCount;
        var rowCount = _gameRule.RowCount;

        for (var c = 0; c < columnCount; c++)
        {
            for (var r = 0; r < rowCount; r++)
            {
                var cell = _cellGenerator.Cells[c, r];
                if (cell.CellType != Cell.CellCategory.Empty) { continue; }
                
                var isCellExistTopLeft = c - 1 >= 0 && r - 1 >= 0;
                var isCellExistTopCenter = c - 1 >= 0;
                var isCellExistTopRight = c - 1 >= 0 && r + 1 < rowCount;
                if (isCellExistTopLeft) { OpenCell(c - 1, r - 1); }
                if (isCellExistTopCenter) { OpenCell(c - 1, r); }
                if (isCellExistTopRight) { OpenCell(c - 1, r + 1); }

                var isCellExistBottomLeft = c + 1 < columnCount && r - 1 >= 0;
                var isCellExistBottomCenter = c + 1 < columnCount;
                var isCellExistBottomRight = c + 1 < columnCount && r + 1 < rowCount;
                if (isCellExistBottomLeft) { OpenCell(c + 1, r - 1); }
                if (isCellExistBottomCenter) { OpenCell(c + 1, r); }
                if (isCellExistBottomRight) { OpenCell(c + 1, r + 1); }

                var isCellExistLeft = r - 1 >= 0;
                var isCellExistRight = r + 1 < rowCount;
                if (isCellExistLeft) { OpenCell(c, r - 1); }
                if (isCellExistRight) { OpenCell(c, r + 1); }
            }
        }
    }

    private bool IsClearedGame()
    {
        var columnCount = _gameRule.ColumnCount;
        var rowCount = _gameRule.RowCount;

        var isOpenedCellCount = 0;
        for (var c = 0; c < columnCount; c++)
        {
            for (var r = 0; r < rowCount; r++)
            {
                if (_cellGenerator.Cells[c, r].IsOpened)
                {
                    ++isOpenedCellCount;
                }
            }
        }

        var cellTotalCount = columnCount * rowCount;
        var mineCount = _gameRule.MineCount;
        var isClearedGame = (cellTotalCount - isOpenedCellCount) == mineCount;
        return isClearedGame;
    }
}
