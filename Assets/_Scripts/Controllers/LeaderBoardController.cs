/*---------------------------------------------------------
File Name: LeaderBoardController.cs
Purpose: This controls leaderboard calls and submits and stuff
Author: Heath Parkes (heath@gargit.games)
Modified: 2019-07-07
-----------------------------------------------------------
Copyright 2019 HP
---------------------------------------------------------*/
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class LeaderBoardController : MonoBehaviour
{
    //singleton instance variable
    private static LeaderBoardController _instance;

    public static LeaderBoardController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LeaderBoardController>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //create client configuration
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.DebugLogEnabled = true;

        // init and activate the platform
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        //try to sign in
        PlayGamesPlatform.Instance.Authenticate(SignInCallback, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SignInOut()
    {
        //if the user is not already signed in and authenticated:
        if(!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            //sign them in
            PlayGamesPlatform.Instance.Authenticate(SignInCallback, false);
        }
        else
        {
            //sign them out
            PlayGamesPlatform.Instance.SignOut();

        }
    }

    public void SignInCallback(bool success)
    {
        if(success)
        {
            Debug.Log("(Tappy Dudes) Signed In Successfully");
        }
        else
        {
            Debug.Log("(Tappy Dudes) Sign in FAILEDT!");
        }
    }
}
