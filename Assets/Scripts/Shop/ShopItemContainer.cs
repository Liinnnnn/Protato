using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class ShopItemContainer : MonoBehaviour
{
     [Header("Settings")]
    [SerializeField] private Image icon ;
    [SerializeField] private TextMeshProUGUI ItemName ;
    [SerializeField] private TextMeshProUGUI priceText ;
    [SerializeField] private Button PurchaseButton;
    [Header("Stats")]
    [SerializeField] private Transform statsContainerP;
    [Header("Color")]
    [SerializeField] private Image levelImage;
    [Header("Button")]
    [SerializeField] private Button locked;
    [SerializeField] private Sprite Locked,unlocked;
    public bool isLocked{get;set;}
    public static Action<ShopItemContainer,int> onPurchase;
    private int weaponLV;
    public WeaponDataSO weaponData{get;private set;}
    public ObjectDataSO objectData{get;private set;}
    private int itemPrice;
    
    void Awake()
    {
        CurrencyManager.spent += Check;
        
    }
    void OnDestroy()
    {
        CurrencyManager.spent -= Check;
        
    }
    public void Configure(WeaponDataSO w,int level)
    {
        PurchaseButton.interactable = CurrencyManager.instance.HasEnough(w.price);
        PurchaseButton.onClick.AddListener(()=>Purchase());
        Dictionary<Stats,float> calc = WeaponStatsCalculated.GetStats(w,level);
        configureStatsContainer(calc);
        icon.sprite =w.Sprite;
        ItemName.text = w.WeaponName + " LV." + (level +1);
        priceText.text = WeaponStatsCalculated.GetPrice(w,level).ToString();
        Color imgColor = ColorHolder.getColor(level);
        ItemName.color = imgColor;
        levelImage.color = imgColor;
        weaponLV = level;
        weaponData = w;
        itemPrice = WeaponStatsCalculated.GetPrice(w,level);
    }
    public void Configure(ObjectDataSO w)
    {
        int buyPrice = w.sellPrice * 2;
        PurchaseButton.interactable = CurrencyManager.instance.HasEnough(buyPrice);
        configureStatsContainer(w.BaseStat);
        PurchaseButton.onClick.AddListener(()=>Purchase());
        
        icon.sprite = w.icon;
        
        ItemName.text = w.Name ;
        priceText.text = buyPrice.ToString();
        Color imgColor = ColorHolder.getColor(w.rarity);
        ItemName.color = imgColor;
        levelImage.color = imgColor;
        objectData = w;
        Debug.Log(w);
        itemPrice = buyPrice;
    }
    private void configureStatsContainer(Dictionary<Stats,float> calc)
    {
        foreach (Transform child in statsContainerP) {
            Destroy(child.gameObject);
        } 
        StatsContainerManager.GenerateStatsContainer(calc,statsContainerP);
    }
    public void lockButtonCallback()
    {
        isLocked = !isLocked;
        updateLock();
    }
    private void Purchase()
    {
        onPurchase?.Invoke(this,weaponLV);
    }
    private void updateLock()
    {
        locked.image.sprite = isLocked ? Locked : unlocked;
    }
    private void Check()
    {
        PurchaseButton.interactable = CurrencyManager.instance.HasEnough(itemPrice);   
    }
}
