using NUnit.Framework;
using Duel.Core;

public class ElapsedTimeFormatterTests
{
    [TestCase(0f, "0.000秒")]
    [TestCase(0.5f, "0.500秒")]
    [TestCase(1.234f, "1.234秒")]
    [TestCase(60f, "1:00.000秒")]
    [TestCase(120f, "2:00.000秒")]
    [TestCase(59.9999f, "1:00.000秒")]
    public void 経過秒数を表示用の文字列に整形する(float seconds, string expected)
    {
        Assert.That(ElapsedTimeFormatter.Format(seconds), Is.EqualTo(expected));
    }
}
