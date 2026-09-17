using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CharacterStateMachine))]
public class CharacterAnimationEventReceiver : MonoBehaviour
{
    Character character;
    CharacterStateMachine stateMachine;


    void Awake()
    {
        character = GetComponent<Character>();
        stateMachine = GetComponent<CharacterStateMachine>();
    }

    public void OnAttack()
    {
        character.Attack();
    }

    public void OnAttackEnd()
    {
        stateMachine.NotifyAttackAnimationEnd();
    }
}
