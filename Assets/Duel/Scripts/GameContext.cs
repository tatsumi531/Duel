public class GameContext
{
    public GameData GameData { get; }
    public RoundTimer RoundTimer { get; }
    public RoundCounter RoundCounter { get; }
    public UIContext UI { get; }
    public BattleField BattleField { get; }


    public GameContext(GameData gameData)
    {
        GameData = gameData;
        RoundTimer = new RoundTimer();
        RoundCounter = new RoundCounter();
        UI = new UIContext();
        BattleField = new BattleField();
    }
}
