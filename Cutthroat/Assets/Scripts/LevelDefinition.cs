using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LevelDefinition
{
    public LevelID myID = LevelID.None;
    public LevelID WinUnlock = LevelID.None;
    public string Title = "default title";
    public string MainObjectiveDescription = "default objective description";

    public string Scene = "MainScene";
    public List<LevelCondition> Conditions;
    public List<Recipe> RecipesUsed;

    public Dictionary<Ingredient, int> StartingIngredients;
    public int StartingGold;
    public int startingIngredientQuantities = 10;

    public float marketVarianceMin = .5f;
    public float marketVarianceMax = 1.5f;

    LevelCondition introCondition;
    StoryEventData lastStoryEvent;

    public void AddIntroDialog(PortraitID portrait, string title, string dialogText, string optionText = "OK")
    {
        StoryEventData newEvent = new StoryEventData(title, portrait, dialogText, portrait.ToString(), optionText);

        if (introCondition == null)
        {
            introCondition = new LevelCondition(Result.Story, 0);
            introCondition.triggeredStory = newEvent;
            Conditions.Add(introCondition);
        }
        else
            lastStoryEvent.Choices.Add(newEvent);

        lastStoryEvent = newEvent;
    }
    public void AddIntroDialog(string optionText, string dialogText = "")
    {
         AddIntroDialog(lastStoryEvent.Portrait, lastStoryEvent.EventTitle, dialogText, optionText);
    }

    public LevelDefinition(LevelID myID, string Title, LevelID WinUnlock, int StartingGold, bool testLevel = false)
    {
        this.myID = myID;
        this.Title = Title;
        this.WinUnlock = WinUnlock;
        this.StartingGold = StartingGold;

        Conditions = new List<LevelCondition>();
        RecipesUsed = new List<Recipe>();
        StartingIngredients = new Dictionary<Ingredient, int>();

        if (testLevel)
        {
            RecipesUsed.Add(Recipe.DreamPowder);
            RecipesUsed.Add(Recipe.PassionPotion);
            RecipesUsed.Add(Recipe.FleaPoultice);
            FinishLevel();
        }
    }

    public void FinishLevel()
    {
        if (GameManager.RecipeBook == null)
            GameManager.LoadRecipes();

        foreach (Recipe recipe in RecipesUsed)
            foreach (Ingredient ingredient in GameManager.RecipeBook[recipe].Ingredients.Keys)
                if (!StartingIngredients.ContainsKey(ingredient))
                    StartingIngredients.Add(ingredient, startingIngredientQuantities);
    }

    //returns -1 for loss, 0 for not over, and 1 for won
    public int CheckGameOver()
    {
        foreach (LevelCondition condition in Conditions)
        {
            if (condition.hasBeenTriggered!=true && (condition.trigger == TriggerFrequency.Continuous || Timepiece.CurrentDay > condition.deadline))
            {
                if (condition.qualifier == Qualifier.None)
                    return condition.ifPassedResult();

                float comparedValue = condition.metric == Metric.Gold ? GameManager.Main.player.Gold : ProgressPanel.popularityPercent(GameManager.Main.player);

                if (condition.qualifier == Qualifier.GreaterThan)
                    if (comparedValue > condition.amount)
                        return condition.ifPassedResult();

                if (condition.qualifier == Qualifier.LessThan)
                    if (comparedValue < condition.amount)
                        return condition.ifPassedResult();
            }
        }
        return 0;
    }

    public bool HasWon()
    {
        return CheckGameOver() == 1;
    }

    public bool HasLost()
    {
        return CheckGameOver() == -1;
    }

    public void ResetConditions()
    {
        foreach (LevelCondition condition in Conditions)
            condition.hasBeenTriggered = false;
    }

    public List<StoryEventData> TriggerStoryEvent()
    {
        List<StoryEventData> TriggeredEvents = new List<StoryEventData>();
        foreach (LevelCondition condition in Conditions)
        {
            if (condition.result== Result.Story && (!condition.hasBeenTriggered && condition.triggeredStory != null) &&
                (condition.trigger == TriggerFrequency.Continuous || Timepiece.CurrentDay > condition.deadline))
            {
                if (condition.qualifier == Qualifier.None)
                {
                    TriggeredEvents.Add(condition.triggeredStory);
                    condition.hasBeenTriggered = true;
                }

                float comparedValue = condition.metric == Metric.Gold ? GameManager.Main.player.Gold : ProgressPanel.popularityPercent(GameManager.Main.player);

                if (condition.qualifier == Qualifier.GreaterThan)
                {
                    if (comparedValue > condition.amount)
                    {
                        TriggeredEvents.Add(condition.triggeredStory);
                        condition.hasBeenTriggered = true;
                    }
                }
                else if (condition.qualifier == Qualifier.LessThan)
                {
                    if (comparedValue < condition.amount)
                    {
                        TriggeredEvents.Add(condition.triggeredStory);
                        condition.hasBeenTriggered = true;
                    }
                }
            }
        }
        return TriggeredEvents;
    }
}