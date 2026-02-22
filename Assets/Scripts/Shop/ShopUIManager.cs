using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private GameObject PlayerView;
    [SerializeField] private GameObject InventoryView;
    [SerializeField] private GameObject InventoryItemView;
    [SerializeField] private GameObject InventoryItemViewInPause;

    void Start()
    {
        hideInventory();
        hideInventoryItem();
        hideStats();
    }
    public void showStats()
    {
        PlayerView.SetActive(true);
    }

    public void hideStats()
    {
        PlayerView.SetActive(false);
    }
    public void showInventory()
    {
        InventoryView.SetActive(true);
    }
    public void showInventoryInPause()
    {
        InventoryItemViewInPause.SetActive(true);
    }   
    public void hideInventory()
    {
        InventoryView.SetActive(false);
    }
    [Button]
    public void showInventoryItem()
    {
        InventoryItemView.SetActive(true);
    }
    [Button]
    public void hideInventoryItem()
    {
        InventoryItemView.SetActive(false);
    }
}
