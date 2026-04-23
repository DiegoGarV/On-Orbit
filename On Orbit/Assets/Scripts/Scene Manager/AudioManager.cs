using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip victoryMusic;
    [SerializeField] private AudioClip defeatMusic;

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
}