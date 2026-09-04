using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    GameContext context;
    IGameState current;
    bool isStateChanging;


    void Start()
    {
        var titleScreen = FindAnyObjectByType<TitleScreen>();
        var player = FindAnyObjectByType<PlayerStateMachine>();
        var enemy = FindAnyObjectByType<Enemy>();
        
        context = new GameContext(titleScreen, player, enemy);
        ChangeState(new GameState_Boot(this, context));
    }

    void Update()
    {
        current?.OnUpdate();
    }

    public void ChangeState(IGameState nextState)
    {
        current?.OnExit();
        current = nextState;
        current.OnEnter();
    }
}
