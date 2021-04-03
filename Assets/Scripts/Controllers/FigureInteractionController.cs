using UnityEngine;
using Grid = CustomGrid.Grid;
using EventBusSystem;
using System.Collections.Generic;

public class FigureInteractionController : IExecuteable, ICleanupable, IUserInputHandler
{
    private Grid _grid;
    private ICell _returnCell;
    public IFigure CurrentFigure { get; private set; }
    private IPossiblemoveChecker _moveChecker;
    private List<ICell> _possibleMoves;

    public FigureInteractionController(Grid grid, IPossiblemoveChecker moveChecker)
    {
        _possibleMoves = new List<ICell>();
        _grid = grid;
        _moveChecker = moveChecker;
        EventBus.Subscribe(this);
    }

    public void Execute(float deltaTime)
    {
        if (CurrentFigure != null)
        {
            var newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CurrentFigure.transform.position = new Vector3(newPosition.x,
                                                           newPosition.y,
                                                           CurrentFigure.transform.position.z);
        }
    }

    public void PrimaryActionHandler(UserInputArgs args)
    {
        var cell = _grid.GetValue(args.MousePosition);
        if (cell != null)
        {
            if (cell.Figure != null && CurrentFigure == null)
            {
                _possibleMoves = _moveChecker.GetPossibleMoves(cell.Figure, cell);
                if (_possibleMoves.Count > 0)
                {
                    PickUpFigure(cell);
                }
            }
            else if (cell.Figure == null && CurrentFigure != null)
            {
                if (_possibleMoves.Contains(cell))
                {
                    PlaceFigure(cell);
                }
            }
        }
    }

    void PickUpFigure(ICell fromCell)
    {
        CurrentFigure = fromCell.Figure;
        fromCell.Figure = null;
        var interactionArgs = new FigureInteractionArgs()
        {
            Figure = CurrentFigure,
            StartCell = fromCell,
        };
        _returnCell = fromCell;
        EventBus.RaiseEvent<IFigurePickedUpHandler>
                            (x => x.FigurePickedUpHandler(interactionArgs));
    }

    void PlaceFigure(ICell cell)
    {
        cell.Figure = CurrentFigure;
        CurrentFigure.transform.position = cell.Coordinates;
        ReleaseFigure();
        var interactionArgs = new FigureInteractionArgs()
        {
            Figure = cell.Figure,
            PlacementCell = cell,
        };
        EventBus.RaiseEvent<IFigurePlacedHandler>
                            (x => x.FigurePlacedHandler(interactionArgs));
    }

    public void ReleaseFigure()
    {
        CurrentFigure = null;
        _possibleMoves.Clear();
    }

    public void SecondaryActionHandler(UserInputArgs args)
    {
        if (CurrentFigure == null)
        {
            return;
        }
        CurrentFigure.transform.position = _returnCell.Coordinates;
        _returnCell.Figure = CurrentFigure;
        ReleaseFigure();
        var interactionArgs = new FigureInteractionArgs()
        {
            Figure = CurrentFigure,
            PlacementCell = _returnCell,
            isCanceled = true,
        };
        EventBus.RaiseEvent<IFigurePlacedHandler>(x => x.FigurePlacedHandler(interactionArgs));
    }

    public void Cleanup()
    {
        EventBus.Unsubscribe(this);
    }
}
