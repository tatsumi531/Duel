using UnityEngine;

public class RoundCounter
{
    public int CurrentRound { get; private set; }


    public void IncrementRound()
    {
        CurrentRound++;
    }

    public void ResetRound()
    {
        CurrentRound = 1;
    }

    public bool ExistNextRound(GameData gameData)
    {
        return CurrentRound < gameData.maxRounds;
    }
}
