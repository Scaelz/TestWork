using EventBusSystem;

public class TurnController : ICleanupable, IFigurePlacedHandler
{
    public Turn CurrentTurn { get; private set; }

    public TurnController()
    {
        CurrentTurn = Turn.Pale;
        EventBus.Subscribe(this);
    }

    public void FigurePlacedHandler(FigureInteractionArgs args)
    {
        if (!args.isCanceled)
        {
            SwapTurn();
        }
    }

    private void SwapTurn()
    {
        Turn nextTurn = Turn.Pale;
        if (CurrentTurn == Turn.Pale)
        {
            nextTurn = Turn.Dark;
        }
        CurrentTurn = nextTurn;
        EventBus.RaiseEvent<ITurnChangedHandler>(x => x.OnTurnChangedHandler(nextTurn));
    }

    public void Cleanup()
    {
        EventBus.Unsubscribe(this);
    }
}
