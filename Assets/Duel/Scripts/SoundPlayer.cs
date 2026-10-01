using UnityEngine;

/// <summary>
/// 効果音を2Dで一度だけ再生するユーティリティ。
/// 再生を要求したGameObjectが即座にDestroyされても音が途切れないよう、再生専用のGameObjectを生成して鳴らす。
/// </summary>
public static class SoundPlayer
{
    public static void PlayOneShot(AudioClip clip, float volume = 1f)
    {
        if (clip == null) {
            return;
        }
        var gameObject = new GameObject($"OneShotSound ({clip.name})");
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        // 2D再生（リスナーとの距離による減衰を無効化）
        audioSource.spatialBlend = 0f;
        audioSource.Play();
        Object.Destroy(gameObject, clip.length);
    }
}
