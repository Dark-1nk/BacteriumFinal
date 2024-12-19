using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TowerHealth : MonoBehaviour
{
    public int maxHealth = 500;
    public int health;
    public Slider healthSlider;
    public Canvas victoryScreen;
    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;

        if (health <= 0)
        {
            Debug.Log($"{gameObject.name} has been destroyed!");
            audioManager.PlaySFX(audioManager.towerDeath); 
            Destroy(gameObject);
            
            if (tag == "EnemyTower")
            {
                Time.timeScale = 0;
                if (victoryScreen != null)
                {
                    victoryScreen.gameObject.SetActive(true);
                }
            }

            if (tag == "PlayerTower")
            {
                SceneManager.LoadScene("Game Over");
            }

        }
    }
}
