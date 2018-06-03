using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RC_EnemyRowCreator : RowCreator {

    public override void Init(int buffersize, int lanesize, int rownumber)
    {
        //call the base method
        base.Init(buffersize, lanesize, rownumber);

        //set up this classes unique stuff
        GetComponent<EnemySpawner>().Init(new Vector3(-(lanesize + (buffersize * 2)) / 2, 0.5f, rownumber), 1.0f + (Random.Range(0.0f, 1.0f)));
    }
}
