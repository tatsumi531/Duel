using UnityEngine;

namespace Duel.Characters.States
{
    public class CharacterState_Hit : CharacterStateBase
    {
        const float duration = 1f;
        float elapsed;

        public CharacterState_Hit(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

        public override void OnEnter()
        {
            elapsed = 0f;
            Context.Character.PlaySliceEffect();
            Context.Character.AnimatorController.SetEnabled(false);
            Context.Character.Collider2D.enabled = false;
        }

        public override void OnUpdate()
        {
            elapsed += Time.deltaTime;
            if (elapsed >= duration) {
                Machine.ChangeState(new CharacterState_NoneActive(Machine, Context));
            }
        }
    }
}
