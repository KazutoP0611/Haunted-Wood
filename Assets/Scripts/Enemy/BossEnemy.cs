using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Item Settings")]
    [Tooltip("Set this value in percentage (%).")][SerializeField] private float dropHealChance = 30;
    [SerializeField] private GameObject healItem; 

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (Random.value <= dropHealChance / 100.0f)
            Instantiate(healItem, transform.position, Quaternion.identity);
    }
}
