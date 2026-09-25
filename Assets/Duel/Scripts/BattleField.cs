using UnityEngine;

public class BattleField
{
    public readonly Vector3 PlayerInitialPosition = new(-5f, 0f, 0f);
    public readonly Vector3 EnemyInitialPosition = new(5f, 0f, 0f);

    public readonly Vector3 LeftLimit = new(-8.3f, 0f, 0f);
    public readonly Vector3 RightLimit = new(8.3f, 0f, 0f);

    public Character Player { get; }
    public Character Enemy { get; }
    public CharacterStateMachine PlayerStateMachine { get; }
    public EnemyStateMachine EnemyStateMachine { get; }


    public BattleField()
    {
        Player = GameObject.FindAnyObjectByType<Player>();
        Enemy = GameObject.FindAnyObjectByType<Enemy>();
        PlayerStateMachine = GameObject.FindAnyObjectByType<PlayerStateMachine>();
        EnemyStateMachine = GameObject.FindAnyObjectByType<EnemyStateMachine>();
    }
}
