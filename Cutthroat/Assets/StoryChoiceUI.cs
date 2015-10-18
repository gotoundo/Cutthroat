using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StoryChoiceUI : MonoBehaviour
{
    public Text ButtonText;
    public StoryEventData StoryResult;

    public void RunStoryResult()
    {
        List<StoryEventData> temp = new List<StoryEventData>();
        temp.Add(StoryResult);
        GameManager.Main.RunTriggeredEvents(temp);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}