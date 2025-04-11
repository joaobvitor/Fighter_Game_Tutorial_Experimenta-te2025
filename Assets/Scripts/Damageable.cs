using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Damageable : MonoBehaviour
{
    public delegate void DamgeTakenAction(float hp, String tag);
    public static event DamgeTakenAction OnDamageTaken;

    [SerializeField] private int hp = 100;
    private bool invulnerable = false;

    private Animator anim;
    private PlayerScript player;

    private void Awake() {
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerScript>();
    }

    public void TakeDamage(int damage) {
        if (invulnerable) 
            return;
        
        hp -= damage;
        if (hp <= 0) {
            anim.SetTrigger("death");
        }
        else {
            anim.SetTrigger("hurt");
            StartCoroutine(hurt());
        }
        OnDamageTaken?.Invoke(hp, gameObject.tag);
    }

    public Boolean getInvulnerable() {
        return invulnerable;
    }
    
    IEnumerator hurt() {
        invulnerable = true;
        player.StopMoving();
        yield return new WaitForSeconds(0.12f);
        invulnerable = false;
    }
}
