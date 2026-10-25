namespace Duel.Core
{
    public class RoundTimer
    {
        public float RoundElapsed { get; private set; }
        public float TotalElapsed { get; private set; }


        public void OnUpdate(float deltaTime)
        {
            RoundElapsed += deltaTime;
            TotalElapsed += deltaTime;
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
}
