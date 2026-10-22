public class CharacterContext
{
    public Character Character { get; }
    public Character Opponent { get; }
    public MoveIntent MoveIntent { get; set; }
    public bool AttackTriggered { get; set; }
    public bool AttackAnimationEnded { get; set; }
    public bool HasHit { get; set; }


    public CharacterContext(Character character, Character opponent)
    {
        Character = character;
        Opponent = opponent;
    }
}
