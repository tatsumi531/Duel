namespace Duel.GameStates
{
    public interface IGameState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}
