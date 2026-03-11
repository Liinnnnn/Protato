using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
public class CurrencyManager : MonoBehaviour
{
    private const string premiumMoneyKey = "Diamond";
    public static CurrencyManager instance;
    [field:SerializeField] public int Currency {get;private set;}
    // [field:SerializeField] public int diamond {get;private set;}
    [field:SerializeField] public TextMeshProUGUI currentCurrency {get;private set;}
    // [field:SerializeField] public TextMeshProUGUI currentDiamond {get;private set;}

    public static Action spent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateText();
        Coins.onCollected +=CollectCoins;
        // Diamond.onCollected += CollectDiamond;
    }

    void Awake()
    {
        instance = this;
        // AddDiamond(PlayerPrefs.GetInt(premiumMoneyKey,100),false);
    }
    void OnDestroy()
    {
        Coins.onCollected -=CollectCoins;
        // Diamond.onCollected -= CollectDiamond;

    }
    [Button]
    private void Add5000()
    {
        AddCurrency(5000);
    }
    // [Button]
    // private void Add5000Diamond()
    // {
    //     AddDiamond(1000);
    // }
    public void AddCurrency(int price)
    {
        Currency += price;
        UpdateText();
        spent?.Invoke();
    }

    private void UpdateText()
    {
        CurrencyText[] text = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include,FindObjectsSortMode.None);
        // DiamondText[] premium = FindObjectsByType<DiamondText>(FindObjectsInactive.Include,FindObjectsSortMode.None);

        foreach (CurrencyText c in text)
        {
            c.UpdateText(Currency.ToString());
        } 
        // foreach (DiamondText c in premium)
        // {
        //     c.UpdateText(diamond.ToString());
        // } 
    }

    public bool HasEnough(int rerollPrice)
    {
        return Currency >= rerollPrice;
    }
    // public bool HasEnoughDiamond(int rerollPrice)
    // {
    //     return diamond >= rerollPrice;
    // }

    public void UseCoin(int rerollPrice)
    {
        AddCurrency(-rerollPrice);
        spent?.Invoke();

    }
    // public void UseDiamond(int rerollPrice)
    // {
    //     AddDiamond(-rerollPrice);
    //     spent?.Invoke();

    // }
    private void CollectCoins(Coins coins)
    {
        AddCurrency(10);
    }
    // private void CollectDiamond(Diamond diamond)
    // {
    //     AddDiamond(1);
    // }

    // private void AddDiamond(int v,bool save = true)
    // {
    //    diamond +=v;
    //    UpdateText();
    //    spent?.Invoke();
    //    PlayerPrefs.SetInt(premiumMoneyKey,diamond);
    // }
}
