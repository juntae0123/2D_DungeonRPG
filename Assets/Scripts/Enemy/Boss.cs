using UnityEngine;
using System.Collections;

public class Boss : EnemyBase
{
    public int powerAttackDamage = 50;

    public float attackCooldown = .0f;       // 일반 공격 쿨타임 5초
    public float powerAttackCooldown = 20.0f; // 강공 쿨타임 20초

    private float lastAttackTime;
    private float lastPowerAttackTime;

    private bool isFacingRight = true;  // 보스가 바라보는 방향

    protected override void Start()
    {
        maxHealth = 1000;
        moveSpeed = 0.5f;
        attackRange = 3.0f; // 공격 범위 설정

        lastAttackTime = -attackCooldown; // 게임 시작과 동시에 공격할 수 있도록 설정
        lastPowerAttackTime = -powerAttackCooldown; // 게임 시작과 동시에 강공할 수 있도록 설정

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        FacePlayer();  // 플레이어를 향해 보스가 바라보도록
        AttemptAttack(); // 공격 시도
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
        if (Time.time - lastPowerAttackTime >= powerAttackCooldown)
        {
            PowerAttack();
            lastPowerAttackTime = Time.time;  // 강공 공격 후 마지막 강공 시간 갱신
        }
        else if (Time.time - lastAttackTime >= attackCooldown)
        {
            PerformLightAttack();
            lastAttackTime = Time.time;  // 일반 공격 후 마지막 공격 시간 갱신
        }
    }

    private void PerformLightAttack()
    {
        Debug.Log("보스가 일반 공격을 합니다.");
        animator.SetTrigger("AttackTrigger");
    }

    private void PowerAttack()
    {
        Debug.Log("보스가 강공을 합니다.");
        animator.SetTrigger("PowerAttackTrigger");
    }

    public void ApplyDamage()
    {
        Debug.Log("보스가 공격을 시도합니다.");

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Player"));

        foreach (Collider2D player in hitPlayers)
        {
            PlayerAll playerController = player.GetComponent<PlayerAll>();
            if (playerController != null)
            {
                playerController.TakeDamage(attackDamage);
                Debug.Log($"보스가 플레이어에게 {attackDamage} 데미지를 입혔습니다.");
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"보스가 {damage} 데미지를 입었습니다. 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        Debug.Log("보스가 죽습니다.");
        animator.SetTrigger("DieTrigger");
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerAll player = collision.gameObject.GetComponent<PlayerAll>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);
                Debug.Log($"보스가 충돌로 플레이어에게 {attackDamage} 데미지를 입혔습니다.");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}