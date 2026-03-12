using System;
using UnityEngine;
using UnityEngine.AI;

public class Melee : MonoBehaviour
{
    public static Melee instance;
    private Player player;
    [Header("General Settings")]
    [SerializeField] private float playerDetectionRange = 5.0f;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float attackRate;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    private float attackDelay;
    private float attackTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = FindFirstObjectByType<Player>();
        attackDelay = 1f / attackRate;

        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
            Destroy(gameObject);
        }
        
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
    }
    void Awake()
    {
        instance = this;
        
    }
    // Update is called once per frame
    void Update()
    {   
        if(!agent.isOnNavMesh) Destroy(gameObject);  
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
        animator.SetBool("Move",false);
        player.TakeDamage(damage);   
        attackTimer = 0f;
       
    }
    private void MoveTowardsPlayer()
    {
        animator.SetBool("Move",true);
        Vector2 targetPos = player.getCenter();
        Vector2 currentPos = transform.position;
        
        Vector2 moveDirection = (targetPos - currentPos).normalized;

        agent.SetDestination(targetPos);

        if (moveDirection.x > 0) {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y);
        } 
        else if (moveDirection.x < 0) {
            transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y);
        }
}   

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
    }
   
}
