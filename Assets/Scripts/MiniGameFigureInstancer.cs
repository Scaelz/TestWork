using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = CustomGrid.Grid;
public interface IFigureInstancer
{
    void Instantiate();
}
public class MiniGameFigureInstancer : IFigureInstancer
{
    private FigureData _figureData;
    private Grid _grid;

    public MiniGameFigureInstancer(FigureData figureData, Grid grid)
    {
        _figureData = figureData;
        _grid = grid;
    }

    public void Instantiate()
    {
        for (int i = 0; i < _figureData.FigurePlaces.Length; i++)
        {
            var figurePlaces = _figureData.FigurePlaces[i];
            var prefab = figurePlaces.FigurePrefab;
            var coordinates = figurePlaces.CellCoordinates;
            var owner = figurePlaces.Owner;
            for (int j = 0; j < coordinates.Length; j++)
            {
                var spawnPosition = _grid.GetWorldPostion(coordinates[j].x, coordinates[j].y);
                var figureGO = GameObject.Instantiate(prefab, spawnPosition, Quaternion.identity);
                IFigure figure = figureGO.GetComponent<IFigure>();
                figure.Initialize(owner, _figureData.PickColor(owner));
                var cell = _grid.GetValue(coordinates[j].x, coordinates[j].y);
                cell.Figure = figure;
            }
        }
    }
}
