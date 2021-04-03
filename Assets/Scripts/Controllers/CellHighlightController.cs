using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBusSystem;

public class CellHighlightController : ICleanupable, IFigurePlacedHandler, IFigurePickedUpHandler
{
    private CellData _cellData;
    private IPossiblemoveChecker _moveChecker;
    private List<ICell> _possibleMoves;

    public CellHighlightController(CellData cellData, IPossiblemoveChecker moveChecker)
    {
        _possibleMoves = new List<ICell>();
        _cellData = cellData;
        _moveChecker = moveChecker;
        EventBus.Subscribe(this);
    }

    public void RestoreCellsDefaultColor()
    {
        for (int i = 0; i < _possibleMoves.Count; i++)
        {
            _possibleMoves[i].SetColor(_possibleMoves[i].DefaultColor);
        }
    }

    public void ColorApprovedCells(Color color)
    {
        for (int i = 0; i < _possibleMoves.Count; i++)
        {
            _possibleMoves[i].SetColor(color);
        }
    }

    public void FigurePickedUpHandler(FigureInteractionArgs args)
    {
        _possibleMoves = _moveChecker.GetPossibleMoves(args.Figure, args.StartCell);
        ColorApprovedCells(_cellData.CanMove);
    }

    public void FigurePlacedHandler(FigureInteractionArgs args)
    {
        RestoreCellsDefaultColor();
        _possibleMoves.Clear();
    }

    public void Cleanup()
    {
        EventBus.Unsubscribe(this);
    }
}
