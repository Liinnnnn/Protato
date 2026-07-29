using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
public class ShopUIManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private GameObject PlayerView;
    [SerializeField] private GameObject Inventory;
    [SerializeField] private RectTransform InventorySlideTranform;
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
        Inventory.SetActive(true);
        InventorySlideTranform.DOKill();
        InventorySlideTranform.anchoredPosition = new Vector2(1000f,0);
        InventorySlideTranform.DOAnchorPosX(-250f,0.2f).SetUpdate(true).SetEase(Ease.InOutSine);
    }
    public void showInventoryInPause()
    {
        InventoryItemViewInPause.SetActive(true);
    }   
    public void hideInventory()
    {
        Inventory.SetActive(false);
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
