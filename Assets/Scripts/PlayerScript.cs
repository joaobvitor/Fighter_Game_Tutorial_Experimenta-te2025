using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class PlayerScript : MonoBehaviour
{
    private float movement;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 10;

    private enum AnimationState { Idle, Walking, Jumping, Falling};

    private bool canMove { 
        get {
            return animator.GetBool("canMove");
        }
    }

    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator animator;
    private LayerMask groundLayer;
    private Collider2D jabHitbox;
    private Collider2D kickHitbox;
    private PlayerInput inputController;
    private Damageable damageable;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Ground");
        jabHitbox = transform.Find("Jab").GetComponent<Collider2D>();
        kickHitbox = transform.Find("Kick").GetComponent<Collider2D>();
        inputController = GetComponent<PlayerInput>();
        damageable = GetComponent<Damageable>();
    }

    private void OnMovement(InputValue value) {
        movement = value.Get<float>();
    }

    private void OnJump(InputValue value) {
        if (damageable.getInvulnerable() || !canMove)
            return;
        if (IsGrounded()) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void OnJab(InputValue value) {
        if (damageable.getInvulnerable() || !canMove)
            return;
        animator.SetTrigger("jab");
        StartCoroutine(jab());
    }

    IEnumerator jab() {
        StopMoving();
        yield return new WaitForSeconds(0.08f);
        jabHitbox.enabled = true;
        yield return new WaitForSeconds(0.12f);
        jabHitbox.enabled = false;
    }

    private void OnKick(InputValue value) {
        if (damageable.getInvulnerable() || !canMove)
            return;
        animator.SetTrigger("kick");
        StartCoroutine(kick());
    }

    IEnumerator kick() {
        StopMoving();
        yield return new WaitForSeconds(0.18f);
        kickHitbox.enabled = true;
        yield return new WaitForSeconds(0.2f);
        kickHitbox.enabled = false;
    }

    private void FixedUpdate() {
        if (damageable.getInvulnerable() || !canMove)
            return;
        rb.linearVelocity = new Vector2(movement * speed, rb.linearVelocity.y);
        UpdateFacingDirection();
        UpdateAnimationState();
    }

    public void StopMoving() {
        if (IsGrounded())
            rb.linearVelocity = new Vector2(0, 0);
    }

    private void UpdateAnimationState() {
        if (rb.linearVelocityY != 0 && !IsGrounded())
            animator.SetInteger("state", (int)AnimationState.Jumping);
        else if (rb.linearVelocityX == 0) {
            animator.SetInteger("state", (int)AnimationState.Idle);
        }
        else {
            animator.SetInteger("state", (int)AnimationState.Walking);
        }
    }

    private void UpdateFacingDirection() {
        if (movement != 0 && transform.localScale.x != movement)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 0);
    }

    private bool IsGrounded() {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
    }
}
