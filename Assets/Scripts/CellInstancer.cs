using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = CustomGrid.Grid;
public class CellInstancer
{
    private Grid _grid;
    private CellData _cellData;
    public CellInstancer(Grid grid, CellData cellData)
    {
        _cellData = cellData;
        _grid = grid;
    }

    public void InstantiateCells()
    {
        for (int x = 0; x < _grid.Width; x++)
        {
            for (int y = 0; y < _grid.Height; y++)
            {
                var cellSizeOffset = new Vector2(_grid.CellSize, _grid.CellSize) * 0.5f;
                var worldPosition = _grid.GetWorldPostion(x, y);
                var cellGO = GameObject.Instantiate(_cellData.CellPrefab,
                                                    worldPosition,
                                                    Quaternion.identity);
                ICell cell = cellGO.GetComponent<ICell>();

                var coordinates = _grid.GetWorldPostion(x, y);
                var color = PickStandardColor(x, y);
                cell.Initialize(color, coordinates);
                _grid.SetValue(x, y, cell);
            }
        }
    }

    Color PickStandardColor(int x, int y)
    {
        if ((x + y) % 2 == 0)
        {
            return _cellData.Dark;
        }

        return _cellData.Pale;
    }
}
