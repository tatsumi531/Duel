public class CharacterState_Neutral : CharacterStateBase
{
    public CharacterState_Neutral(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

    public override void OnEnter()
    {
        Context.Character.SpriteRenderer.enabled = true;
        Context.Character.AnimatorController.SetEnabled(true);
        Context.Character.AnimatorController.SetIdleTrigger();
        Context.Character.Collider2D.enabled = true;
    }
}
