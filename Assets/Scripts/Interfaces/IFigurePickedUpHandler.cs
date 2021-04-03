using EventBusSystem;

public interface IFigurePickedUpHandler : IGlobalSubscriber
{
    void FigurePickedUpHandler(FigureInteractionArgs args);
}
