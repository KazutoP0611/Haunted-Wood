using System;
using System.Collections.Generic;
using UnityEngine;

public class BoxRandomer : MonoBehaviour
{
    [SerializeField] private Transform[] boxSpawnerPoint;
    [SerializeField] private Transform spawnParent;
    [Space]
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private Vector2 healItemAmount;
    [SerializeField] private GameObject healItem;
    [Space]
    [SerializeField] private GameObject kingBoxPrefab;
    [SerializeField] private GameObject keyPrefab;

    [Header("For Debug Only")]
    [SerializeField] private bool testBoss;
    [SerializeField] private Transform kingBoxSpawnPoint;
    
    private bool keyIsSpawned = false;

    public void SpawnItems()
    {
        if(testBoss)
        {
            Box box = Instantiate(kingBoxPrefab, kingBoxSpawnPoint.position, Quaternion.identity, spawnParent).GetComponent<Box>();
            box.SetItemToDrop(keyPrefab);
        }
        else
        {
            int spawnHealItemAmount = UnityEngine.Random.Range((int)healItemAmount.x, (int)healItemAmount.y + 1);
            Debug.Log($"There should be {spawnHealItemAmount} pieces in this map.");

            //List<int> enemySpawnPosIndexList = new List<int>();

            List<int> tempList = new List<int>();
            for (int i = 0; i < boxSpawnerPoint.Length; i++)
                tempList.Add(i);

            for (int i = 0; i < boxSpawnerPoint.Length; i++)
            {
                int numberIndex = UnityEngine.Random.Range(0, tempList.Count);

                if (i == boxSpawnerPoint.Length - 1 && !keyIsSpawned)
                {
                    Box box = Instantiate(kingBoxPrefab, boxSpawnerPoint[tempList[numberIndex]].position, Quaternion.identity, spawnParent).GetComponent<Box>();
                    box.SetItemToDrop(keyPrefab);
                    keyIsSpawned = true;
                    tempList.RemoveAt(numberIndex);
                    continue;
                }

                if (UnityEngine.Random.value < 0.1f && !keyIsSpawned)
                {
                    Box box = Instantiate(kingBoxPrefab, boxSpawnerPoint[tempList[numberIndex]].position, Quaternion.identity, spawnParent).GetComponent<Box>();
                    box.SetItemToDrop(keyPrefab);
                    keyIsSpawned = true;
                    tempList.RemoveAt(numberIndex);
                }
                else
                {
                    Box box = Instantiate(boxPrefab, boxSpawnerPoint[tempList[numberIndex]].position, Quaternion.identity, spawnParent).GetComponent<Box>();

                    bool shouldSpawnHealItem = false;
                    if (spawnHealItemAmount > 0)
                        shouldSpawnHealItem = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));

                    if (i == boxSpawnerPoint.Length - 1 && spawnHealItemAmount > 0)
                        shouldSpawnHealItem = true;

                    if (shouldSpawnHealItem)
                    {
                        box.SetItemToDrop(healItem);
                        spawnHealItemAmount--;
                    }

                    tempList.RemoveAt(numberIndex);
                }
            }
        }
    }
}
