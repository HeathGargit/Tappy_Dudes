using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RC_RockRowCreator : RowCreator {

    public override void Init(int buffersize, int lanesize, int rownumber)
    {
        //call the base method
        base.Init(buffersize, lanesize, rownumber);

        //set up this classes unique stuff
        GetComponent<RockSpawner>().Init(lanesize, buffersize, rownumber);
    }
}
