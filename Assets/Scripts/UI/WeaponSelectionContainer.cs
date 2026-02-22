using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionContainer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Image icon ;
    [SerializeField] private TextMeshProUGUI Wname ;
    [field : SerializeField] public Button button{get;private set;}
    [Header("Stats")]
    [SerializeField] private Transform statsContainerP;
    [Header("Color")]
    [SerializeField] private Image levelImage;

    public void Configure(Sprite sprite, string name,int level,WeaponDataSO w)
    {
        Dictionary<Stats,float> calc = WeaponStatsCalculated.GetStats(w,level);
        configureStatsContainer(calc);
        
        icon.sprite =sprite;
        Wname.text = name + " LV." + (level + 1);
        Color imgColor = ColorHolder.getColor(level);
        Wname.color = imgColor;
        levelImage.color = imgColor;

    }

    private void configureStatsContainer(Dictionary<Stats,float> calc)
    {
        StatsContainerManager.GenerateStatsContainer(calc,statsContainerP);
    }

    public void DeSelect()
    {
        transform.localScale = Vector3.one;
    }

    public void Select()
    {
        transform.localScale = new Vector3(1.1f,1.1f,1.1f);
    }
}
