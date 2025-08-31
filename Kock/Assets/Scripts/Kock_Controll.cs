using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kock_Controll : MonoBehaviour
{
    public float movementSpeed = 1;
    private bool isFacingRight = true;
    private Vector2 movement = Vector2.zero;
    public Rigidbody2D rb;

    //Animation
    public Animator animator;
    string currentState;
    const string ANIM_K_IDLE = "K_Idle";
    const string ANIM_K_WALK = "K_Walk";
    const string ANIM_K_ATTACK = "K_Attack";
    public bool attackFlag = false;
    private float attackDelay;
    const string ANIM_K_DODGE = "K_Dodge";
    public bool dodgeFlag = false;
    private float dodgeDelay;
    const string ANIM_K_CHARGE = "K_Charge";
    const string ANIM_K_DEATH = "K_Death";


    private void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            attackFlag = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            dodgeFlag = true;
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x * movementSpeed, movement.y * movementSpeed);

        if (!IsAnimationPlaying(animator, ANIM_K_ATTACK) || !IsAnimationPlaying(animator, ANIM_K_DODGE))
        {
            if (movement.x != 0 || movement.y != 0)
            {
                ChangeAnimationState(ANIM_K_WALK);
            }
            else if (!IsAnimationPlaying(animator, ANIM_K_IDLE))
            {
                ChangeAnimationState(ANIM_K_IDLE);
            }
        }


        if (attackFlag)
        {
            if (!IsAnimationPlaying(animator, ANIM_K_ATTACK) || !IsAnimationPlaying(animator, ANIM_K_DODGE))
            {
                ChangeAnimationState(ANIM_K_ATTACK);
            }

            attackDelay = animator.GetCurrentAnimatorClipInfo(0).Length;
            Invoke("AttackAnim", attackDelay);
        }

        if (dodgeFlag)
        {
            if (!IsAnimationPlaying(animator, ANIM_K_DODGE) || !IsAnimationPlaying(animator, ANIM_K_ATTACK))
            {
                ChangeAnimationState(ANIM_K_DODGE);
            }

            dodgeDelay = animator.GetCurrentAnimatorClipInfo(0).Length;
            Invoke("DodgeAnim", dodgeDelay);
        }
    }

    private void AttackAnim()
    {
        attackFlag = false;
    }

    private void DodgeAnim()
    {
        dodgeFlag = false;
    }

    private void ChangeAnimationState(string newState)
    {
        if(newState == currentState)
        {
            return;
        }
        animator.Play(newState);
        currentState = newState;
    }

    public bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Flip()
    {
        if(isFacingRight && movement.x < 0f || !isFacingRight && movement.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    
}

