/*---------------------------------------------------------
File Name: RC_RockRowCreator.cs
Purpose: Derived from the RowCreator class, created a row with a few rocks in it.
Author: Heath Parkes (gargit@gargit.net)
Modified: 2018-06-03
-----------------------------------------------------------
Copyright 2018 AIE/HP
---------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RC_RockRowCreator : RowCreator {

    /// <summary>
    /// sets up initial variables
    /// </summary>
    /// <param name="buffersize">size of the non-playable gutter/buffer area each side of the playable area</param>
    /// <param name="lanesize">size of the playable area</param>
    /// <param name="rownumber">number of the row this creator is creating</param>
    public override void Init(int buffersize, int lanesize, int rownumber)
    {
        //call the base method
        base.Init(buffersize, lanesize, rownumber);

        //set up this classes unique stuff
        GetComponent<RockSpawner>().Init(lanesize, buffersize, rownumber);
    }
}
