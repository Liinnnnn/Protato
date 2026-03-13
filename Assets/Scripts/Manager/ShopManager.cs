using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour,IGameStateListener
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Shop")]
    [SerializeField] private Transform ContainerParent; 
    [SerializeField] private ShopItemContainer shopItemPref;
    [Header("Player")]
    [SerializeField] private PlayerWeapon playerWeapon; 
    [SerializeField] private PlayerObject playerObject;
    [Header("Rerolls")]
    [SerializeField] private Button rerollButton;
    [SerializeField] private int rerollPrice;
    [SerializeField] private TextMeshProUGUI priceText;
    public static Action onItemPurchase;
    void Awake()
    {
        ShopItemContainer.onPurchase += ItemBuy;
        CurrencyManager.spent += CurrencyManagerCallback;
        
    }
    void OnDestroy()
    {
        ShopItemContainer.onPurchase -= ItemBuy;
        CurrencyManager.spent -= CurrencyManagerCallback;
        
    }
    public void GameStateChangeCallBack(GameState gameState)
    {
        if(gameState == GameState.SHOP)
        {
            Configure();
            UpdateRerollVisual();
        }
    }

    private void Configure()
    {
        
        List<GameObject> ToDestroy = new List<GameObject>();
        for (int i = 0; i < ContainerParent.childCount; i++)
        {
            ShopItemContainer container = ContainerParent.GetChild(i).GetComponent<ShopItemContainer>();
            if(!container.isLocked) ToDestroy.Add(container.gameObject);
        }
        while(ToDestroy.Count > 0)
        {
            Transform t = ToDestroy[0].transform;
            t.SetParent(null);
            ToDestroy.RemoveAt(0);
        }
        int index = 6 - ContainerParent.childCount;
        int weaponContainerCount = UnityEngine.Random.Range(Mathf.Min(2,index),index);
        int objectContainerCount = index - weaponContainerCount;

        for (int i = 0; i < weaponContainerCount; i++)
        {
            ShopItemContainer weapon =   Instantiate(shopItemPref,ContainerParent);
            weapon.name = "Weapon Container";
            WeaponDataSO randomWP = ResourcesManager.randomWeapon();
            weapon.Configure(randomWP,UnityEngine.Random.Range(0,4));
        }
        for (int i = 0; i < objectContainerCount; i++)
        {
            ShopItemContainer anitque =   Instantiate(shopItemPref,ContainerParent);
            anitque.name = "Relic Container";
            ObjectDataSO randomObj = ResourcesManager.randomObj();
            anitque.Configure(randomObj);
        }
    }
    public void Reroll()
    {
        Configure();
        CurrencyManager.instance.UseCoin(rerollPrice);
    }
    private void UpdateRerollVisual()
    {
        priceText.text = rerollPrice.ToString();
        rerollButton.interactable = CurrencyManager.instance.HasEnough(rerollPrice);
    }

    private void ItemBuy(ShopItemContainer container, int lv)
    {
        if(container.weaponData!=null)
            tryPurchaseWeapon(container,lv);
        else 
            purchaseRelics(container);
    }

    private void purchaseRelics(ShopItemContainer c)
    {
        playerObject.AddObject(c.objectData);
        CurrencyManager.instance.UseCoin(c.objectData.sellPrice);
        Destroy(c.gameObject);
        onItemPurchase?.Invoke();

    }

    private void tryPurchaseWeapon(ShopItemContainer s,int lv)
    {
        if (playerWeapon.tryAddWeapon(s.weaponData, lv))
        {
            int price = WeaponStatsCalculated.GetPrice(s.weaponData,lv);
            CurrencyManager.instance.UseCoin(price);
            onItemPurchase?.Invoke();
            Destroy(s.gameObject);
        }
    }

    private void CurrencyManagerCallback()
    {
        UpdateRerollVisual();
    }

}
