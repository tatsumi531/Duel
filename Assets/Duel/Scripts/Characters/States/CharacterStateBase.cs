namespace Duel.Characters.States
{
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

        protected bool TryTransitionToAttack()
        {
            if (!Context.AttackTriggered) {
                return false;
            }
            Machine.ChangeState(new CharacterState_Attack(Machine, Context));
            return true;
        }
    }
}
