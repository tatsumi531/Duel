using UnityEngine;

public class GameContext
{
    public GameData GameData { get; }
    public RoundTimer RoundTimer { get; }
    public RoundCounter RoundCounter { get; }
    public TitleScreen TitleScreen { get; }
    public IntroductionScreen IntroductionScreen { get; }
    public ResultScreen ResultScreen { get; }
    public BattleField BattleField { get; }
    public PlayerStateMachine PlayerStateMachine { get; }
    public CharacterStateMachine EnemyStateMachine { get; }


    public GameContext(GameData gameData)
    {
        GameData = gameData;
        RoundTimer = new RoundTimer();
        RoundCounter = new RoundCounter();
        TitleScreen = GameObject.FindAnyObjectByType<TitleScreen>();
        IntroductionScreen = GameObject.FindAnyObjectByType<IntroductionScreen>();
        ResultScreen = GameObject.FindAnyObjectByType<ResultScreen>();
        BattleField = new BattleField();
        PlayerStateMachine = GameObject.FindAnyObjectByType<PlayerStateMachine>();
        EnemyStateMachine = GameObject.FindAnyObjectByType<Enemy>()?.GetComponent<CharacterStateMachine>();
    }
}
