using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    public string LevelName;    
	
	void Update () {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(LevelName);
        }
	}
}