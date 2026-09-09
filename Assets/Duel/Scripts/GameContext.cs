using UnityEngine;

public class GameContext
{
    public TitleScreen TitleScreen { get; }
    public IntroductionScreen IntroductionScreen { get; }
    public ResultScreen ResultScreen { get; }
    public PlayerStateMachine PlayerStateMachine { get; }
    public EnemyStateMachine EnemyStateMachine { get; } 


    public GameContext()
    {
        TitleScreen = GameObject.FindAnyObjectByType<TitleScreen>();
        IntroductionScreen = GameObject.FindAnyObjectByType<IntroductionScreen>();
        ResultScreen = GameObject.FindAnyObjectByType<ResultScreen>();
        PlayerStateMachine = GameObject.FindAnyObjectByType<PlayerStateMachine>();
        EnemyStateMachine = GameObject.FindAnyObjectByType<EnemyStateMachine>();
    }
}
