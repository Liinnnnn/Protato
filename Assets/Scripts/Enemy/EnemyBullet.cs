using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    private Player player;
    private RangedAttack rangedAttack;
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage;
    void Start()
    {
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<Player>();
    }
    void OnEnable()
    {
        StartCoroutine(backToPool());
    }
    private IEnumerator backToPool()
    {
        yield return new WaitForSeconds(5f);
        rangedAttack.releaseBullet(this);
    }
   
    public void Shoot(float damage,Vector2 dir)
    {
        this.damage = damage;
        transform.right = dir;
        rb.linearVelocity = dir * speed;
    }
    public void Configure(RangedAttack rangedAttack)
    {
        this.rangedAttack = rangedAttack;
    }
    public void reload()
    {
        rb.linearVelocity = Vector2.zero;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(10f); // Example damage value
                Debug.Log("Bullet hit the player and dealt damage.");
            }
            StopAllCoroutines();
            rangedAttack.releaseBullet(this);
        }
        
    }
}
