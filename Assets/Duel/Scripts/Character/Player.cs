using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーキャラクター。
/// A キーで画面左、D キーで画面右へ移動する。
/// 実際の前進 / 後退はキャラクターの向き（Direction）に応じて決まる。
/// </summary>
public class Player : Character
{
    void Update()
    {
        HandleMoveInput();
        HandleAttackInput();
    }

    void HandleMoveInput()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) {
            return;
        }

        // 押されているキーから移動したい「ワールド上の向き」を求める。
        // A: -X 方向 / D: +X 方向
        int worldDirection = 0;
        if (keyboard.aKey.isPressed) {
            worldDirection -= 1;
        }
        if (keyboard.dKey.isPressed) {
            worldDirection += 1;
        }
        if (worldDirection == 0) {
            return;
        }

        // ワールド上の移動方向と、キャラクターの向きが一致していれば前進、
        // 逆なら後退。
        if (worldDirection == (int)Direction) {
            MoveForward();
        } else {
            MoveBack();
        }
    }

    void HandleAttackInput()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) {
            return;
        }

        if(mouse.leftButton.IsPressed())
        {
            Attack();
        }
    }
}
