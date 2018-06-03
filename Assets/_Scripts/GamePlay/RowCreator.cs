/*---------------------------------------------------------
File Name: RowCreator.cs
Purpose: Creates a basic row for the board.
Author: Heath Parkes (gargit@gargit.net)
Modified: 2018-06-03
-----------------------------------------------------------
Copyright 2018 AIE/HP
---------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowCreator : MonoBehaviour {

    //set blocks to use in the inspector
    public GameObject m_StandardBlock;
    public GameObject m_BufferBlock;

    //a list of the move-to-able nodes in the row
    private List<NodeInfo> m_RowNodes;

    //accessor
    public List<NodeInfo> RowNodes
    {
        get
        {
            return m_RowNodes;
        }
    }

    private void Awake()
    {
        m_RowNodes = new List<NodeInfo>();
    }

    /// <summary>
    /// Creates the row.
    /// </summary>
    /// <param name="buffersize">Size of non-playable area each side of lane</param>
    /// <param name="lanesize">width of the lane</param>
    /// <param name="RowYPos">Row number of this instance of the row</param>
    protected void CreateRow(int buffersize, int lanesize, int RowYPos)
    {
        //set where to start creating blocks
        int currentRowXPos = -((lanesize + (buffersize * 2)) / 2);

        //Add left buffer blocks
        for (int i = 0; i <= buffersize; i++)
        {
            GameObject newBlock = Instantiate(m_BufferBlock, new Vector3(currentRowXPos, 0, RowYPos), Quaternion.identity);
            newBlock.transform.SetParent(transform);

            currentRowXPos++;

        }
        //add lane blocks
        for (int i = 0; i <= lanesize; i++)
        {
            //create new lane block and add iot to the row
            GameObject newBlock = Instantiate(m_StandardBlock, new Vector3(currentRowXPos, 0, RowYPos), Quaternion.identity);
            newBlock.transform.SetParent(transform);

            //if the new block has a movement node
            if(newBlock.GetComponentInChildren<NodeInfo>())
            {
                //set the movement node properties
                newBlock.GetComponentInChildren<NodeInfo>().GridLocation = new Vector2Int(currentRowXPos, RowYPos);

                //add the movement node to the row's movement nodes.
                m_RowNodes.Add(newBlock.GetComponentInChildren<NodeInfo>());
            }

            //increment the current x pos in the row
            currentRowXPos++;
        }
        //add right buffer blocks
        for (int i = 0; i <= buffersize; i++)
        {
            GameObject newBlock = Instantiate(m_BufferBlock, new Vector3(currentRowXPos, 0, RowYPos), Quaternion.identity);
            newBlock.transform.SetParent(transform);

            currentRowXPos++;
        }
    }

    /// <summary>
    /// initialises/creates the row
    /// </summary>
    /// <param name="buffersize"></param>
    /// <param name="lanesize"></param>
    /// <param name="rownumber"></param>
    public virtual void Init(int buffersize, int lanesize, int rownumber)
    {
        //m_RowNodes = new List<NodeInfo>();
        CreateRow(buffersize, lanesize, rownumber);
    }

    /// <summary>
    /// Removes a movement node to mark that node as not-move-to-able
    /// </summary>
    /// <param name="nodeXPos"></param>
    public void RemoveMovementNodeAt(int nodeXPos)
    {
        //check each row
        foreach(NodeInfo node in m_RowNodes)
        {
            //if it's the node we cant move to
            if(node.GridLocation.x == nodeXPos)
            {
                //remove the row and return
                m_RowNodes.Remove(node);
                break;
            }
        }
    }
}
