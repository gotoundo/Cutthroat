using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.Main.Source.clip = AudioManager.Main.Music[1];
        AudioManager.Main.Source.Play();

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
