using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelLoadButton : MonoBehaviour {

    public LevelID MyLevel;
    Button button;

	// Use this for initialization
	void Start () {
        GetComponentInChildren<Text>().text = LevelManager.LevelDefinitions[MyLevel].Title;
        button = GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
        button.interactable = SaveData.current.UnlockedLevels.Contains(MyLevel);
	}

    public void LoadLevel()
    {
        LevelDefinition levelDef = LevelManager.LevelDefinitions[MyLevel];
        LevelManager.SelectedLevel = levelDef;
        Application.LoadLevel(levelDef.Scene);
    }

    public void MainMenu()
    {
        Application.LoadLevel("IntroScene");
    }
}
