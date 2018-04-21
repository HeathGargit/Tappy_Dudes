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

    private void AddGameplayRow()
    {
        int rand = Random.Range(0,(m_GamePlayRows.Count));
        //create the row object
        GameObject thisRow = Instantiate(m_GamePlayRows[rand], new Vector3(0, 0, 0), Quaternion.identity);

        //initialise the row
        thisRow.GetComponent<RowCreator>().Init(m_LaneBufferSize, m_LaneSize, m_LastRowCreated);

        //if the row has enemies, 
        if(thisRow.gameObject.GetComponent<EnemySpawner>() != null)
        {
            thisRow.gameObject.GetComponent<EnemySpawner>().Init(new Vector3(-(m_LaneSize + (m_LaneBufferSize * 2)) / 2, 0.5f, m_LastRowCreated), 1.0f + (Random.Range(0.0f,1.0f)));
        }

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

        for (int i = 0; i < LanesToCreate - 9; i++)
        {
            AddGameplayRow();
        }
    }

    //returns if there is a node at the given position or not
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

    //adds new rows and removes old ones.
    public void CycleRows()
    {
        //adds a new row
        AddGameplayRow();

        //destroys the oldest row gameobject then updates the list.
        Destroy(m_Rows[0]);
        m_Rows.RemoveAt(0);
    }

    public void ResetBoard()
    {
        foreach(GameObject row in m_Rows)
        {
            Destroy(row);
        }

        m_LastRowCreated = -6;
        m_BoardMovementNodes = new List<NodeInfo>();
        m_Rows = new List<GameObject>();
    }
}
