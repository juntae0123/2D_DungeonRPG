using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMove : PlayerActions
{
    public float moveSpeed = 5.0f;
    public float dashSpeed = 15.0f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 100.0f;
    private bool isFacingRight = true;
    private Vector2 moveDirection;
    private bool isDashing = false;
    private float lastDashTime;
    private void Update()
    {
        if (!isDashing)
        {
            // 방향키 입력을 직접 읽어 이동 방향을 설정합니다.
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

            // 매 프레임마다 이동을 처리합니다.
            Move(moveDirection);
        }
    }
    protected override void OnDash(InputAction.CallbackContext context)
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
    protected override void OnAttack(InputAction.CallbackContext context)
    {
        if (animator != null)
        {
            ResetAnimatorTriggers(); // 모든 트리거 초기화
            animator.SetTrigger("AttackTrigger");
        }
    }

    protected override void OnPowerAttack(InputAction.CallbackContext context)
    {
        if (animator != null)
        {
            ResetAnimatorTriggers(); // 모든 트리거 초기화
            animator.SetTrigger("PowerAttackTrigger");
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
}