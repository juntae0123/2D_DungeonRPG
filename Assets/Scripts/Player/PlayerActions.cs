using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    protected PlayerInputActions inputActions;
    protected Animator animator;

    protected virtual void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    protected virtual void OnEnable()
    {
        inputActions.Enable();
        inputActions.OnMove.Move.performed += OnMove;
        inputActions.OnMove.Dash.performed += OnDash;
        inputActions.OnMove.Attack.performed += OnAttack;
        inputActions.OnMove.PowerAttack.performed += OnPowerAttack;
    }

    protected virtual void OnDisable()
    {
        inputActions.OnMove.Move.performed -= OnMove;
        inputActions.OnMove.Dash.performed -= OnDash;
        inputActions.OnMove.Attack.performed -= OnAttack;
        inputActions.OnMove.PowerAttack.performed -= OnPowerAttack;
        inputActions.Disable();
    }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        ResetAnimatorTriggers();
    }

    protected void ResetAnimatorTriggers()
    {
        if (animator != null)
        {
            animator.ResetTrigger("AttackTrigger");
            animator.ResetTrigger("PowerAttackTrigger");
        }
    }

    protected virtual void OnMove(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnDash(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnAttack(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnPowerAttack(InputAction.CallbackContext context)
    {
       
    }
}