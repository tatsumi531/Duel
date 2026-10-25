using Duel.Core;

namespace Duel.Characters.States
{
    public class CharacterState_Move : CharacterStateBase
    {
        readonly MoveIntent intent;

        public CharacterState_Move(CharacterStateMachine machine, CharacterContext context, MoveIntent intent) : base(machine, context)
        {
            this.intent = intent;
        }

        public override void OnEnter()
        {
            Character.AnimatorController.SetMoveIntent(intent);
        }

        public override void OnUpdate()
        {
            if (TryTransitionToAttack()) {
                return;
            }

            if (Context.MoveIntent != intent) {
                Machine.ChangeState(new CharacterState_Idle(Machine, Context));
                return;
            }

            Character.Move(intent);
        }

        public override void OnExit()
        {
            Character.AnimatorController.SetMoveIntent(MoveIntent.None);
        }
    }
}
