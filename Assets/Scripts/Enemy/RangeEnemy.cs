using System;
using UnityEngine;
using UnityEngine.Pool;

public class RangeEnemy : MonoBehaviour
{
    private Player player;
    private RangedAttack rangedAttack;
    [Header("General Settings")]
    [SerializeField] private float speed = 3.0f;
    public float detectionDistance = 1.5f; 
    public float obstacleAvoidanceWeight = 2.0f; // Độ ưu tiên né vật cản
    public LayerMask obstacleLayer;

    [Header("Ranged Enemy Settings")]
    [SerializeField] private float rangePlayerDetectionRange = 10f;
    [SerializeField] private Bullet projectile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        rangedAttack = GetComponent<RangedAttack>();

        if (player == null)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        ManageAttack();
    }
    private void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= rangePlayerDetectionRange)
        {
            TryAttackPlayer();
        }else
        {
            MoveTowardsPlayer();
        }
    }
    private void TryAttackPlayer()
    {
        rangedAttack.AimTowardsPlayer();   
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

        rangedAttack.Flip(moveDirection);

        transform.position = newPosition;

        Debug.DrawRay(currentPos, moveDirection * detectionDistance, Color.green);
        
    }
   
   
    void OnDrawGizmos()
    {
        Gizmos.color = Color.beige;
        Gizmos.DrawWireSphere(transform.position, rangePlayerDetectionRange);
    }
}
