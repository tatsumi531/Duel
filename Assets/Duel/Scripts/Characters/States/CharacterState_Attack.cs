namespace Duel.Characters.States
{
    public class CharacterState_Attack : CharacterStateBase
    {
        public CharacterState_Attack(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

        public override void OnEnter()
        {
            Context.AttackAnimationEnded = false;
            Character.AnimatorController.SetAttackTrigger();
        }

        public override void OnUpdate()
        {
            if (Context.AttackAnimationEnded) {
                Machine.ChangeState(new CharacterState_Idle(Machine, Context));
            }
        }
    }
}
