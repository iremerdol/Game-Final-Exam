using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour, IHealthObserver
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed;
    
    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    
    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Attack Sound")]
    [SerializeField] private AudioClip attackSound;
    
    private Animator anim;
    private Health playerHealth;
    private Transform player;
    private Rigidbody2D rb;
    private bool facingRight = true; // Track the direction the enemy is facing
    private Health health;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = GetComponent<Health>();
        health.AddObserver(this); // Add this boss as an observer to its own health
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            Debug.Log("Player in sight");
            if (cooldownTimer >= attackCooldown && playerHealth.GetCurrentHealth() > 0)
            {
                anim.ResetTrigger("moving");
                // Start attack
                Debug.Log("Attacking player");
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
                SoundManager.Instance.PlaySound(attackSound);
                rb.velocity = Vector2.zero; // Stop moving while attacking
            }
        }
        else
        {
            FollowPlayer();
        }

        // Flip the enemy to face the player
        FlipTowardsPlayer();
    }

    private void FollowPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
            anim.SetTrigger("moving");
        }
    }

    private void FlipTowardsPlayer()
    {
        if (player != null)
        {
            if ((player.position.x > transform.position.x && !facingRight) || (player.position.x < transform.position.x && facingRight))
            {
                facingRight = !facingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
            Debug.Log("Player detected with health: " + playerHealth.GetCurrentHealth());
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    // This method should be called by an animation event
    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            Debug.Log("Dealing damage to player");
            playerHealth.TakeDamage(damage);
        }
    }

    // IHealthObserver implementation
    public void OnHealthChanged(float currentHealth)
    {
        // Optionally handle health changes here if needed
    }

    public void OnPlayerDied()
    {
        // Notify EnemyManager when this boss dies
        EnemyManager.Instance.CheckEnemiesAndLoadScene();
    }

    public void OnPlayerRespawned(Transform checkpointParent)
    {
        // Optionally handle respawn logic here if needed
    }
}
