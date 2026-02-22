using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;
    private Player player;
    [Header("General Settings")]
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float playerDetectionRange = 5.0f;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float attackRate;
    [SerializeField] private Animator animator;
    public float detectionDistance = 2.5f; 
    public float obstacleAvoidanceWeight = 2.0f; // Độ ưu tiên né vật cản
    public LayerMask obstacleLayer;
    private float attackDelay;
    private float attackTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
            Destroy(gameObject);
        }
        attackDelay = 1f / attackRate;
    }
    void Awake()
    {
        instance = this;
        
    }
    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
        if (attackTimer >= attackDelay)
        {
            TryAttackPlayer();
        }
        else
        {
            WaitAttackDelay();
        }
    }
    private void TryAttackPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
    
        if (distanceToPlayer <= playerDetectionRange)
        {
            AttackPlayer();
        }            
    
    }
    private void WaitAttackDelay()
    {
        attackTimer += Time.deltaTime;
    }
    private void AttackPlayer()
    {
        animator.SetBool("Attack",true);
        player.TakeDamage(damage);   
        attackTimer = 0f;
       
    }
    private void MoveTowardsPlayer()
    {
        Vector2 targetPos = player.getCenter();
        Vector2 currentPos = transform.position;
        
        Vector2 moveDirection = (targetPos - currentPos).normalized;

        RaycastHit2D hit = Physics2D.Raycast(currentPos, moveDirection, detectionDistance, obstacleLayer);
        
        if (hit.collider != null) {
            Vector2 avoidDirection = Vector2.Perpendicular(hit.normal); 
            
            if (Vector2.Dot(avoidDirection, moveDirection) < 0) {
                avoidDirection = -avoidDirection;
            }

            moveDirection = (moveDirection + avoidDirection * obstacleAvoidanceWeight).normalized;
        }

        Vector2 newPosition = (Vector2)transform.position + (moveDirection * speed * Time.deltaTime);

        if (moveDirection.x > 0) {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y);
        } 
        else if (moveDirection.x < 0) {
            transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y);
        }

        transform.position = newPosition;

        Debug.DrawRay(currentPos, moveDirection * detectionDistance, Color.green);
    }   

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
    }
   
}
