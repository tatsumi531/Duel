using UnityEngine;

public class EnemyStateMachine : CharacterStateMachine
{
    [SerializeField] float attackRange = 2f;
    [SerializeField] float attackCooldown = 0.6f;

    Player player;
    float cooldownTimer;

    public override void InputAction()
    {
        context.MoveIntent = MoveIntent.None;
        context.AttackTriggered = false;

        if (player == null) {
            player = FindAnyObjectByType<Player>();
            if (player == null) return;
        }

        if (cooldownTimer > 0f) {
            cooldownTimer -= Time.deltaTime;
        }

        float distance = Mathf.Abs(player.transform.position.x - character.transform.position.x);

        if (distance <= attackRange) {
            if (cooldownTimer <= 0f) {
                context.AttackTriggered = true;
                cooldownTimer = attackCooldown;
            }
        } else {
            context.MoveIntent = MoveIntent.Forward;
        }
    }
}
