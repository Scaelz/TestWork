using System.Collections.Generic;
using EventBusSystem;

public class GameOverController : ICleanupable, IFigurePlacedHandler, ITurnChangedHandler
{
    private Turn _currentTurn;

    private List<ICell> _paleTargetCells;
    private List<ICell> _darkTargetCells;

    public GameOverController(List<ICell> paleTargetCells, List<ICell> darkTargetCells)
    {
        _paleTargetCells = paleTargetCells;
        _darkTargetCells = darkTargetCells;
        EventBus.Subscribe(this);
    }

    public void Cleanup()
    {
        EventBus.Unsubscribe(this);
    }

    public void FigurePlacedHandler(FigureInteractionArgs args)
    {
        if (args.isCanceled)
        {
            return;
        }
        var figureOwner = args.Figure.Owner;
        if (figureOwner == FigureOwner.Dark && _currentTurn == Turn.Dark)
        {
            for (int i = 0; i < _darkTargetCells.Count; i++)
            {
                var cellFigure = _darkTargetCells[i].Figure;
                if (cellFigure == null || cellFigure.Owner == FigureOwner.Pale)
                {
                    return;
                }
            }
            EventBus.RaiseEvent<IGameOverHandler>(x => x.GameOverHandler(new GameOverArgs()
            {
                IsPaleWon = false,
            }));
        }
        else if (figureOwner == FigureOwner.Pale && _currentTurn == Turn.Pale)
        {
            for (int i = 0; i < _paleTargetCells.Count; i++)
            {
                var cellFigure = _paleTargetCells[i].Figure;
                if (cellFigure == null || cellFigure.Owner == FigureOwner.Dark)
                {
                    return;
                }
            }
            EventBus.RaiseEvent<IGameOverHandler>(x => x.GameOverHandler(new GameOverArgs()
            {
                IsPaleWon = true,
            }));
        }
    }

    public void OnTurnChangedHandler(Turn turn)
    {
        _currentTurn = turn;
    }
}
