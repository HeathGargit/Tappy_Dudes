using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScreenController : MonoBehaviour {

    public GameObject m_ScoreScreen;

    private void Start()
    {
        m_ScoreScreen.SetActive(false);
    }

    public void SetScoreScreenActive()
    {
        m_ScoreScreen.SetActive(true);
    }

    public void SetScoreScreenInActive()
    {
        m_ScoreScreen.SetActive(false);
    }
}
