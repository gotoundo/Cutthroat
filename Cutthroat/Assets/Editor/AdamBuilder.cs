using UnityEditor;
using UnityEngine;
using System.Collections;

public class AdamBuilder : MonoBehaviour {

    [MenuItem("Adam/MakeIOSBuild")]
    static void MakeIOSBuild()
    {
//#if UNITY_EDITOR_OSX
        string[] levels = { "Assets/IntroScene.unity", "Assets/MainScene.unity" };
        BuildPipeline.BuildPlayer(levels, "iOSBuild", BuildTarget.iOS, BuildOptions.AutoRunPlayer);
//#else
        //Debug.Log("Can only build iOS in OSX");
//#endif
    }

    [MenuItem("Adam/Make Windows Build")]
    static void MakeWindowsBuild()
    {
        //string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = { "Assets/IntroScene.unity", "Assets/MainScene.unity" };
        BuildPipeline.BuildPlayer(levels, "WindowsBuild/WB.exe", BuildTarget.StandaloneWindows, BuildOptions.AutoRunPlayer);
    }
}
