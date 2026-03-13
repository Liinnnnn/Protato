using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
    [field: SerializeField] public Weapon weapon{get ; private set;}
    public void assignWeapon(Weapon w,int lv)
    {
        Weapon clone = Instantiate(w,transform);
    
        weapon = clone;

        weapon.UpgradeTo(lv);
        

        weapon.transform.localRotation = Quaternion.identity;
    }
    public void removeWeapon()
    {
        foreach (Transform w in transform)
        {
            Destroy(w.gameObject);
        }
        weapon = null;
    }
}
