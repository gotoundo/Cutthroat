using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsListUI : MonoBehaviour {
    public GameObject[] cheatButtons;
	// Use this for initialization
	void Start () {
        foreach (GameObject cheat in cheatButtons)
            cheat.SetActive(SaveData.current.cheatsEnabled());
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
