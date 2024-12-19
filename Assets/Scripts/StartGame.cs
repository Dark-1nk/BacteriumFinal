using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

    AudioManager audioManager;
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void LoadGame()
    {
        Time.timeScale = 1.0f;
        audioManager.PlaySFX(audioManager.upgrade);
        SceneManager.LoadScene("Game");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        audioManager.PlaySFX(audioManager.upgrade);
        SceneManager.LoadScene("Main Menu");
    }
}