using UnityEngine;

public class CharacterAnimationEventReceiver : MonoBehaviour
{
    [SerializeField] Character character;
    [SerializeField] CharacterStateMachine stateMachine;


    public void OnAttack()
    {
        character.Attack();
    }

    public void OnAttackEnd()
    {
        stateMachine.NotifyAttackAnimationEnd();
    }
}
