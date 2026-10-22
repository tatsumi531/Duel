public class CharacterState_Forward : CharacterStateBase
{
    public CharacterState_Forward(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

    public override void OnEnter()
    {
        Context.Character.AnimatorController.SetMoveIntent(MoveIntent.Forward);
    }

    public override void OnUpdate()
    {
        if (Context.AttackTriggered) {
            Machine.ChangeState(new CharacterState_Attack(Machine, Context));
            return;
        }

        if (Context.MoveIntent != MoveIntent.Forward) {
            Machine.ChangeState(new CharacterState_Idle(Machine, Context));
            return;
        }

        Character.MoveForward();
    }

    public override void OnExit()
    {
        Context.Character.AnimatorController.SetMoveIntent(MoveIntent.None);
    }
}
