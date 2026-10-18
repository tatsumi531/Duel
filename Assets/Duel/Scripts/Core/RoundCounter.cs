public class RoundCounter
{
    public int CurrentRound { get; private set; }
    public int WinRoundCount { get; private set; }


    public void IncrementRound()
    {
        CurrentRound++;
    }

    public void IncrementWinRound()
    {
        WinRoundCount++;
    }

    public void ResetAll()
    {
        CurrentRound = 0;
        WinRoundCount = 0;
    }

    public bool ExistNextRound(int maxRounds)
    {
        return CurrentRound < maxRounds;
    }
}
