namespace Duel.Core
{
    public static class TotalResultText
    {
        public static string Build(bool isAllWin, int winRoundCount)
        {
            if (isAllWin) {
                return $"{winRoundCount}人抜き達成!!";
            }
            return winRoundCount > 0 ? $"{winRoundCount}人抜き" : "敗北";
        }
    }
}
