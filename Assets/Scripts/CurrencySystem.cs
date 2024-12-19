using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public int currency = 100;
    public int passiveGain = 1;
    public TMP_Text currencyText;

    private void Start()
    {
        InvokeRepeating("AddPassiveCurrency", 1f, 1f);
        UpdateCurrencyUI();
    }

    private void AddPassiveCurrency()
    {
        currency += passiveGain;
        UpdateCurrencyUI();
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        UpdateCurrencyUI();
    }

    public void SpendCurrency(int amount)
    {
        currency -= amount;
        UpdateCurrencyUI();
    }

    public int GetCurrency()
    {
        return currency;
    }

    private void UpdateCurrencyUI()
    {
        currencyText.text = $"Oxygen: {currency}";
    }
}
