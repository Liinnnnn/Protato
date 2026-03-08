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
        health = MaxHealth;
        
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
    
    private void Death()
    {
        onDying?.Invoke(transform.position);  
        deathEffect.Play();
        deathEffect.transform.SetParent(null);
        Destroy(gameObject);    
    }   
}
