using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private void OnTriggerEnter2D(Collider2D collision) {
        Damageable enemy = collision.GetComponent<Damageable>();

        if (enemy != null)
            enemy.TakeDamage(damage);
    }
}
