using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip victoryMusic;
    [SerializeField] private AudioClip defeatMusic;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip playerShootSFX;
    [SerializeField] private AudioClip playerHitSFX;
    [SerializeField] private AudioClip playerDeathSFX;
    [SerializeField] private AudioClip enemyShootSFX;
    [SerializeField] private AudioClip enemyHitSFX;
    [SerializeField] private AudioClip enemyDeathSFX;
    [SerializeField] private AudioClip meteorAlertSFX; 

    private void Awake()
    {
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        PlayLevelMusic();
    }

    public void PlayLevelMusic()
    {
        PlayMusic(levelMusic, true);
    }

    public void PlayVictoryMusic()
    {
        PlayMusic(victoryMusic, false);
    }

    public void PlayDefeatMusic()
    {
        PlayMusic(defeatMusic, false);
    }

    private void PlayMusic(AudioClip clip, bool loop)
    {
        if (musicSource == null || clip == null)
            return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlayPlayerShootSFX() => PlaySFX(playerShootSFX);
    public void PlayPlayerHitSFX() => PlaySFX(playerHitSFX);
    public void PlayPlayerDeathSFX() => PlaySFX(playerDeathSFX);

    public void PlayEnemyShootSFX() => PlaySFX(enemyShootSFX);
    public void PlayEnemyHitSFX() => PlaySFX(enemyHitSFX);
    public void PlayEnemyDeathSFX() => PlaySFX(enemyDeathSFX);
    public void PlayMeteorAlertSFX() => PlaySFX(meteorAlertSFX);
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        if (musicSource == null)
            return;

        musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource == null)
            return;

        musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource == null)
            return;

        sfxSource.volume = Mathf.Clamp01(volume);
    }
}