/*---------------------------------------------------------
File Name: NodeInfo.cs
Purpose: Stores information for movement nodes in the game for the player to move to.
Author: Heath Parkes (gargit@gargit.net)
Modified: 2018-06-03
-----------------------------------------------------------
Copyright 2018 AIE/HP
---------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInfo : MonoBehaviour {

    [SerializeField]
    private Vector2Int m_GridLocation;

    /// <summary>
    /// holds the location in the board.
    /// </summary>
    public Vector2Int GridLocation
    {
        get
        {
            return m_GridLocation;
        }

        set
        {
            m_GridLocation = value;
        }
    }
}
