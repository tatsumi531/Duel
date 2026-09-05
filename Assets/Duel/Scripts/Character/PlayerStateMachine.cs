using UnityEngine.InputSystem;

public enum MoveIntent
{
    None,
    Forward,
    Back
}


public class PlayerStateMachine : CharacterStateMachine
{
    public override void InputAction()
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
                context.MoveIntent = worldDirection == (int)character.Direction
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
