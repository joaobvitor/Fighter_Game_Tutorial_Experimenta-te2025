using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Damageable : MonoBehaviour
{
    public delegate void DamgeTakenAction(float hp, String tag);
    public static event DamgeTakenAction OnDamageTaken;

    [SerializeField] private int hp = 100;

    public bool invulnerable {
        get {
            return animator.GetBool("invulnerable");
        }
    }

    private Animator animator;
    private PlayerScript player;

    private void Awake() {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerScript>();
    }

    public void TakeDamage(int damage) {
        if (invulnerable) 
            return;
        
        hp -= damage;
        if (hp <= 0) {
            animator.SetTrigger("death");
        }
        else {
            animator.SetTrigger("hurt");
            player.StopMoving();
        }
        OnDamageTaken?.Invoke(hp, gameObject.tag);
    }

    public Boolean getInvulnerable() {
        return invulnerable;
    }
}
