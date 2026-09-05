public class GameContext
{
    public TitleScreen TitleScreen { get; }
    public PlayerStateMachine PlayerStateMachine { get; }
    public EnemyStateMachine EnemyStateMachine { get; } 

    public GameContext(TitleScreen titleScreen, PlayerStateMachine playerStateMachine, EnemyStateMachine enemyStateMachine)
    {
        TitleScreen = titleScreen;
        PlayerStateMachine = playerStateMachine;
        EnemyStateMachine = enemyStateMachine;
    }
}
