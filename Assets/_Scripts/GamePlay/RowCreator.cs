using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowCreator : MonoBehaviour {

    public GameObject m_StandardBlock;
    public GameObject m_BufferBlock;

    private List<NodeInfo> m_RowNodes;

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

    protected void CreateRow(int buffersize, int lanesize, int RowYPos)
    {
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


    public virtual void Init(int buffersize, int lanesize, int rownumber)
    {
        //m_RowNodes = new List<NodeInfo>();
        CreateRow(buffersize, lanesize, rownumber);
    }

    public void RemoveMovementNodeAt(int nodeXPos)
    {
        foreach(NodeInfo node in m_RowNodes)
        {
            if(node.GridLocation.x == nodeXPos)
            {
                m_RowNodes.Remove(node);
                break;
            }
        }
    }
}
