using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip collisionSound;
    public AudioClip deathSound;
    public AudioClip winSound;
    public AudioClip startSound;
    public AudioClip restartSound;
    
    public AudioClip backgroundMusic;
    public AudioClip pauseMusic;

    private AudioSource sfxSource;
    private AudioSource bgSource;

    void Awake()
    {
        instance = this;

        AudioSource[] sources = GetComponents<AudioSource>();
        bgSource = sources[0];     // Background music
        sfxSource = sources[1];    // Sound effects
    }

    public void PlaySound(AudioClip clip, bool pauseMusic = false)
    {
        if (pauseMusic)
        {
            bgSource.Pause();
            StartCoroutine(ResumeMusicAfter(clip.length));
        }

        sfxSource.PlayOneShot(clip);
    }

    public void ChangeBackgroundMusic(AudioClip newClip, bool loop = true)
    {
        if (bgSource.clip == newClip) return; // prevent restarting same music

        bgSource.Stop();
        bgSource.clip = newClip;
        bgSource.loop = loop;
        bgSource.Play();
    }

    IEnumerator ResumeMusicAfter(float time)
    {
        yield return new WaitForSeconds(time);
        bgSource.UnPause();
    }
}
