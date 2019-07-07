/*---------------------------------------------------------
File Name: Game Controller.cs
Purpose: Controls the game, including gameplay states and player input.
Author: Heath Parkes (gargit@gargit.net)
Modified: 2018-09-28
-----------------------------------------------------------
Copyright 2018 AIE/HP
---------------------------------------------------------*/

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
    private float m_InactiveTime;
    private int m_FurthestScoreProgress;
    //private HighScoreController m_HighScoreController;


    //game state stuff
    enum GameStates { Initialisation, Playing, GameOver};
    private GameStates m_GameState;
    public GameObject m_Menu;
    public Text m_GameOverText;

    //Swipe control stuff.
    public SwipeController m_SwipeController;

    //Audio stuff
    public AudioSource m_JumpSound;
    public AudioSource m_DeathSound;

	// Use this for initialization
	void Start ()
    {
        //these are in here because the unity player has issues when resolution is set in the inspector and changed after a build has been created.
        //PlayerPrefs.DeleteAll();
        //Application.Quit();

        //set up gameobject references
        m_BoardController = m_Board.GetComponent<BoardController>();
        m_CameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        //m_HighScoreController = GameObject.FindGameObjectWithTag("HighScoreSystem").GetComponent<HighScoreController>();

        //set game state
        m_GameState = GameStates.Initialisation;
        
        //set up other variables
        m_CameraController.SetOffset(gameObject);
        m_Menu.SetActive(false);
        m_GameOverText.text = "";
    }

    // Update is called once per frame
    void Update ()
    {
        //check what state the game is currently in
	    switch(m_GameState)
        {
            //initialising
            case GameStates.Initialisation:
                InitGame();
                m_GameState = GameStates.Playing;
                break;
            //Playing
            case GameStates.Playing:
                //process the player's input.
                ProcessInput();
                //check if the player is dead
                if(!m_Player.GetComponent<DudeController>().IsPlayerAlive)
                {
                    //set the player as dead
                    m_Player.GetComponent<DudeController>().DudeDeath();
                    //play the death sound
                    m_DeathSound.Play();
                    //set the gamestate to game over
                    m_GameState = GameStates.GameOver;
                    //send the score to the high score system and set the game over text on the game over menu
                    SubmitHighScore(m_PlayerScore);
                }
                break;
            //game over
            case GameStates.GameOver:
                //display game over panel
                m_Menu.SetActive(true);
                break;
            default:
                break;
        }

        /* Game Losing Condition Checks*/
        //check if they have moved too far backwards and kill player if so
        if(m_CurrentNode.GridLocation.y < (m_PlayerScore - 2))
        {
            m_Player.GetComponent<DudeController>().IsPlayerAlive = false;
        }

        m_InactiveTime -= Time.deltaTime;

        //check if they havent moved to a new row forward for long enough.
        if(m_PlayerScore > m_FurthestScoreProgress)
        {
            m_FurthestScoreProgress = m_PlayerScore;
            m_InactiveTime = 5.5f;
        }
        else
        {
            if(m_InactiveTime < 0.0f)
            {
                m_Player.GetComponent<DudeController>().IsPlayerAlive = false;
            }
        }
	}

    private void SubmitHighScore(int m_PlayerScore)
    {
        m_GameOverText.text = "Your Score: " + m_PlayerScore + "\n";
        /*if(m_HighScoreController.isPlayerLoggedIn())
        {
            int submitSuccess = m_HighScoreController.SubmitScore(m_PlayerScore);
            if(submitSuccess == 1)
            {
                m_GameOverText.text += "Your Score was submitted to the high score system. Click \"High Scores\" to see this game's high scores!";
            }
            else
            {
                m_GameOverText.text += "There was an issue submitting your high score.";
            }
        }*/
    }

    /// <summary>
    /// processes the player's input from all controllers
    /// </summary>
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

        //Swipe Input
        m_SwipeController.ProcessTouchInput();
        if (m_SwipeController.SwipeLeft)
        {
            nextNode = m_BoardController.FindNode(m_CurrentNode.GridLocation + new Vector2Int(-1, 0));
        }
        if (m_SwipeController.SwipeRight)
        {
            nextNode = m_BoardController.FindNode(m_CurrentNode.GridLocation + new Vector2Int(1, 0));
        }
        if (m_SwipeController.SwipeDown)
        {
            nextNode = m_BoardController.FindNode(m_CurrentNode.GridLocation + new Vector2Int(0, -1));
        }
        if (m_SwipeController.SwipeUp)
        {
            nextNode = m_BoardController.FindNode(m_CurrentNode.GridLocation + new Vector2Int(0, 1));
        }

        //if there is a node to move to
        if (nextNode != null)
        {
            //if the player is not already jumping
            if(!m_Player.GetComponent<DudeController>().IsJumping)
            {
                //jump from this node to next node
                m_Player.GetComponent<DudeController>().MoveTo(m_CurrentNode, nextNode);
                //set the next node to the current node
                m_CurrentNode = nextNode;

                //play the jump sound
                m_JumpSound.Play();

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

        //update the player's score in the UI
        m_ScoreLabel.text = m_PlayerScore.ToString();
    }

    /// <summary>
    /// Initialise the game parameters.
    /// </summary>
    void InitGame()
    {
        //initialise the board
        m_BoardController.Init(m_LaneBufferSize, m_LaneSize, m_BoardSize);

        //set up the player
        GameObject StartingPos = m_BoardController.FindNode(new Vector2Int(2, 0)).gameObject;
        m_Player = Instantiate(m_PlayerPrefab, StartingPos.transform.position, Quaternion.identity);
        m_CameraController.SetTarget(m_Player);
        m_PlayerScore = StartingPos.GetComponent<NodeInfo>().GridLocation.y;
        m_CurrentNode = StartingPos.GetComponentInChildren<NodeInfo>();
        m_InactiveTime = 5.5f;
        m_FurthestScoreProgress = 0;

        //disable the menu
        m_Menu.SetActive(false);
        m_GameOverText.text = "";
    }

    /// <summary>
    /// reset the game back to it's initial state
    /// </summary>
    public void ResetGame()
    {
        //reset the game components
        m_BoardController.ResetBoard();
        Destroy(m_Player);
        m_PlayerScore = 0;

        //set the game back to the start to be initialised.
        m_GameState = GameStates.Initialisation;
    }
}
