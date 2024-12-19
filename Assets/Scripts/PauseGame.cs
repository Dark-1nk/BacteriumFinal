using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public Canvas targetCanvas;
    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            audioManager.PlaySFX(audioManager.interact);
            Time.timeScale = 0;
            ManageCanvas();
        }
    }

    public void ManageCanvas()
    {
        if (!targetCanvas.gameObject.activeInHierarchy)
        {
            targetCanvas.gameObject.SetActive(true);
        }

        else 
        {
            targetCanvas.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
}
