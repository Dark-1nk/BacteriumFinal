using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;
    public AudioSource musicSource; // AudioSource for background music
    public AudioSource sfxSource;  // AudioSource for sound effects
    public List<AudioClip> sceneMusic; // List of music clips (one per scene)

    private static AudioManager instance;

    [Header("----- Audio Clips -----")]
    public AudioClip death;
    public AudioClip spawn;
    public AudioClip towerDeath;
    public AudioClip interact;
    public AudioClip victory;
    public AudioClip upgrade;

    private void Start()
    {
        // Set sliders to saved values or defaults
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        // Apply the saved volume settings
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        // Add listeners to update volume dynamically
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    private void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Subscribe to scene change events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to prevent memory leaks
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play the corresponding music for the loaded scene
        int sceneIndex = scene.buildIndex;

        if (sceneIndex >= 0 && sceneIndex < sceneMusic.Count && sceneMusic[sceneIndex] != null)
        {
            PlayMusic(sceneMusic[sceneIndex]);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return; // Avoid restarting the same music

        StartCoroutine(CrossfadeMusic(clip, 0.5f));
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    public IEnumerator CrossfadeMusic(AudioClip clip, float fadeDuration)
    {
        float startVolume = musicSource.volume;

        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }
    }
}
