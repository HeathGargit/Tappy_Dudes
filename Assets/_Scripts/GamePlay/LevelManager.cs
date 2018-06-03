/*---------------------------------------------------------
File Name: LevelManager.cs
Purpose: Cheap nasty script to change the level. for buttons in the UI to use.
Author: Heath Parkes (gargit@gargit.net)
Modified: 2018-06-03
-----------------------------------------------------------
Copyright 2018 AIE/HP
---------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    /// <summary>
    /// load the specified level.
    /// </summary>
    /// <param name="LevelName"></param>
	public void LoadLevel(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }
}
