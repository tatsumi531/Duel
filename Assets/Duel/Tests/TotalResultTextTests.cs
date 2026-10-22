using NUnit.Framework;

public class TotalResultTextTests
{
    [TestCase(true, 8, "8人抜き達成!!")]
    [TestCase(false, 4, "4人抜き")]
    [TestCase(false, 1, "1人抜き")]
    [TestCase(false, 0, "敗北")]
    public void リザルトの表示文字列を組み立てる(bool isAllWin, int winRoundCount, string expected)
    {
        Assert.That(TotalResultText.Build(isAllWin, winRoundCount), Is.EqualTo(expected));
    }
}
