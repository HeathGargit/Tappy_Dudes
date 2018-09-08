﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class HighScoreController : MonoBehaviour {
    
    // The base URL of the High Score server being used
    public string m_URL;
    // The Game ID for the current game within the High Scores system
    public int m_Game_ID;

    // The player's details
    private int m_Player_ID;
    private const string m_Player_ID_key = "Player_ID";

    private string m_Player_Email;
    private const string m_Player_Email_key = "Player_Email";

    private string m_Player_Username;
    private const string m_Player_Username_key = "Player_Username";

    private string m_Player_Password;
    private const string m_Player_Password_Key = "Player_Password";

    //The High Scores endpoint
    private const string m_HighScoresEndpoint = "/high_scores";
    //The User info endpoint

    //The Login endpoint
    private const string m_LoginEndpoint = "/login";

    public string Player_Username
    {
        get
        {
            return m_Player_Username;
        }
    }
    public string Player_Email
    {
        get
        {
            return m_Player_Email;
        }
    }
    public int Player_ID
    {
        get
        {
            return m_Player_ID;
        }
    }

    private void Awake()
    {
        //Singleton Control to make sure high scores available from all scenes and isn't reset, etc.
        int singletonCount = FindObjectsOfType<HighScoreController>().Length;
        if (singletonCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        //if there are stored player preferences, load them
        if(PlayerPrefs.HasKey(m_Player_ID_key) && PlayerPrefs.HasKey(m_Player_Password_Key))
        {
            m_Player_ID = PlayerPrefs.GetInt(m_Player_ID_key);
            m_Player_Password = PlayerPrefs.GetString(m_Player_Password_Key);
            m_Player_Username = PlayerPrefs.GetString(m_Player_Username_key);
            m_Player_Password = PlayerPrefs.GetString(m_Player_Password_Key);
        }
    }

    /// <summary>
    /// returns the top 5 scores for this game
    /// </summary>
    /// <returns>returns the top 5 scores for this game</returns>
    public string GetGlobalHighScores()
    {
        string resultsString = "";

        if (m_URL != null && m_Game_ID != 0)
        {
            //post to api to get high scores
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://{0}{1}?game_id={2}", m_URL, m_HighScoresEndpoint, m_Game_ID));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //process results
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            Debug.Log(jsonResponse);
            High_Scores hs_globalHighScores = JsonUtility.FromJson<High_Scores>(jsonResponse);
            //create return string
            foreach (Score score in hs_globalHighScores.scores)
            {
                resultsString += String.Format("{0} - {1}\n\n", score.display_name, score.score);
            }
        }
        else
        {
            return "Error: High Score Parameters Not Set Correctly";
        }

        //return result
        return resultsString;
    }

    public int LogPlayerIn(string email, string password)
    {
        //convert the password into it's hashed form
        string hashedPassword = getMD5Value(password);
        
        //call the API to log in
        //post to api to get high scores
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://{0}{1}?email={2}&password={3}", m_URL, m_LoginEndpoint, email, hashedPassword));
        request.Method = "POST";
        Debug.Log(String.Format("http://{0}{1}?email={2}&password={3}", m_URL, m_LoginEndpoint, email, hashedPassword));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //process results
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(jsonResponse);

        //if login successful
        if (loginResponse.login_success == 1)
        {
            //then set the player info
            m_Player_ID = loginResponse.player_id;
            m_Player_Username = loginResponse.username;
            m_Player_Email = loginResponse.email;
            m_Player_Password = hashedPassword;

            //if email/pass it toggled to be stored
            if(true)
            {
                //then save stuff in player prefs
                PlayerPrefs.SetInt(m_Player_ID_key, loginResponse.player_id);
                PlayerPrefs.SetString(m_Player_Email_key, loginResponse.email);
                PlayerPrefs.SetString(m_Player_Username_key, loginResponse.username);
                PlayerPrefs.SetString(m_Player_Password_Key, hashedPassword);
            }

            //return successful
            Debug.Log(loginResponse.login_message);
            return 1;
        }

        //else return 0
        Debug.Log(loginResponse.login_message);
        return 0;
    }

    public void LogPlayerOut()
    {
        PlayerPrefs.DeleteKey(m_Player_Email_key);
        PlayerPrefs.DeleteKey(m_Player_Username_key);
        PlayerPrefs.DeleteKey(m_Player_ID_key);
        PlayerPrefs.DeleteKey(m_Player_Password_Key);
    }

    public string getMD5Value(string value)
    {
        if (value != null)
        {
            //Initialist Stuff we'll need
            ASCIIEncoding encoderer = new ASCIIEncoding();
            MD5 md5 = new MD5CryptoServiceProvider();

            //convert to a hashed value
            byte[] hashedString = md5.ComputeHash(encoderer.GetBytes(value));

            //turn the hashed string back into human readable hex
            string returnString = "";
            for (int i = 0; i < hashedString.Length; i++)
            {
                returnString += hashedString[i].ToString("x2"); //X2 is the lower cased hex format "X2" would be upper case
            }

            return returnString;
        }

        //else return null
        return null;
    }
}

[Serializable]
public class Score
{
    public string display_name;
    public int score;
}

[Serializable]
public class High_Scores
{
    public string game_id;
    public List<Score> scores;
}

[Serializable]
public class LoginResponse
{
    public int login_success;
    public string login_message;
    public string email;
    public string username;
    public int player_id;
}
