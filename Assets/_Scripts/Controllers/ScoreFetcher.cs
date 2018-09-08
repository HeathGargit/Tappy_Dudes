using UnityEngine;
using UnityEngine.UI;

public class ScoreFetcher : MonoBehaviour {

    public Text m_ScoreText;
    HighScoreController m_HighScoreController;

    private void Awake()
    {
        m_HighScoreController = GameObject.FindGameObjectWithTag("HighScoreSystem").GetComponent<HighScoreController>();
        m_ScoreText.text = "Fetching Scores...";
    }

    private void OnEnable()
    {
        m_ScoreText.text = m_HighScoreController.GetGlobalHighScores();
    }
}
