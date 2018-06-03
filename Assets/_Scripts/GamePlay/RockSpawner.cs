/*---------------------------------------------------------
File Name: RockSpawner.cs
Purpose: Controls the spawning of rocks in a rock row.
Author: Heath Parkes (gargit@gargit.net)
Modified: 2018-06-03
-----------------------------------------------------------
Copyright 2018 AIE/HP
---------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour {

    //rock Game Object
    public GameObject m_RockType;

    /// <summary>
    /// sets initial variables.
    /// </summary>
    /// <param name="lanesize">width of the playable area</param>
    /// <param name="lanebuffersize">each sides non-playable area</param>
    /// <param name="RowYPos">number of the row this instance belongs to</param>
    public void Init(int lanesize, int lanebuffersize, int RowYPos)
    {
        //determine number of rocks to spawn
        int rocksToSpawn = Random.Range(0, (lanesize / 3)) + 1;

        //spawn rocks
        while (rocksToSpawn > 0)
        {
            //picks a block in the lane to put a rock on
            int rockSpawnSpot = Random.Range(-(lanesize/2)+1,(lanesize/2)+2);
            //remove the movement node
            gameObject.GetComponent<RowCreator>().RemoveMovementNodeAt(rockSpawnSpot);
            //put the rock in
            GameObject spawnedRock = Instantiate(m_RockType, new Vector3(rockSpawnSpot, 0, RowYPos), Quaternion.identity);
            spawnedRock.gameObject.transform.parent = gameObject.transform;

            //decrement amount of rocks to spawn
            rocksToSpawn--;
        }
    }
}
