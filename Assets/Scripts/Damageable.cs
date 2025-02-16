using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public delegate void DamgeTakenAction(float hp, String tag);
    public static event DamgeTakenAction OnDamageTaken;

    [SerializeField] private int hp = 100;

    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage) {
        hp -= damage;
        anim.SetTrigger("hurt");
        OnDamageTaken?.Invoke(hp, gameObject.tag);
    }
}
