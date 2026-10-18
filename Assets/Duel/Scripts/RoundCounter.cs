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

    public void ResetRound()
    {
        CurrentRound = 0;
        WinRoundCount = 0;
    }

    public bool ExistNextRound(GameData gameData)
    {
        return CurrentRound < gameData.maxRounds;
    }
}
