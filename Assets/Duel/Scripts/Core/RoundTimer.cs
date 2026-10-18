public class RoundTimer
{
    public float RoundElapsed { get; private set; }
    public float TotalElapsed { get; private set; }


    public void OnUpdate(float deltatime)
    {
        RoundElapsed += deltatime;
        TotalElapsed += deltatime;
    }

    public void ResetRound()
    {
        RoundElapsed = 0f;
    }

    public void ResetTotal()
    {
        TotalElapsed = 0f;
    }
}
