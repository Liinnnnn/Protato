using System;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryItemManager : MonoBehaviour,IGameStateListener
{
    [Header("REF")] 
    [SerializeField] private Transform inventoryItemsParent;
    [SerializeField] private Transform inventoryItemsPause;
    [SerializeField] private InventoryItemContainer inventoryItemContainer;
    [SerializeField] private PlayerObject playerObject;
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private ShopUIManager shopUIManager;
    [SerializeField] private InventoryItemInfo inventoryItemInfo;
    [SerializeField] private InventoryItemInfo inventoryItemInfoPause;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShopManager.onItemPurchase += ItemPurchase;
        WeaponMerge.onMerge += MergeCallback;
        GameManager.onPaused += Configure;
    }



    // Update is called once per frame
    void OnDestroy()
    {
        ShopManager.onItemPurchase -= ItemPurchase;
        WeaponMerge.onMerge -= MergeCallback;
        GameManager.onPaused -= Configure;
    }

    public void GameStateChangeCallBack(GameState gameState)
    {
        if (gameState == GameState.SHOP)
        {
            Configure();
        }
    }

    private void Configure()
    {
        foreach (Transform item in inventoryItemsParent)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in inventoryItemsPause)
        {
            Destroy(item.gameObject);
        }
        Weapon[] wData = playerWeapon.GetWeapons();
        ObjectDataSO[] oData =  playerObject.objects.ToArray();
       
        for (int i = 0; i < wData.Length; i++)
        {
            if(wData[i] ==null )
                continue;
            InventoryItemContainer w = Instantiate(inventoryItemContainer,inventoryItemsParent);
            InventoryItemContainer p = Instantiate(inventoryItemContainer,inventoryItemsPause);
            w.Configure(wData[i],i,()=>ShowItemInfo(w));
            p.Configure(wData[i],i,()=>ShowPauseItemInfo(w));
            
        }
        for (int i = 0; i < oData.Length; i++)
        {
            InventoryItemContainer inventoryItem = Instantiate(inventoryItemContainer,inventoryItemsParent);
            inventoryItem.Configure(oData[i],()=>ShowItemInfo(inventoryItem));

            InventoryItemContainer p = Instantiate(inventoryItemContainer,inventoryItemsPause);
            p.Configure(oData[i],()=>ShowPauseItemInfo(inventoryItem));

        }
    }

    private void ShowPauseItemInfo(InventoryItemContainer i)
    {
        if(i.weapon == null)
        {
            showPauseRelic(i.objectDataSO);
        }else showPauseWeapon(i.weapon,i.Index);
    }

   
    private void ShowItemInfo(InventoryItemContainer i)
    {
        if(i.weapon == null)
        {
            showRelic(i.objectDataSO);
        }else showWeapon(i.weapon,i.Index);
    }

    private void showRelic(ObjectDataSO objectDataSO)
    {
        inventoryItemInfo.Configure(objectDataSO);
        inventoryItemInfo.recycle.onClick.RemoveAllListeners();
        inventoryItemInfo.recycle.onClick.AddListener(()=>RecycleObj(objectDataSO));
        shopUIManager.showInventoryItem();
    }
    private void showPauseRelic(ObjectDataSO objectDataSO)
    {
        shopUIManager.showInventoryInPause();
        inventoryItemInfoPause.Configure(objectDataSO);
    }

    private void RecycleObj(ObjectDataSO objectDataSO)
    {
        playerObject.recyle(objectDataSO);
        Configure();
        shopUIManager.hideInventoryItem();
    }

    private void showWeapon(Weapon weaponDataSO,int index)
    {
        inventoryItemInfo.Configure(weaponDataSO);
        shopUIManager.showInventoryItem();
        inventoryItemInfo.recycle.onClick.RemoveAllListeners();
        inventoryItemInfo.recycle.onClick.AddListener(()=>RecycleWeapon(index));
    }
    private void showPauseWeapon(Weapon weaponDataSO,int index)
    {
        shopUIManager.showInventoryInPause();
        inventoryItemInfoPause.Configure(weaponDataSO);
    }
    private void RecycleWeapon(int index)
    {
        playerWeapon.Recyle(index);
        Configure();
        shopUIManager.hideInventoryItem();
    }

    private void ItemPurchase()
    {
        Configure();
    }
    private void MergeCallback(Weapon weapon)
    {
        Configure();
        inventoryItemInfo.Configure(weapon);
    }

}
