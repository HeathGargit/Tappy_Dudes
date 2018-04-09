using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    //reference to the board to build
    public GameObject m_Board;
    private BoardController m_BoardController;

    //game board settings
    public int m_BoardSize;
    public int m_LaneBufferSize;
    public int m_LaneSize;

    //player settings
    public GameObject m_PlayerPrefab;
    private GameObject m_Player;
    public NodeInfo m_CurrentNode;
    public CameraFollow m_CameraController;

    //score tracking
    private int m_PlayerScore;
    public Text m_ScoreLabel;

    //game state
    enum GameStates { Initialisation, Playing, GameOver};
    private GameStates m_GameState;


	// Use this for initialization
	void Start ()
    {
        m_BoardController = m_Board.GetComponent<BoardController>();
        m_GameState = GameStates.Initialisation;
        m_CameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        
        m_CameraController.SetOffset(gameObject);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    switch(m_GameState)
        {
            case GameStates.Initialisation:
                InitGame();
                m_GameState = GameStates.Playing;
                break;
            case GameStates.Playing:
                ProcessInput();
                if(!m_Player.GetComponent<DudeController>().IsPlayerAlive)
                {
                    m_GameState = GameStates.GameOver;
                }
                break;
            case GameStates.GameOver:
                //display game over panel
                
                if(Input.GetKeyDown(KeyCode.R))
                {
                    m_GameState = GameStates.Initialisation;
                }
                break;
            default:
                break;
        }
	}

    //process the frame's input
    private void ProcessInput()
    {
        //variable to store the next node being moved to
        NodeInfo nextNode = null;

        //get input
        if (Input.GetButtonDown("Horizontal"))
        {
            if(Input.GetAxis("Horizontal") < 0)
            {
                nextNode = m_BoardController.FindNode(m_CurrentNode.GridLocation + new Vector2Int(-1, 0));
            }
            else if (Input.GetAxis("Horizontal") > 0f)
            {
                nextNode = m_BoardController.FindNode(m_CurrentNode.GridLocation + new Vector2Int(1, 0));
            }
        }
        if(Input.GetButtonDown("Vertical"))
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                nextNode = m_BoardController.FindNode(m_CurrentNode.GridLocation + new Vector2Int(0, -1));
            }
            else if (Input.GetAxis("Vertical") > 0f)
            {
                nextNode = m_BoardController.FindNode(m_CurrentNode.GridLocation + new Vector2Int(0, 1));
            }
        }

        //if there is a node to move to
        if(nextNode != null)
        {
            //if the player is not already jumping
            if(!m_Player.GetComponent<DudeController>().IsJumping)
            {
                //jump from this node to next node
                m_Player.GetComponent<DudeController>().MoveTo(m_CurrentNode, nextNode);
                //set the next node to the current node
                m_CurrentNode = nextNode;

                //check if we are increasing the score and/or cycling rows
                if(nextNode.GridLocation.y > m_PlayerScore)
                {
                    //add/remove rows
                    for(int i = nextNode.GridLocation.y - m_PlayerScore; i > 0; i--)
                    {
                        m_BoardController.CycleRows();
                    }
                    
                    //increase the score
                    m_PlayerScore = nextNode.GridLocation.y;
                }
            }
        }

        m_ScoreLabel.text = m_PlayerScore.ToString();
    }

    void InitGame()
    {
        m_BoardController.Init(m_LaneBufferSize, m_LaneSize, m_BoardSize);
        GameObject StartingPos = m_BoardController.FindNode(new Vector2Int(2, 0)).gameObject;
        m_Player = Instantiate(m_PlayerPrefab, StartingPos.transform.position, Quaternion.identity);
        m_CameraController.SetTarget(m_Player);
        m_PlayerScore = StartingPos.GetComponent<NodeInfo>().GridLocation.y;

        m_CurrentNode = StartingPos.GetComponentInChildren<NodeInfo>();
    }
}
