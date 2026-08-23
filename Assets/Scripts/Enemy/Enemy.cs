using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float MaxHealth = 20f;
    [SerializeField] private ParticleSystem deathEffect;
    private float health;
    public static event Action<Vector2> onDying;
    public static event Action<float,Vector2> onTakeDamage;
    void OnEnable()
    {
        ResetHealth();
    }

    private void ResetHealth()
    {
        float rate = 1f;
        if (GameManager.instance != null)
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
        health = MaxHealth * rate;
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
        StartCoroutine(waitDeath());
    }
    private IEnumerator waitDeath()
    {
        onDying?.Invoke(transform.position);  
        deathEffect.gameObject.SetActive(true);
        deathEffect.Play();
        yield return new WaitForEndOfFrame();
        EnemyPoolManager.Instance.Despawn(gameObject);
    }
}
