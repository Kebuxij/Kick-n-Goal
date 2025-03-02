using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } // Singleton

    public AudioSource musicSource;  // Источник для фоновой музыки
    public AudioSource sfxSource;    // Источник для эффектов

    public AudioClip backgroundMusic; // Фоновая музыка
    public AudioClip[] sfxCollision;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Оставлять при смене сцен
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);
    }

    // Метод для проигрывания музыки
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return; // Если уже играет, ничего не делаем
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Метод для проигрывания звуковых эффектов
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // Метод для изменения громкости
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void PlayRandomCollisionSound()
    {
        AudioClip sfxRandomCollision = sfxCollision[Random.Range(0, sfxCollision.Length)];
        if (sfxRandomCollision != null)
        {
            sfxSource.PlayOneShot(sfxRandomCollision);
        }
    }
}
