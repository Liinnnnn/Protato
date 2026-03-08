using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class RangeEnemy : MonoBehaviour
{
    private Player player;
    private RangedAttack rangedAttack;
    [Header("General Settings")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [Header("Ranged Enemy Settings")]
    [SerializeField] private float rangePlayerDetectionRange = 10f;
    // [SerializeField] private Bullet projectile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        rangedAttack = GetComponent<RangedAttack>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.stoppingDistance = rangePlayerDetectionRange;
        if (player == null)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if(!agent.isOnNavMesh) Destroy(gameObject);
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
        animator.SetBool("Move",false);
        rangedAttack.AimTowardsPlayer();
    }
   
    private void MoveTowardsPlayer()
    {
        
        animator.SetBool("Move",true);
        Vector2 targetPos = player.getCenter();
        Vector2 currentPos = transform.position;
        
        Vector2 moveDirection = (targetPos - currentPos).normalized;

        agent.SetDestination(targetPos);

        rangedAttack.Flip(moveDirection);
     
    }
    
   
    void OnDrawGizmos()
    {
        Gizmos.color = Color.beige;
        Gizmos.DrawWireSphere(transform.position, rangePlayerDetectionRange);
    }
}
