using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {

    public string LevelName;
    private bool tutorial = true;
    [SerializeField]
    Image tutorialScreen;
	
	void Update () {
        if (Input.anyKeyDown)
        {
            if(tutorial)
            {
                tutorialScreen.gameObject.SetActive(true);
                tutorial = false;
            }
            else
            {
                SceneManager.LoadScene(LevelName);
            }
        }
	}
}