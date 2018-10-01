using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangeButtonController : MonoBehaviour {

    private LevelManager m_LevelManager;

	// Use this for initialization
	void Start ()
    {
        m_LevelManager = FindObjectOfType<LevelManager>();	
	}

    public void LoadLevel(string levelname)
    {
        m_LevelManager.LoadLevel(levelname);
    }
}
