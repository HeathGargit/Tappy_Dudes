/*---------------------------------------------------------
File Name: GPGSButtonController.cs
Purpose: Script to control showing the leaderboard loginout/show buttons.
Author: Heath Parkes (heath@gargit.games)
Modified: 2019-07-07
-----------------------------------------------------------
Copyright 2019 HP
---------------------------------------------------------*/

using GooglePlayGames;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GPGSButtonController : MonoBehaviour
{
    public GameObject m_LeaderboardButtonGameObject;

    // ref to the leaderboard controller
    private LeaderBoardController m_LeaderBoardController;
    private Button m_LoginComboButton;
    private Button m_LeaderboardButton;
    private Text m_LoginButtonText;
    private Text m_LeaderboardButtonText;
    

    //Button to add the login function to.

    // Start is called before the first frame update
    void Start()
    {
        //get references
        m_LeaderBoardController = GameObject.FindGameObjectWithTag("GPGS").GetComponent<LeaderBoardController>();
        m_LoginComboButton = GetComponent<Button>();
        m_LoginButtonText = GetComponentInChildren<Text>();
        m_LeaderboardButton = m_LeaderboardButtonGameObject.GetComponent<Button>();
        m_LeaderboardButtonText = m_LeaderboardButtonGameObject.GetComponentInChildren<Text>();

        //add onclick for this button to sign-in or out of GPGS
        m_LoginComboButton.onClick.AddListener(delegate { m_LeaderBoardController.SignInOut(); });
    }

    // Update is called once per frame
    void Update()
    {
        //if not logged into GPGS
        if(!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // set the text to ask to log in
            m_LoginButtonText.text = "Sign Into Google Play";
            // make sure the leaderboard button is hidden
            m_LeaderboardButtonGameObject.gameObject.SetActive(false);
        }
        else
        {
            //set the text of the combo button to log out
            m_LoginButtonText.text = "Sign Out Of Google Play";
            //show the leaderboard button
            m_LeaderboardButtonGameObject.gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        //get references
        m_LeaderBoardController = GameObject.FindGameObjectWithTag("GPGS").GetComponent<LeaderBoardController>();
        m_LoginComboButton = GetComponent<Button>();
        m_LoginButtonText = GetComponentInChildren<Text>();
        m_LeaderboardButton = m_LeaderboardButtonGameObject.GetComponent<Button>();
        m_LeaderboardButtonText = m_LeaderboardButtonGameObject.GetComponentInChildren<Text>();

        //add onclick for this button to sign-in or out of GPGS
        m_LoginComboButton.onClick.AddListener(delegate { m_LeaderBoardController.SignInOut(); });
    }
}
