using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;
using System.Collections;

public class AdamBuilder : MonoBehaviour {

    [MenuItem("Adam/MakeIOSBuild")]
    static void MakeIOSBuild()
    {
        string[] levels = { "Assets/IntroScene.unity", "Assets/MainScene.unity" };
        BuildPipeline.BuildPlayer(levels, "iOSBuild", BuildTarget.iOS, BuildOptions.AutoRunPlayer);
    }

    static int screenshotNum = 0;
    [MenuItem("Adam/Screenshot")]
    static void Screenshot()
    {
        Application.CaptureScreenshot("Screenshot"+ screenshotNum+".png");
        screenshotNum++;
    }

    /*[PostProcessBuild]
    static void IncrementVersion(UnityEditor.BuildTarget target, string whoknows)
    {
        PlayerSettings.bundleVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }*/
}
