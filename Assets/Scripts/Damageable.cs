using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int hp = 100;

    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage) {
        hp -= damage;
        anim.SetTrigger("hurt");
    }
}
