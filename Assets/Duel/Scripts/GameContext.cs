public class GameContext
{
    public GameData GameData { get; }
    public CharacterData PlayerCharacterData { get; }
    public EnemyRoster EnemyRoster { get; }
    public Character Player { get; }
    public Character Enemy { get; }
    public CharacterStateMachine PlayerStateMachine { get; }
    public CharacterStateMachine EnemyStateMachine { get; }
    public RoundTimer RoundTimer { get; }
    public RoundCounter RoundCounter { get; }
    public UIContext UI { get; }
    public BattleField BattleField { get; }


    public GameContext(GameData gameData, CharacterData playerCharacterData, EnemyRoster enemyRoster, Character player, Character enemy)
    {
        GameData = gameData;
        PlayerCharacterData = playerCharacterData;
        EnemyRoster = enemyRoster;

        Player = player;
        Enemy = enemy;
        PlayerStateMachine = Player.GetComponent<CharacterStateMachine>();
        EnemyStateMachine = Enemy.GetComponent<CharacterStateMachine>();

        RoundTimer = new RoundTimer();
        RoundCounter = new RoundCounter();
        UI = new UIContext();
        BattleField = new BattleField();
    }
}
