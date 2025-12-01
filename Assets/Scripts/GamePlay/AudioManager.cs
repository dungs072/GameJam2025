using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gamePlayMusic;
    [SerializeField] private AudioClip startButtonSfx;
    [SerializeField] private AudioClip pickUpItemSfx;
    [SerializeField] private AudioClip throwItemSfx;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMainMenuSong()
    {
        backgroundMusicSource.clip = mainMenuMusic;
        backgroundMusicSource.Play();
    }
    public void PlayGamePlaySong()
    {
        backgroundMusicSource.clip = gamePlayMusic;
        backgroundMusicSource.Play();
    }
    public void PlayStartButtonSfx()
    {
        sfxSource.PlayOneShot(startButtonSfx);
    }
    public void PlayPickUpItemSfx()
    {
        sfxSource.PlayOneShot(pickUpItemSfx);
    }
    public void PlayThrowItemSfx()
    {
        sfxSource.PlayOneShot(throwItemSfx);
    }
}