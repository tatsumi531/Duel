namespace Duel.Characters.States
{
    public interface ICharacterState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}
