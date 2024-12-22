using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundMusicClip;
    public AudioClip buttonHoverClip;

    private AudioSource backgroundMusicSource;
    private AudioSource uiSoundEffectSource;

    void Start()
    {
        // Create and configure the background music AudioSource
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource.clip = backgroundMusicClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.playOnAwake = false;

        // Create and configure the UI sound effect AudioSource
        uiSoundEffectSource = gameObject.AddComponent<AudioSource>();
        uiSoundEffectSource.playOnAwake = false;

        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusicSource != null && backgroundMusicClip != null)
        {
            backgroundMusicSource.Play();
        }
    }

    public void PlayButtonHoverSound()
    {
        if (uiSoundEffectSource != null && buttonHoverClip != null)
        {
            uiSoundEffectSource.PlayOneShot(buttonHoverClip);
        }
    }
}