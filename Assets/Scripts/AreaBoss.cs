using System.Collections;
using UnityEngine;

public class AreaBoss : Area
{
    public delegate void OnEnterBossRoom(AreaBoss bossRoom);
    private OnEnterBossRoom onEnterBossRoomCallback;

    [SerializeField] private Transform enemySpawnPoint;

    public Transform GetEnemySpawnPoint { get { return enemySpawnPoint; } }

    public void InitAreaBoss(OnEnterBossRoom onEnterBossRoomCallback)
    {
        this.onEnterBossRoomCallback = onEnterBossRoomCallback;
    }

    public override void EnterArea()
    {
        base.EnterArea();
    }

    public override void Update()
    {
        base.Update();

        if (player != null)
        {
            if (!roomIsActivated)
            {
                //Debug.LogWarning((player.transform.position - transform.position).magnitude);
                if ((player.transform.position - transform.position).magnitude <= distanceFromRoomUntilEnter)
                {
                    roomIsActivated = true;
                    onEnterBossRoomCallback?.Invoke(this);
                }
            }
        }
    }

    public override void ExitArea()
    {
        base.ExitArea();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            player = collision.GetComponent<Player>();
    }
}
