public class CharacterState_Back : CharacterStateBase
{
    public CharacterState_Back(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

    public override void OnEnter()
    {
        Context.Character.AnimatorController.SetMoveIntent(MoveIntent.Back);
    }

    public override void OnUpdate()
    {
        if (Context.AttackTriggered) {
            Machine.ChangeState(new CharacterState_Attack(Machine, Context));
            return;
        }

        if (Context.MoveIntent != MoveIntent.Back) {
            Machine.ChangeState(new CharacterState_Idle(Machine, Context));
            return;
        }

        Character.MoveBack();
    }

    public override void OnExit()
    {
        Context.Character.AnimatorController.SetMoveIntent(MoveIntent.None);
    }
}
