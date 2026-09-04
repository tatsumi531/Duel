public class GameContext
{
    public TitleScreen TitleScreen { get; }
    public PlayerStateMachine PlayerStateMachine { get; }
    public Enemy Enemy { get; } 

    public GameContext(TitleScreen titleScreen, PlayerStateMachine playerStateMachine, Enemy enemy)
    {
        TitleScreen = titleScreen;
        PlayerStateMachine = playerStateMachine;
        Enemy = enemy;
    }
}
