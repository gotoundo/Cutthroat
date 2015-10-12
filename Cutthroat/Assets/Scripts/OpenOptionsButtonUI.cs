using UnityEngine;
using System.Collections;

public class OpenOptionsButtonUI : MonoBehaviour {
    public GameObject OptionsPanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void ToggleOptions()
    {
        OptionsPanel.SetActive(!OptionsPanel.activeSelf);
    }
}
