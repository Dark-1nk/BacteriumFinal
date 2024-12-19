using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public int upgradeCost = 50;
    public int passiveGainIncrease = 2;
    public Button upgradeButton;
    private CurrencyManager currencyManager;
    public TMPro.TMP_Text upgradeText;
    public TMPro.TMP_Text levelText;
    public int currentLevel;

    AudioManager audioManager;

    private void Start()
    {
        currentLevel = 1;
        levelText.text = ("Level: " + currentLevel);
        currencyManager = FindObjectOfType<CurrencyManager>();
        upgradeButton.onClick.AddListener(UpgradeEconomy);
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void UpgradeEconomy()
    {
        if(currentLevel < 4)
        {
            
            if (currencyManager.GetCurrency() >= upgradeCost)
            {
                audioManager.PlaySFX(audioManager.upgrade);
                currencyManager.SpendCurrency(upgradeCost);
                currentLevel++;
                currencyManager.passiveGain *= passiveGainIncrease;
                upgradeCost *= 5;
                upgradeText.text = ("$" + upgradeCost);
                levelText.text = ("Level: " + currentLevel);
                Debug.Log("Economy upgraded!");
            }
            else
            {
                audioManager.PlaySFX(audioManager.death);
                Debug.Log("Not enough currency to upgrade.");
            }
        }
        else if (currentLevel == 4) 
        {
            if (currencyManager.GetCurrency() >= upgradeCost)
            {
                audioManager.PlaySFX(audioManager.upgrade);
                currencyManager.SpendCurrency(upgradeCost);
                currentLevel++;
                currencyManager.passiveGain *= passiveGainIncrease;
                upgradeCost *= 5;
                upgradeText.text = ("MAX");
                levelText.text = ("Level: MAX");
                Debug.Log("Economy upgraded to max!");
            }
            else
            {
                audioManager.PlaySFX(audioManager.death);
                Debug.Log("Not enough currency to upgrade.");
            }
        }
        else if (currentLevel == 5)
        {
            audioManager.PlaySFX(audioManager.death);
            Debug.Log("Your Greed Sickens Me...");
        }
    }
}
