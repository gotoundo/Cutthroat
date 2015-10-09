using UnityEngine;
using System.Collections;

public class CheatWinLoseUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void WinLevel()
    {
        if (GameManager.singleton != null)
            GameManager.singleton.autoWin = true;
    }
    public void LoseLevel()
    {
        if (GameManager.singleton != null)
            GameManager.singleton.autoLose = true;
    }
}
