using UnityEngine;

public static class AudioSourceExtentions
{
    public static void CheckIsAudioEnabledAndPlay(this AudioSource audioSource)
    {
        if (PersistentData.AudioSettigns.IsEnabled())
            audioSource.Play();
    }

    public static void CheckIsAidioEnabledAndPlayOneShot(this AudioSource audioSource)
    {
        if (PersistentData.AudioSettigns.IsEnabled())
            audioSource.PlayOneShot(audioSource.clip);
    }
}
