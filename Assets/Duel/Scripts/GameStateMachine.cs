using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    [SerializeField] GameData gameData;
    GameContext context;
    IGameState current;


    void Start()
    {
        context = new GameContext(gameData);
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
