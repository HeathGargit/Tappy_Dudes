/*---------------------------------------------------------
File Name: RC_EnemyRowCreator.cs
Purpose: Derived clss from RowCreator, is a row that spawns enemies
Author: Heath Parkes (gargit@gargit.net)
Modified: 2018-06-03
-----------------------------------------------------------
Copyright 2018 AIE/HP
---------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RC_EnemyRowCreator : RowCreator {

    /// <summary>
    /// Initialises variables for the class.
    /// </summary>
    /// <param name="buffersize">size of the non-playable gutter/buffer area each side of the playable area</param>
    /// <param name="lanesize">size of the playable area</param>
    /// <param name="rownumber">number of the row this creator is creating</param>
    public override void Init(int buffersize, int lanesize, int rownumber)
    {
        //call the base method
        base.Init(buffersize, lanesize, rownumber);

        //set up this classes unique stuff
        GetComponent<EnemySpawner>().Init(new Vector3(-(lanesize + (buffersize * 2)) / 2, 0.5f, rownumber), 1.0f + (Random.Range(0.0f, 1.0f)));
    }
}
