using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BoxRandomer : MonoBehaviour
{
    [SerializeField] private Transform[] boxSpawnerPoint;
    [SerializeField] private Transform spawnParent;

    [Header("Box Spawner Mode")]
    [Tooltip("If true, this scipt will randomly place healing item, so some boxes may empty. Uncheck it, and the box script will be the one take care of random chance if healing item will be spawn or not.")]
    [SerializeField] private bool fixedBoxSpawnAmount;
    [SerializeField] private Vector2 healItemAmount;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject healItem;
    [Space]
    [SerializeField] private GameObject kingBoxPrefab;
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private float kingboxSpawnChance = 0.06f;

    [Header("For Debug Only")]
    [SerializeField] private bool testBoss;
    [SerializeField] private Transform kingBoxSpawnPoint;

    private bool keyIsSpawned = false;

    public void SpawnItems()
    {
        if (testBoss)
        {
            Box box = Instantiate(kingBoxPrefab, kingBoxSpawnPoint.position, Quaternion.identity, spawnParent).GetComponent<Box>();
            box.SetItemToDrop(keyPrefab);
        }
        else
        {
            int spawnHealItemAmount = UnityEngine.Random.Range((int)healItemAmount.x, (int)healItemAmount.y + 1);
            Debug.Log($"If \"fixedBoxSpawnAmount\" is true, There should be {spawnHealItemAmount} pieces in this map.");

            //List<int> enemySpawnPosIndexList = new List<int>();

            List<int> tempList = new List<int>();
            for (int i = 0; i < boxSpawnerPoint.Length; i++)
                tempList.Add(i);

            //Random spawn item box as the same amount as set box spawner's points.
            for (int i = 0; i < boxSpawnerPoint.Length; i++)
            {
                //random the number from the amount of transform in templist, after spawned box in randomed transform, the script will remove that transform from list, so next loop will not be in the same place.
                int numberIndex = UnityEngine.Random.Range(0, tempList.Count);

                //If this loop is the last loop, and king's room key hasn't been spawned yet, this will force setting the last box as the king's box.
                if (i == boxSpawnerPoint.Length - 1 && !keyIsSpawned)
                {
                    Box box = Instantiate(kingBoxPrefab, boxSpawnerPoint[tempList[numberIndex]].position, Quaternion.identity, spawnParent).GetComponent<Box>();
                    box.SetItemToDrop(keyPrefab);
                    keyIsSpawned = true;
                    tempList.RemoveAt(numberIndex);
                    continue;
                }

                //Random 10% chance for spawning king's room's key. so the key will not be easily spawned at the first half loop.
                if (UnityEngine.Random.value < kingboxSpawnChance && !keyIsSpawned)
                {
                    Box box = Instantiate(kingBoxPrefab, boxSpawnerPoint[tempList[numberIndex]].position, Quaternion.identity, spawnParent).GetComponent<Box>();
                    box.SetItemToDrop(keyPrefab);
                    keyIsSpawned = true;
                    tempList.RemoveAt(numberIndex);
                }
                else
                {
                    Box box = Instantiate(boxPrefab, boxSpawnerPoint[tempList[numberIndex]].position, Quaternion.identity, spawnParent).GetComponent<Box>();

                    if (fixedBoxSpawnAmount)
                    {
                        #region First method for setting item, random if this box will have item in it or not. But this have flaw that it may not set key as the same amount as it should be
                        ////Random 50% chance if this box should have healing item in it.
                        //bool shouldSpawnHealItem = false;
                        //if (spawnHealItemAmount > 0)
                        //    shouldSpawnHealItem = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));

                        ////If this is the last loop, and there are sill healing item that should be spawn left, this script will force set healing item in the box.
                        //if (i == boxSpawnerPoint.Length - 1 && spawnHealItemAmount > 0)
                        //    shouldSpawnHealItem = true;

                        //if (shouldSpawnHealItem)
                        //{
                        //    box.SetItemToDrop(healItem);
                        //    spawnHealItemAmount--;
                        //}
                        #endregion

                        #region Second methosd for setting item, set this box to have item anyway. This will make sure that there will be the same amount of healing item as it has been randomed.
                        //This will set healing item no matter what. The position of the box is random anyway, so I don't think they will be too close to each other.
                        if (spawnHealItemAmount > 0)
                        {
                            box.SetItemToDrop(healItem);
                            spawnHealItemAmount--;
                        }
                        #endregion
                    }
                    else
                    {
                        box.SetItemToDrop(healItem);
                        box.SetAutoRandomToDrop(true);
                    }

                    tempList.RemoveAt(numberIndex);
                }
            }
        }
    }
}
