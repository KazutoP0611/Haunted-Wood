using System;
using System.Collections.Generic;
using UnityEngine;

public class AreaField : Area
{
    [SerializeField] private Transform[] enemySpawnPoints;

    private List<Enemy> listOfEnemy = new List<Enemy>();

    public void InitAreaField(OnPlayerEnterArea onPlayerEnterAreaCallback)
    {
        this.onPlayerEnterAreaCallback = onPlayerEnterAreaCallback;
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
                    onPlayerEnterAreaCallback?.Invoke(this);
                }
            }
        }
    }

    public override void EnterArea()
    {
        base.EnterArea();

        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        listOfEnemy.Clear();

        if (enemySpawnPoints.Length > 0)
        {
            List<int> enemySpawnPosIndexList = new List<int>();

            //Created a list of position number in order;
            List<int> tempList = new List<int>();
            for (int i = 0; i < enemySpawnPoints.Length; i++)
                tempList.Add(i);

            for (int i = 0; i < enemySpawnPoints.Length; i++)
            {
                //Random numbers of created list for spawn enemy;
                int numberIndex = UnityEngine.Random.Range(0, tempList.Count);

                if (i == enemySpawnPoints.Length - 1)
                {
                    bool spawnSkeleton = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
                    if (spawnSkeleton)
                    {
                        Enemy enemy = Instantiate(GameController.instance.EnemyGrimreaper, enemySpawnPoints[tempList[numberIndex]].position, Quaternion.identity, enemyParent).GetComponent<Enemy>();
                        listOfEnemy.Add(enemy);
                    }
                }
                else
                {
                    Enemy enemy = Instantiate(GameController.instance.EnemySkeleton, enemySpawnPoints[tempList[numberIndex]].position, Quaternion.identity, enemyParent).GetComponent<Enemy>();
                    listOfEnemy.Add(enemy);
                }

                tempList.RemoveAt(numberIndex);
            }
        }

        //SetActivateEnemy(true);
    }

    //private void SetActivateEnemy(bool activate)
    //{
    //    foreach (Enemy enemy in listOfEnemy)
    //        enemy.SetActionActiveEnemy(activate);
    //}
}
