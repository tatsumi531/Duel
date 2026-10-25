using UnityEngine;
using Duel.Characters;
using Duel.GameStates;
using Duel.UI;

namespace Duel
{
    public class GameStateMachine : MonoBehaviour
    {
        [SerializeField] GameData gameData;
        [SerializeField] CharacterData playerCharacterData;
        [SerializeField] EnemyRoster enemyRoster;
        [SerializeField] Character player;
        [SerializeField] Character enemy;
        [SerializeField] UIRoot ui;
        [SerializeField] BattleField battleField;
        GameContext context;
        IGameState current;


        void Start()
        {
            context = new GameContext(gameData, playerCharacterData, enemyRoster, player, enemy, ui, battleField);
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
}
