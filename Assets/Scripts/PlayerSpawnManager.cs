using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform spawnPoint; // Position to spawn units
    public GameObject[] unitPrefabs; // Array of player unit prefabs
    public int[] unitCosts; // Costs for each unit
    private CurrencyManager currencyManager; // Reference to CurrencyManager
    public int[] unitCooldowns;
    bool[] isButtonOnCooldown;
    public Button[] unitButtons;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        isButtonOnCooldown = new bool[unitButtons.Length];

        // Assign listeners to the buttons
        for (int i = 0; i < unitButtons.Length; i++)
        {
            int index = i; // Capture the loop variable
            unitButtons[i].onClick.AddListener(() => SpawnUnit(index));
        }

        currencyManager = FindObjectOfType<CurrencyManager>();
        if (currencyManager == null)
        {
            Debug.LogError("CurrencyManager not found in the scene!");
        }
    }

    public void SpawnUnit(int unitIndex)
    {
        if (unitIndex < 0 || unitIndex >= unitPrefabs.Length)
        {
            Debug.LogError("Invalid unit index!");
            return;
        }

        if (currencyManager != null && currencyManager.GetCurrency() >= unitCosts[unitIndex])
        {
            audioManager.PlaySFX(audioManager.spawn);
            Instantiate(unitPrefabs[unitIndex], spawnPoint.position, Quaternion.identity);
            currencyManager.SpendCurrency(unitCosts[unitIndex]);
            StartCoroutine(ButtonCooldown(unitIndex, unitCooldowns[unitIndex]));
        }
    }

    private IEnumerator ButtonCooldown(int index, float cooldownDuration)
    {
        isButtonOnCooldown[index] = true;
        unitButtons[index].interactable = false;

        // Get the cooldown text component
        Text cooldownText = unitButtons[index].GetComponentInChildren<Text>();

        float elapsed = 0f;
        while (elapsed < cooldownDuration)
        {
            if (cooldownText != null)
            {
                cooldownText.text = Mathf.CeilToInt(cooldownDuration - elapsed).ToString();
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (cooldownText != null)
        {
            cooldownText.text = ""; // Clear text after cooldown
        }

        unitButtons[index].interactable = true;
        isButtonOnCooldown[index] = false;
    }
}
