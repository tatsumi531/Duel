public abstract class CharacterStateBase : ICharacterState
{
    protected CharacterStateMachine Machine { get; }
    protected CharacterContext Context { get; }
    protected Character Character => Context.Character;

    protected CharacterStateBase(CharacterStateMachine machine, CharacterContext context)
    {
        Machine = machine;
        Context = context;
    }

    public virtual void OnEnter() {}
    public virtual void OnUpdate() {}
    public virtual void OnExit() {}
}
