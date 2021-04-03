using EventBusSystem;
public interface IGameOverHandler : IGlobalSubscriber
{
    void GameOverHandler(GameOverArgs args);
}
