public class CharacterState_Idle : CharacterStateBase
{
    public CharacterState_Idle(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

    public override void OnUpdate()
    {
        if (Context.AttackTriggered) {
            Machine.ChangeState(new CharacterState_Attack(Machine, Context));
            return;
        }

        switch (Context.MoveIntent) {
            case MoveIntent.Forward:
                Machine.ChangeState(new CharacterState_Move(Machine, Context, MoveIntent.Forward));
                break;
            case MoveIntent.Back:
                Machine.ChangeState(new CharacterState_Move(Machine, Context, MoveIntent.Back));
                break;
        }
    }
}
