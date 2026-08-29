using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    GameContext context;
    IGameState current;
    bool isStateChanging;


    void Start()
    {
        context = new GameContext();
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
