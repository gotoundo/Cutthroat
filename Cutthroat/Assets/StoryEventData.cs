using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public enum PortraitID { Beagle, BostonTerrier, Bulldog, BullTerrier, Chihuahua, GermanShepherd, LabradorRetriever, PitBull, Pomeranian,Pug, Shihzu, SiberianHusky}
public class StoryEventData
{
    public PortraitID Portrait;
    public string EventTitle; // typically just the level of the map?
    public string Description;
    public string ButtonText;
    public string NPCName;
    public List<StoryEventData> Choices;
    //public bool CloseOnSelection;

    public StoryEventData()
    {
        Choices = new List<StoryEventData>();
    }

    public StoryEventData(string ButtonText) : this()
    {
        this.ButtonText = ButtonText;
    }

    public StoryEventData(string EventTitle, PortraitID Portrait, string Description, string NPCName, string ButtonText = "OK") : this()
    {
        this.EventTitle = EventTitle;
        this.Portrait = Portrait;
        this.Description = Description;
        this.ButtonText = ButtonText;
        this.NPCName = NPCName;
    }

}
