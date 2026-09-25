using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    [SerializeField] GameData gameData;
    [SerializeField] EnemyRoster enemyRoster;
    GameContext context;
    IGameState current;


    void Start()
    {
        context = new GameContext(gameData, enemyRoster);
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
