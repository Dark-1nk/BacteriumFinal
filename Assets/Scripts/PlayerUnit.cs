using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUnit : MonoBehaviour
{
    public float speed = 2.0f;
    public int health;
    public int maxHealth = 100;
    public int attackDamage = 10;
    public int towerDamage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.0f;
    public bool aoeAttack = false; // Attack all in range
    public Slider healthSlider;

    AudioManager audioManager;
    Animator animator;
    Rigidbody2D rb;

    private Transform targetUnit;
    private Transform targetTower;
    private float nextAttackTime = 0f;  // Cooldown timer

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        animator.SetBool("isWalking", false);
    }

    private void Update()
    {
        // Update health slider position
        if (healthSlider != null)
        {
            healthSlider.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        }

        bool unitsInRange = AnyEnemyInRange();
        bool towersInRange = AnyTowerInRange();

        if (!unitsInRange && !towersInRange)
        {
            // Move forward if no targets are in range
            rb.velocity = new Vector2(speed, rb.velocity.y);
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Stop moving if there are targets in range
            rb.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);

            // Attack if targets are in range and cooldown has passed
            if (Time.time >= nextAttackTime)
            {
                if (unitsInRange)
                {
                    if (aoeAttack)
                        AttackAllInRange();
                    else
                        AttackClosestEnemy();
                }
                else if (towersInRange)
                {
                    AttackTower();
                }
            }
        }
    }

    private bool AnyEnemyInRange()
    {
        targetUnit = null;
        float closestDistance = attackRange;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyUnit"))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                targetUnit = enemy.transform;
            }
        }
        return targetUnit != null;
    }

    private bool AnyTowerInRange()
    {
        GameObject enemyTower = GameObject.FindGameObjectWithTag("EnemyTower");
        if (enemyTower != null)
        {
            float towerX = enemyTower.transform.position.x;
            float unitX = transform.position.x;

            // Check if the absolute horizontal difference is within attackRange
            if (Mathf.Abs(unitX - towerX) <= attackRange)
            {
                targetTower = enemyTower.transform; // Set the tower as the target
                return true;
            }
        }
        return false;
    }

    private void AttackClosestEnemy()
    {
        if (targetUnit != null)
        {
            EnemyUnit enemy = targetUnit.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                animator.SetTrigger("Attack");
                enemy.TakeDamage(attackDamage);
                nextAttackTime = Time.time + attackCooldown;  // Set cooldown
            }
 
        }
    }

    private void AttackAllInRange()
    {
        // Gather all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyUnit");

        foreach (GameObject enemy in enemies)
        {
            // Check if enemy is within attack range
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= attackRange)
            {
                EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
                if (enemyUnit != null)
                {
                    animator.SetTrigger("Attack");
                    enemyUnit.TakeDamage(attackDamage);
                }
            }
        }

        nextAttackTime = Time.time + attackCooldown;  // Set cooldown

    }

    private void AttackTower()
    {
        if (targetTower != null)
        { 
            TowerHealth towerHealth = targetTower.GetComponent<TowerHealth>();
            if (towerHealth != null)
            {
                animator.SetTrigger("Attack");
                towerHealth.TakeDamage(towerDamage);
                nextAttackTime = Time.time + attackCooldown;  // Set cooldown
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;

        bool reachedHalfHealth = false;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        else if (health <= maxHealth / 2 && !reachedHalfHealth) 
        {
            reachedHalfHealth = true;
            animator.SetTrigger("Hit");
        }
        else if (reachedHalfHealth)
        {

        }
    }

    private void OnDestroy()
    {
        audioManager.PlaySFX(audioManager.death);
        animator.SetTrigger("Die");
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
