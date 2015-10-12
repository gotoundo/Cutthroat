using UnityEngine;
using System.Collections;

public class SaveLoadUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Save()
    {
        SaveTool.Save();
    }

    public void Load()
    {
        SaveTool.Load();
    }
}
