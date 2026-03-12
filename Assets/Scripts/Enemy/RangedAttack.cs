using System;
using UnityEngine;
using UnityEngine.Pool;

public class RangedAttack : MonoBehaviour
{
    [Header("Ranged Attack Settings")]
    private Player player;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float attackRate;

    private float attackDelay;
    private float attackTimer;
    [Header("Projectile Settings")]
    [SerializeField] private EnemyBullet projectile;
    [SerializeField] private Transform shootPoint;
    private ObjectPool<EnemyBullet> bulletPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        attackDelay = 1f / attackRate;
        switch (GameManager.instance.currentDiff)
        {
            case Difficulty.EASY :
                damage *= 1f;
                break;
            case Difficulty.NORMAL :
                damage *= 1.5f;
                break;
            case Difficulty.HARD :
                damage *= 2f;
                break;
        }
        bulletPool = new ObjectPool<EnemyBullet>(createFunc,actionOnGet,actionOnRelease,actionOnDestroy);
    }
    private EnemyBullet createFunc()
    {
        EnemyBullet bullet = Instantiate(projectile, transform.position, Quaternion.identity);
        bullet.Configure(this);
        return bullet;
    }
    private void actionOnGet(EnemyBullet bullet)
    {
        bullet.transform.position = shootPoint.position;
        bullet.reload();
        bullet.gameObject.SetActive(true);
    }
    private void actionOnRelease(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    private void actionOnDestroy(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }
    
    public void AimTowardsPlayer()
    {
        TryShooting();
    }
    public void releaseBullet(EnemyBullet bullet)
    {
        bulletPool.Release(bullet);
    }
    private void TryShooting()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDelay)
        {
            // player.TakeDamage(damage);
            ShootProjectile();
            attackTimer = 0f;
        } 
    }
    Vector2 gizmoDirection;
    private void ShootProjectile()
    {
        Vector2 direction = (player.getCenter() - (Vector2)shootPoint.position).normalized;  
        Flip(direction);       
        EnemyBullet bullet = bulletPool.Get();
        bullet.Shoot(damage, direction);
        gizmoDirection = direction;
    }
    public void Flip(Vector2 direction)
    {
        if (direction.x > 0) {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x),transform.localScale.y);
        } 
        else if (direction.x < 0) {
            transform.localScale = new Vector3(-Math.Abs(transform.localScale.x),transform.localScale.y);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(shootPoint.position, (Vector2)shootPoint.position + gizmoDirection * 8f);
    }
}
