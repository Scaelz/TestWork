using EventBusSystem;

public interface IFigurePlacedHandler : IGlobalSubscriber
{
    void FigurePlacedHandler(FigureInteractionArgs args);
}
