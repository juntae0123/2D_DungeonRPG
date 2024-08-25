using UnityEngine;
using System.Collections;

public class BowSkeleton : EnemyBase
{


    public float attackCooldown = 7.0f;


    private float lastAttackTime;


    private bool isFacingRight = true;

    protected override void Start()
    {
        maxHealth = 90;
        moveSpeed = 0.5f;
        attackRange = 7.0f;

        lastAttackTime = -attackCooldown;


        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        FacePlayer();
        AttemptAttack();
    }

    private void FacePlayer()
    {
        if (player != null)
        {
            if (player.position.x > transform.position.x && !isFacingRight)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && isFacingRight)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void AttemptAttack()
    {

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PerformLightAttack();
            lastAttackTime = Time.time;
        }
    }

    private void PerformLightAttack()
    {
        Debug.Log("보우가 일반공격을 합니다.");
        animator.SetTrigger("AttackTrigger");
    }


    public void ApplyDamage()
    {
        Debug.Log("보우가 공격을 시도합니다.");

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Player"));

        if (hitPlayers.Length > 0)
        {
            foreach (Collider2D player in hitPlayers)
            {
                PlayerAll playerController = player.GetComponent<PlayerAll>();
                if (playerController != null)
                {
                    playerController.TakeDamage(attackDamage);  // 공격 데미지 적용
                    Debug.Log($"보우가 플레이어에게 {attackDamage} 데미지를 입혔습니다.");
                }
            }
        }
        else
        {
            Debug.Log("보우가 공격이 플레이어에게 닿지 않았습니다.");
        }
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"보우가 {damage} 데미지를 입었습니다. 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        Debug.Log("보우 스켈레톤이 죽습니다.");

        // OnDeath 이벤트 호출
        InvokeOnDeath();

        // 애니메이션 실행 후 게임 오브젝트 삭제
        animator.SetTrigger("DieTrigger");
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        // 애니메이션 길이만큼 기다린 후 오브젝트 삭제
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject); // 오브젝트를 메모리에서 제거
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerAll player = collision.gameObject.GetComponent<PlayerAll>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);
                Debug.Log($"보우가 충돌로 플레이어에게 {attackDamage} 데미지를 입혔습니다.");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}