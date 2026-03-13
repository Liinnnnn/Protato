using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemInfo : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI Iname;
    [SerializeField] private TextMeshProUGUI price;
    [Header("Color")]
    [SerializeField] private Image container;

    [Header("stats")]
    [SerializeField] private Transform parents;
    [Header("Button")]

    [field : SerializeField] public Button recycle{get;private set;}
    // [field : SerializeField] public Button merge{get;private set;}

    public void Configure(Weapon w)
    {
        configure(w.weaponData.Sprite,
        w.name + " LV. " + (w.Level + 1),
        WeaponStatsCalculated.GetPrice(w.weaponData,(float) w.Level) / 2,
        ColorHolder.getColor(w.Level),
        WeaponStatsCalculated.GetStats(w.weaponData,(float) w.Level)
        );
        // merge.gameObject.SetActive(true);

        // merge.interactable = WeaponMerge.instance.CanMerge(w);
        // merge.interactable = false;
        // merge.onClick.RemoveAllListeners();
        // merge.onClick.AddListener(() => WeaponMerge.instance.Merge());
    }
    public void Configure(ObjectDataSO w)
    {
        configure(w.icon,
        w.name,
        w.sellPrice,
        ColorHolder.getColor(w.rarity),
        w.BaseStat
        );
    }
    public void configure(Sprite s, string name,int pre ,Color c, Dictionary<Stats,float> stat)
    {
        icon.sprite = s;
        Iname.text = name ;
        price.text = pre.ToString();
        container.color = c;
        Iname.color = c;
        StatsContainerManager.GenerateStatsContainer(stat,parents);
    }
}
