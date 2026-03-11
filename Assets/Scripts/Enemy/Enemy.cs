using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float MaxHealth = 20f;
    [SerializeField] private ParticleSystem deathEffect;
    private float health;
    public static event Action<Vector2> onDying;
    public static event Action<float,Vector2> onTakeDamage;
    void Start()
    {
        switch (GameManager.instance.currentDiff)
        {
            case Difficulty.EASY :
                setMaxHealthByRate(1f);
                break;
            case Difficulty.NORMAL :
                setMaxHealthByRate(1.5f);
                break;
            case Difficulty.HARD :
                setMaxHealthByRate(2f);
                break;
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
        onTakeDamage?.Invoke(damage,transform.position);
    }
    private void setMaxHealthByRate(float rate)
    {
        MaxHealth *= rate;
        health = MaxHealth;
    }

    private void Death()
    {
        onDying?.Invoke(transform.position);  
        deathEffect.Play();
        deathEffect.transform.SetParent(null);
        Destroy(gameObject);    
    }

}
