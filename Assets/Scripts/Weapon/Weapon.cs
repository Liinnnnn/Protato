using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour,IPlayerStats
{
    [field: SerializeField] public WeaponDataSO weaponData{get;private set;}
    protected Animator animator;
    [Header("Weapon Properties")]
    [SerializeField] protected float damage;
    [SerializeField] protected float critChance;
    [SerializeField] protected float critDamageMult;
    [SerializeField] protected float range = 10f;
    [SerializeField] protected float attackDelay = 2f; 
    protected float attackTimer ;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected float aimLerp;
    [Header("LEVEL")]
    [field: SerializeField] public int Level {get;private set;}
    [Header("Audio")]
    [SerializeField] protected AudioSource audioSource;


    protected void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = weaponData.attackSounds;
    }
    protected void playAttackSound()
    {
        if(!AudioManager.instance.isSfxOn) return;
        audioSource.Play();
        audioSource.pitch = Random.Range(0.9f,1.05f);
    }
    protected Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        // Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude,FindObjectsSortMode.None);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);

        if (enemies.Length <= 0)
        {
            transform.up = Vector3.up;
            return null;
        }
        float minDistance = range;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy e = enemies[i].GetComponent<Enemy>();
            float distance = Vector2.Distance(transform.position, e.transform.position);
            if (distance < minDistance)
            {
                closestEnemy = e;
                minDistance = distance;
            }
        }
        
        return closestEnemy;
    }
    protected float getDamage(out bool isCrits)
    {
        isCrits = false;
        if (UnityEngine.Random.Range(0f,100f) <= critChance)
        {
            Debug.Log("Crits" + damage * critDamageMult);
            return damage * critDamageMult;
        }
        return damage;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    public abstract void updateStat(PlayerStatsManager playerStatsManager);
    protected void ConfigureStats()
    {
        Dictionary<Stats,float> calc = WeaponStatsCalculated.GetStats(weaponData,Level);

        damage = calc[Stats.Attack];
        attackDelay = 1f / calc[Stats.AttackSpeed];
        critChance = calc[Stats.CritChance];
        critDamageMult = calc[Stats.CritDamage];
    
        range = calc[Stats.Range];

        Debug.Log(Level);
    }

    public void UpgradeTo(int lv)
    {
        Level = lv;
        ConfigureStats();
    }
    public int getSellPrice()
    {
        return WeaponStatsCalculated.GetSellPrice(weaponData,Level);
    }
    public void upgrade()
    {
        UpgradeTo(Level+1);
    }
}
