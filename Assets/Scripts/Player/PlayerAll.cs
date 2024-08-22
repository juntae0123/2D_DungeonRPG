using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAll : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public int attackDamage = 30;
    public int powerAttackDamage = 60;
    public float powerAttackCooldown = 5f;
    private bool canPowerAttack = true;

    public float moveSpeed = 5.0f;
    public float dashSpeed = 15.0f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 1.0f;
    private bool isFacingRight = true;
    private Vector2 moveDirection;
    private bool isDashing = false;
    private float lastDashTime;

    private PlayerInputActions inputActions;
    private Animator animator;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.OnMove.Move.performed += OnMove;
        inputActions.OnMove.Dash.performed += OnDash;
        inputActions.OnMove.Attack.performed += OnAttack;
        inputActions.OnMove.PowerAttack.performed += OnPowerAttack;
    }

    private void OnDisable()
    {
        inputActions.OnMove.Move.performed -= OnMove;
        inputActions.OnMove.Dash.performed -= OnDash;
        inputActions.OnMove.Attack.performed -= OnAttack;
        inputActions.OnMove.PowerAttack.performed -= OnPowerAttack;
        inputActions.Disable();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        ResetAnimatorTriggers();
    }

    private void Update()
    {
        if (!isDashing)
        {
            moveDirection = Vector2.zero;

            if (Keyboard.current.upArrowKey.isPressed)
            {
                moveDirection += Vector2.up;
            }
            if (Keyboard.current.downArrowKey.isPressed)
            {
                moveDirection += Vector2.down;
            }
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                moveDirection += Vector2.left;
            }
            if (Keyboard.current.rightArrowKey.isPressed)
            {
                moveDirection += Vector2.right;
            }

            Move(moveDirection);
        }
    }

    private void Move(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            Vector3 moveVector = new Vector3(direction.x, direction.y, 0);
            transform.position += Time.deltaTime * moveSpeed * moveVector;

            if (direction.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (direction.x < 0 && isFacingRight)
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

    private void OnMove(InputAction.CallbackContext context)
    {
        // 이동 입력 처리
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (Time.time - lastDashTime >= dashCooldown)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            Vector3 dashVector = new Vector3(moveDirection.x, moveDirection.y, 0).normalized * dashSpeed;
            transform.position += dashVector * Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        lastDashTime = Time.time;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (animator != null)
        {
            ResetAnimatorTriggers();
            animator.SetTrigger("AttackTrigger"); // 올바른 트리거 이름 사용
            Attack();
        }
    }

    private void OnPowerAttack(InputAction.CallbackContext context)
    {
        if (canPowerAttack && animator != null)
        {
            ResetAnimatorTriggers();
            animator.SetTrigger("PowerAttackTrigger"); // 올바른 트리거 이름 사용
            PowerAttack();
        }
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 2.0f, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in hitEnemies)
        {
            Boss boss = enemy.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(attackDamage);
                Debug.Log($"플레이어가 보스에게 {attackDamage} 데미지를 입혔습니다.");
            }
        }

        animator.SetTrigger("AttackTrigger"); // 여기서도 올바른 트리거 이름 사용
    }

    private void PowerAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1.0f, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in hitEnemies)
        {
            Boss boss = enemy.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(powerAttackDamage);
                Debug.Log($"플레이어가 보스에게 {powerAttackDamage} 데미지를 입혔습니다.");
            }
        }

        animator.SetTrigger("PowerAttackTrigger"); // 여기서도 올바른 트리거 이름 사용
        canPowerAttack = false;
        Invoke("ResetPowerAttack", powerAttackCooldown);
    }

    private void ResetPowerAttack()
    {
        canPowerAttack = true;
    }

    private void ResetAnimatorTriggers()
    {
        if (animator != null)
        {
            animator.ResetTrigger("AttackTrigger");
            animator.ResetTrigger("PowerAttackTrigger");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"플레이어가 {damage} 데미지를 입었습니다. 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("플레이어가 사망했습니다.");
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }
}