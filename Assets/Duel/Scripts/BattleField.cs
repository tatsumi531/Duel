using UnityEngine;

public class BattleField
{
    public readonly Vector3 PlayerInitialPosition = new(-5f, 0f, 0f);
    public readonly Vector3 EnemyInitialPosition = new(5f, 0f, 0f);

    public readonly Vector3 LeftLimit = new(-8.3f, 0f, 0f);
    public readonly Vector3 RightLimit = new(8.3f, 0f, 0f);

    public CharacterStateMachine PlayerStateMachine { get; }
    public CharacterStateMachine EnemyStateMachine { get; }


    public BattleField()
    {
        PlayerStateMachine = GameObject.FindAnyObjectByType<PlayerStateMachine>();
        EnemyStateMachine = GameObject.FindAnyObjectByType<CharacterStateMachine>();
    }
}
