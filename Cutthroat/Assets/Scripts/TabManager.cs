using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class TabManager : MonoBehaviour {

    public GameObject[] Panels;
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
        Background.transform.SetAsLastSibling();
        foreach (GameObject o in Panels)
        {
            o.SetActive(o.Equals(chosenPanel));
            if (o.Equals(chosenPanel))
                o.transform.parent.SetAsLastSibling();
        }
    }
}
