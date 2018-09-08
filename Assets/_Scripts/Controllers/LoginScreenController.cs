using UnityEngine.UI;
using UnityEngine;

public class LoginScreenController : MonoBehaviour {

    public Text m_ResponseText;
    //public Text m_email, m_password;
    public InputField m_email, m_password;
    HighScoreController m_HighScoreController;

    private void Awake()
    {
        m_HighScoreController = GameObject.FindGameObjectWithTag("HighScoreSystem").GetComponent<HighScoreController>();
        m_ResponseText.text = "Not Logged In.........";
        if (m_HighScoreController.Player_Email != null)
        {
            m_ResponseText.text = "Logged in as " + m_HighScoreController.Player_Username;
        }
        else
        {
            m_ResponseText.text = "Not Logged In...";
        }
    }

    private void OnEnable()
    {
        if(m_HighScoreController.Player_Email != null)
        {
            m_ResponseText.text = "Logged in as " + m_HighScoreController.Player_Username;
        }
        else
        {
            m_ResponseText.text = "Not Logged In";
        }
    }

    public void LogUserIn()
    {
        if(m_email.text.Length > 0 && m_password.text.Length > 0)
        {
            Debug.Log(m_password.text);
            int response = m_HighScoreController.LogPlayerIn(m_email.text, m_password.text);
            if (response == 1)
            {
                m_ResponseText.text = "Logged in as " + m_HighScoreController.Player_Username;
            }
            else
            {
                m_ResponseText.text = "There was an error logging in!";
            }
        }
        else
        {
            m_ResponseText.text = "email or password empty";
        }
    }

    public void LogUserOut()
    {
        m_HighScoreController.LogPlayerOut();
    }
}
