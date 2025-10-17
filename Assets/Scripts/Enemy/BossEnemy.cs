using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Item Settings")]
    [Tooltip("Set this value between 0 to 1")][SerializeField] private float dropHealChance = 0.3f;
    [SerializeField] private GameObject healItem;

    public override void OnTakeDamage(int damage)
    {
        base.OnTakeDamage(damage);

        if (Random.value <= dropHealChance)
            Instantiate(healItem, transform.position, Quaternion.identity);
    }
}
