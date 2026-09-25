public class GameContext
{
    public GameData GameData { get; }
    public EnemyRoster EnemyRoster { get; }
    public RoundTimer RoundTimer { get; }
    public RoundCounter RoundCounter { get; }
    public UIContext UI { get; }
    public BattleField BattleField { get; }


    public GameContext(GameData gameData, EnemyRoster enemyRoster)
    {
        GameData = gameData;
        EnemyRoster = enemyRoster;
        RoundTimer = new RoundTimer();
        RoundCounter = new RoundCounter();
        UI = new UIContext();
        BattleField = new BattleField();
    }
}
