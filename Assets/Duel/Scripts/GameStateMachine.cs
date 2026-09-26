using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    [SerializeField] GameData gameData;
    [SerializeField] CharacterData playerCharacterData;
    [SerializeField] EnemyRoster enemyRoster;
    GameContext context;
    IGameState current;


    void Start()
    {
        context = new GameContext(gameData, playerCharacterData, enemyRoster);
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
