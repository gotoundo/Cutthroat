﻿using UnityEngine;
using System.Collections;

public class LevelLoadButton : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadLevel(string levelID)
    {
        Application.LoadLevel(levelID);
    }
}