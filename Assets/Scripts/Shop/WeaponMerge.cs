using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMerge : MonoBehaviour
{
    public static WeaponMerge instance;
    [SerializeField] private PlayerWeapon playerWeapon;
    private List<Weapon> weaponsToMerger = new List<Weapon>();
    public static Action<Weapon> onMerge;
    void Awake()
    {
        instance = this;
    }
    public bool CanMerge(Weapon w)
    {
        Weapon[] weapons = playerWeapon.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null || weapons[i].Level >= 3) continue;

            for (int j = i + 1; j < weapons.Length; j++)
            {
                if (weapons[j] == null) continue;

                // Kiểm tra trùng Tên và trùng Level
                if (weapons[i].weaponData.WeaponName == weapons[j].weaponData.WeaponName && 
                    weapons[i].Level == weapons[j].Level)
                {
                    Debug.Log($"Tìm thấy cặp trùng: {weapons[i].weaponData.WeaponName} tại vị trí {i} và {j}");

                    weaponsToMerger.Clear();
                    weaponsToMerger.Add(weapons[i]);
                    weaponsToMerger.Add(weapons[j]);
                    Debug.Log(weaponsToMerger);
                    return true; 
                }
            }
    }
        return false;
    }
    public void Merge()
    {
        // if(weaponsToMerger.Count < 2)
        // {
        //     return;
        // }
        // Destroy(weaponsToMerger[0].gameObject);

        weaponsToMerger[0].upgrade();

        // Weapon weapon = weaponsToMerger[1];
        // weaponsToMerger.Clear();
        // onMerge?.Invoke(weapon);
    }
}
