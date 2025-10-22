using UnityEngine;

public class BossAppearAnimTrigger : MonoBehaviour
{
    private void SpawnBoss()
    {
        GameplaySceneController.instance.SpawnBossEnemy();
    }

    private void OnEventFinished()
    {
        GameplaySceneController.instance.BossAppearanceEventFinished();
    }
}
