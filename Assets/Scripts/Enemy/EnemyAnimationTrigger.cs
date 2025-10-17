using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void StartAttacking()
    {
        enemy.AnimationTriggerAttack(true);
    }

    private void FinishedAttacking()
    {
        enemy.AnimationTriggerAttack(false);
    }

    private void PlayAttackSound()
    {
        enemy.PlayAttackSound();
    }

    private void PlayDeadSound()
    {
        enemy.PlayDeadSound();
    }

    private void TriggerAttack()
    {
        enemy.GetDetectedColliders();
    }

    private void Dead()
    {
        enemy.AfterDeathHandle();
    }
}
