using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroPanelUI : MonoBehaviour {
    public Text TitleText;
    public Text ObjectiveText;

	// Use this for initialization
	void Start () {
        TitleText.text = GameManager.Main.CurrentLevel.Title;
        ObjectiveText.text = GameManager.Main.CurrentLevel.MainObjectiveDescription;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
