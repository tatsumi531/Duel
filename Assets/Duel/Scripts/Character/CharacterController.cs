using UnityEngine;
using UnityEngine.InputSystem;

public interface ICharacterController
{
    void OnUpdate();
}


public class PlayerController : ICharacterController
{
    readonly CharacterContext context;


    public PlayerController(CharacterContext context)
    {
        this.context = context;
    }

    public void OnUpdate()
    {
        context.MoveIntent = MoveIntent.None;
        context.AttackTriggered = false;

        Keyboard keyboard = Keyboard.current;
        if (keyboard != null) {
            // A: -X 方向 / D: +X 方向 のワールド上の入力方向
            int worldDirection = 0;
            if (keyboard.aKey.isPressed) worldDirection -= 1;
            if (keyboard.dKey.isPressed) worldDirection += 1;

            if (worldDirection != 0) {
                context.MoveIntent = worldDirection == (int)context.Character.Direction
                    ? MoveIntent.Forward
                    : MoveIntent.Back;
            }
        }

        Mouse mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame) {
            context.AttackTriggered = true;
        }
    }
}


public class AIController : ICharacterController
{
    readonly CharacterContext context;
    readonly CharacterAIProfile profile;
    float decisionTimer;
    MoveIntent currentMoveIntent = MoveIntent.None;
    bool rushing; // 間合いを詰めると決めた後、攻撃範囲に入るまで前進をやめないためのフラグ


    public AIController(CharacterContext context, CharacterAIProfile profile)
    {
        this.context = context;
        this.profile = profile;
    }

    public void OnUpdate()
    {
        context.AttackTriggered = false;

        if (profile == null) return;

        decisionTimer -= Time.deltaTime;
        if (decisionTimer <= 0f) {
            MakeDecision();
            decisionTimer = Random.Range(profile.reactionTimeMin, profile.reactionTimeMax);
        }

        context.MoveIntent = currentMoveIntent;
    }

    void MakeDecision()
    {
        var input = new AIDecisionInput();
        input.distance = Mathf.Abs(context.Opponent.transform.position.x - context.Character.transform.position.x);
        input.selfAttackRange = context.Character.GetAttackRange(context.Opponent);
        input.opponentAttackRange = context.Opponent.GetAttackRange(context.Character);
        input.roll = Random.value;
        input.rushing = rushing;

        AIDecisionResult result = AIDecision.Decide(input, profile.ToParameters());

        currentMoveIntent = result.moveIntent;
        context.AttackTriggered = result.attack;
        rushing = result.rushing;
    }
}
