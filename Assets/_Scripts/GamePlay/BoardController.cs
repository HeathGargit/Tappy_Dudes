/*---------------------------------------------------------
File Name: Board Controller.cs
Purpose: Controls the board, including initial setup and handling new/old rows.
Author: Heath Parkes (gargit@gargit.net)
Modified: 2018-06-03
-----------------------------------------------------------
Copyright 2018 AIE/HP
---------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour {

    //holds the standard row prefab
    public GameObject m_StandardRow;

    //holds the gameplay row prefabs
    public List<GameObject> m_GamePlayRows;

    //board settings
    private int m_LaneBufferSize;
    private int m_LaneSize;

    //this keeps track of the next row to be created.
    private int m_LastRowCreated;

    //list of rows to add to and remove from
    private List<GameObject> m_Rows;

    //grid of movement nodes in the board
    private List<NodeInfo> m_BoardMovementNodes;

    private void Start()
    {
        m_LastRowCreated = -6;
        m_BoardMovementNodes = new List<NodeInfo>();
        m_Rows = new List<GameObject>();
    }

    /// <summary>
    /// Adds a row that is set as the "basic" row to spawn. Generally used when initially seeting up the game board only.
    /// </summary>
    private void AddBasicRow()
    {
        //create the row object
        GameObject thisRow = Instantiate(m_StandardRow, new Vector3(0, 0, 0), Quaternion.identity);

        //initialise the row
        thisRow.GetComponent<RowCreator>().Init(m_LaneBufferSize, m_LaneSize, m_LastRowCreated);

        //set the board as the parent object
        thisRow.transform.SetParent(transform);

        //add movement nodes in the row to the grid.
        foreach (NodeInfo node in thisRow.GetComponent<RowCreator>().RowNodes)
        {
            m_BoardMovementNodes.Add(node);
        }

        //add to the list of rows
        m_Rows.Add(thisRow);

        //increase the total number of rows we've created.
        m_LastRowCreated++;
    }

    /// <summary>
    /// Adds a row to the board from the List of rows that can be used.
    /// </summary>
    private void AddGameplayRow()
    {
        int rand = Random.Range(0,(m_GamePlayRows.Count));
        //create the row object
        GameObject thisRow = Instantiate(m_GamePlayRows[rand], new Vector3(0, 0, 0), Quaternion.identity);

        //initialise the row
        thisRow.GetComponent<RowCreator>().Init(m_LaneBufferSize, m_LaneSize, m_LastRowCreated);

        //set the board as the parent object
        thisRow.transform.SetParent(transform);

        //add movement nodes in the row to the grid.
        foreach (NodeInfo node in thisRow.GetComponent<RowCreator>().RowNodes)
        {
            m_BoardMovementNodes.Add(node);
        }

        //add to the list of rows
        m_Rows.Add(thisRow);

        //increase the total number of rows we've created.
        m_LastRowCreated++;
    }


    /// <summary>
    /// Initialises the Board Controller.
    /// </summary>
    /// <param name="BufferSize">Size of the side "gutter" areas that cant be accessed by the player</param>
    /// <param name="LaneSize">Size across (X Axis) if the playable area of the row</param>
    /// <param name="LanesToCreate">Amount of intiial lanes to create when game begins.</param>
    public void Init(int BufferSize, int LaneSize, int LanesToCreate)
    {
        //set the board dimensions from the passed game controller variables.
        m_LaneBufferSize = BufferSize;
        m_LaneSize = LaneSize;

        //create initial lanes
        for (int i = 0; i < 9; i++)
        {
            AddBasicRow();
        }

        //create lanes beyond the "starting" area
        for (int i = 0; i < LanesToCreate - 9; i++)
        {
            AddGameplayRow();
        }
    }

    /// <summary>
    /// find the passed node in the game board
    /// </summary>
    /// <param name="nodeToFind">similar node to find in the game board</param>
    /// <returns>The node if it is in the gameboard, or else null</returns>
    public NodeInfo FindNode(Vector2Int nodeToFind)
    {
        //look through the nodes
        foreach(NodeInfo node in m_BoardMovementNodes)
        {
            //if we find the node
            if(node.GridLocation == nodeToFind)
            {
                //return the node
                return node;
            }
        }
        //else return null
        return null;
    }

    /// <summary>
    /// adds new rows and removes old ones
    /// </summary>
    public void CycleRows()
    {
        //adds a new row
        AddGameplayRow();

        //destroys the oldest row gameobject then updates the list.
        Destroy(m_Rows[0]);
        m_Rows.RemoveAt(0);
    }

    /// <summary>
    /// Resets the game board to it's initalised state.
    /// </summary>
    public void ResetBoard()
    {
        //destroy what's in there
        foreach(GameObject row in m_Rows)
        {
            Destroy(row);
        }

        //reset the initial variables.
        m_LastRowCreated = -6;
        m_BoardMovementNodes = new List<NodeInfo>();
        m_Rows = new List<GameObject>();
    }
}
