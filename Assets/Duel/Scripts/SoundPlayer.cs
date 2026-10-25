using UnityEngine;

namespace Duel
{
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
            audioSource.spatialBlend = 0f;
            audioSource.Play();
            Object.Destroy(gameObject, clip.length);
        }
    }
}
