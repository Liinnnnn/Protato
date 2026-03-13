using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private WeaponPosition[] WParents;
    public bool tryAddWeapon(WeaponDataSO w,int lv)
    {
        for (int i = 0; i < WParents.Length; i++)
        {
            if(WParents[i].weapon != null)
            {
                continue;
            }
            WParents[i].assignWeapon(w.weapon,lv);
            Debug.Log($"Gán vũ khí vào vị trí index: {i}, Tên Object: {WParents[i].gameObject.name}");
            return true;
        }
        return false;
    }

    public Weapon[] GetWeapons()
    {
        List<Weapon> weapons = new List<Weapon>();
        foreach (WeaponPosition w in WParents)
        {
            if(w.weapon != null)
                weapons.Add(w.weapon);
            else weapons.Add(null);
        }
        return weapons.ToArray();
    }

    public void Recyle(int index)
    {
        for (int i = 0; i < WParents.Length; i++)
        {
            if(i !=index) continue;
            int recyclePrice = WParents[i].weapon.getSellPrice();
            CurrencyManager.instance.AddCurrency(recyclePrice);
            WParents[i].removeWeapon();
            return;
        }
        Debug.Log("recycle at" +  index);
        
    }
}