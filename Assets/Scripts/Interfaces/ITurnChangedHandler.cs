using EventBusSystem;

public interface ITurnChangedHandler : IGlobalSubscriber
{
    void OnTurnChangedHandler(Turn turn);
}
