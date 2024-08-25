using UnityEngine;
using System;
using System.Collections;

public class EnemyBase : MonoBehaviour
{
    public int attackDamage = 10;
    public int maxHealth = 100;
    protected int currentHealth;

    public float moveSpeed = 2.0f;
    public float attackRange = 2.0f;

    protected Animator animator;
    protected Transform player;

    // 몬스터가 죽을 때 호출되는 이벤트
    public static event Action OnDeath;

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

    // 이벤트 호출 메서드
    protected void InvokeOnDeath()
    {
        OnDeath?.Invoke();
    }

    protected virtual void Die()
    {
        Debug.Log("Enemy is dying.");

        // OnDeath 이벤트 호출
        InvokeOnDeath();

        Destroy(gameObject);  // 오브젝트를 제거하여 더 이상 존재하지 않게 만듦
    }
}