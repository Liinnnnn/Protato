using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour,IPlayerStats
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float baseHp;
    private float MaxHealth;
    private float health ; 
    [SerializeField] private float armor;
    [SerializeField] private float lifeSteal;
    [SerializeField] private float dodge;
    [SerializeField] private float hpRecoverySpd;
    [SerializeField] private float hpRecoveryDuration;
    [SerializeField] private float hpRecoveryTimer;
    public static Player instance;
    private PlayerLevel playerLevel;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Collider2D playerCollider;
    public static Action onTakeDamge;

    void Start()
    {
        health = MaxHealth;
        healthBar.value = 1;
        playerLevel = GetComponent<PlayerLevel>();
        Enemy.onTakeDamage += stealHp;
    }
    void OnDestroy()
    {
        Enemy.onTakeDamage -= stealHp;
        
    }
    void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if(health < MaxHealth)
        {
            RecoverHP();
        }
    }

    private void RecoverHP()
    {
        hpRecoveryTimer += Time.deltaTime;
        if(hpRecoveryTimer >= hpRecoveryDuration)
        {
            hpRecoveryTimer = 0;
            float healed = Mathf.Min(0.1f, MaxHealth - health);
            health += healed;
            changeHealthBar();
        }
    }

    private void stealHp(float dam,Vector2 enemypos)
    {
        if(health >= MaxHealth)
        {
            return;
        }
        float stolenValue = dam * lifeSteal;
        float healthAdd = MathF.Min(stolenValue,MaxHealth - health);

        health += healthAdd;
        changeHealthBar();
    }
    public void TakeDamage(float damage)
    {
        if (shouldDodge()){
            Debug.Log("Dodge");
            return;
        }
        Debug.Log("Player took " + damage + " damage.");
        float realDamage = damage * Mathf.Clamp(1-(armor/1000),0,1000 );
        health -= realDamage;
        changeHealthBar();
        onTakeDamge?.Invoke();
        if (health <= 0)
        {
            Die();
        }
    }
    private void changeHealthBar()
    {
        healthBar.value = health / MaxHealth;
        hpText.text = health.ToString("F1") + " / " + MaxHealth.ToString();
    }   
    private void Die()
    {
        Time.timeScale =0;
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }
    public Vector2 getCenter()
    {
        return (Vector2)transform.position + playerCollider.offset;
    }
    public bool hasLevelUP()
    {
        return playerLevel.hasLevelUP();
    }
    public bool shouldDodge()
    {
        return UnityEngine.Random.Range(0f,100f) < dodge;
    }
    public void updateStat(PlayerStatsManager playerStatsManager)
    {
        float addedHp = playerStatsManager.GetStatsValue(Stats.MaxHp);
        MaxHealth = baseHp + addedHp;

        armor = playerStatsManager.GetStatsValue(Stats.Armor);

        health = MaxHealth;
        changeHealthBar();

        lifeSteal = playerStatsManager.GetStatsValue(Stats.LifeSteal) / 100;
        dodge = playerStatsManager.GetStatsValue(Stats.Dodge);
        hpRecoverySpd = Mathf.Max(0.0001f,playerStatsManager.GetStatsValue(Stats.HpRecoveryRate));
        hpRecoveryDuration = 1f / hpRecoverySpd;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.TryGetComponent(out ICollectibles collectibles))
        {
            if (!collision.IsTouching(playerCollider))
            {
                return;
            }
            if (collectibles is Chest)
            {
                collectibles.Collect(GetComponent<Player>());
            }
        }
    }
}
