using UnityEngine;

public class BossAppearAnimTrigger : MonoBehaviour
{
    private void SpawnBoss()
    {
        GameController.instance.SpawnBossEnemy();
    }

    private void OnEventFinished()
    {
        GameController.instance.BossAppearanceEventFinished();
    }
}
