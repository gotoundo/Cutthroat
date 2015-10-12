using UnityEngine;
using System.Collections;

public class MainMenuButtonUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void MainMenu()
    {
        Application.LoadLevel("IntroScene");
    }
}
