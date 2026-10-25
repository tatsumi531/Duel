using UnityEngine.InputSystem;
using Duel.Core;

namespace Duel.Characters
{
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
}
