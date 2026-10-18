using UnityEngine;

public static class ElapsedTimeFormatter
{
    public static string Format(float elapsedSeconds)
    {
        // 分と秒の境の四捨五入のズレを回避するために1/1000秒単位で分解する。
        long totalMilliseconds = (long)Mathf.Round(elapsedSeconds * 1000f);
        long minutes = totalMilliseconds / 60000;
        float seconds = totalMilliseconds % 60000 / 1000f;

        return minutes > 0 ? $"{minutes}:{seconds:00.000}" + "秒" : $"{seconds:0.000}" + "秒";
    }
}
