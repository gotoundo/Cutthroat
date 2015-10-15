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
        if (GameManager.Main != null)
            GameManager.Main.autoWin = true;
    }
    public void LoseLevel()
    {
        if (GameManager.Main != null)
            GameManager.Main.autoLose = true;
    }

    public static void ResetSaveData()
    {
        SaveData.current = new SaveData();
        SaveData.current.UnlockedLevels.Add(LevelID.L1);
        SaveTool.Save();
    }

    public void EraseData()
    {
        ResetSaveData();
       // Application.LoadLevel("IntroScene");
    }
}
