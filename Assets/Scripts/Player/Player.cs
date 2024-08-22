using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public int AttackDamage = 30;
    public int PowerAttackDamage = 60;
    public float PowerAttackCooldown = 5f;
    public bool canPowerAttack = true;

    private Animator animator;
    private void Start()
    {
        currentHealth = 100;
    }
   
    void Attack()
    {
        Debug.Log($"기본공격으로 {AttackDamage}의 피해를 입혔다");
        animator.SetTrigger("Attack");
    }

    void PowerAttack()
    {
        Debug.Log($"강한공격으로 {PowerAttackDamage}의 데미지를 입혔다");
        animator.SetTrigger("PowerAttack");
        canPowerAttack = false;
        Invoke("ResetPowerAttack", PowerAttackCooldown);

    }

    void ResetPowerAttack()
    {
        canPowerAttack=true;
        Debug.Log("강한공격 준비완료");

    }






    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("플레이어 남은 체력:" + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die() 
    {
        Debug.Log("You DIe");

        //gameObject.SetActive(false);

    }




    }
