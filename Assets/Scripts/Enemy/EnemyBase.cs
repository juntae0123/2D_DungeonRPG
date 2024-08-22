using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour
{
    public int attackDamage = 10; // 이 필드를 자식 클래스에서 새로 정의하지 않습니다.
    public int maxHealth = 100;
    protected int currentHealth;

    public float moveSpeed = 2.0f;
    public float attackRange = 2.0f;

    protected Animator animator;
    protected Transform player;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        MoveTowardsPlayer();
        CheckAttack();
    }

    protected void MoveTowardsPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer > attackRange)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }

    protected void CheckAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        animator.SetTrigger("AttackTrigger");
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        animator.SetTrigger("Die");
        gameObject.SetActive(false);
    }
}