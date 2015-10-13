using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class TabManager : MonoBehaviour {

    public GameObject[] Panels;
    public Button[] TabButtons;

    public GameObject Background;

	// Use this for initialization
	void Start () {
        Switch(Panels[0]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Switch(GameObject chosenPanel)
    {
        for (int i = 0; i< Panels.Length; i++)
        {
            if(Panels[i].Equals(chosenPanel))
            {
                Panels[i].SetActive(true);
                TabButtons[i].interactable = false;
            }
            else
            {
                Panels[i].SetActive(false);
                TabButtons[i].interactable = true;
            }
            
        }
    }
}
