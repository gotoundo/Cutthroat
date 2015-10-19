using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LevelID { None, L1, L2, L3, L4, L5, L6, L7, L8, L9, L10 }

//this baby persists across all scenes
public class LevelManager : MonoBehaviour {

    public static LevelDefinition SelectedLevel;
    public static Dictionary<LevelID, LevelDefinition> LevelDefinitions;
    public static LevelManager Main;


	void Awake () {
        if (Main == null)
        {
            Main = this;
            DontDestroyOnLoad(gameObject);
            Setup();
        }
        else
            Destroy(gameObject);
	}

    void Setup()
    {
        Main = this;
        LevelDefinitions = new Dictionary<LevelID, LevelDefinition>();

        if (!SaveTool.Load())
            CheatWinLoseUI.ResetSaveData();
        
            LoadLevelData();
    }

    void LoadLevelData()
    {
        //Level 1 - Corgi Town
        LevelDefinition workingLevel = new LevelDefinition(LevelID.L1, "Corgi Town", LevelID.L2, 600);
        workingLevel.marketVarianceMin = 0.5f;
        workingLevel.marketVarianceMax = 1.4f;

        StoryEventData introEventData = new StoryEventData("Welcome!", PortraitID.Pomeranian, "Get of of my town you filthy peasant.", "Dr. Dogson");
        introEventData.Choices.Add(new StoryEventData("Uh, no?"));
        LevelCondition introCondition = new LevelCondition(Result.Story, 0);
        introCondition.triggeredStory = introEventData;
        workingLevel.Conditions.Add(introCondition);


        //workingLevel.MainObjectiveDescription = "Welcome to Puppy Potions! To win this game you have to become the most popular potion shop in town. Buy ingredients when they're cheap and adjust potion prices to remain profitable. Don't forget to buy marketing and upgrade your store! \n\n Get to 50% popularity before 30 days to win this town.";
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .5f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 30));
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        AddLevel(workingLevel);

        //Level 2 - The Forest
        workingLevel = new LevelDefinition(LevelID.L2, "The Forest", LevelID.L3, 600);
        workingLevel.MainObjectiveDescription = "Get 55% of all market share before 30 days are over to win!";
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .55f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 30));
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        AddLevel(workingLevel);

        //Level 3 - Doggerton
        workingLevel = new LevelDefinition(LevelID.L3, "Doggerton", LevelID.L4, 400);
        workingLevel.MainObjectiveDescription = "Get 60% of all market share before 25 days are over to win!";
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .7f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 25));
        workingLevel.RecipesUsed.Add(Recipe.QuickElixer);
        workingLevel.RecipesUsed.Add(Recipe.FleaPoultice);
        AddLevel(workingLevel);

        //Level 4 - Pooch City
        workingLevel = new LevelDefinition(LevelID.L4, "Husky Village", LevelID.L5, 500);
        workingLevel.MainObjectiveDescription = "Get 65% of all market share before 25 days are over to win!";
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .8f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 25));
        workingLevel.RecipesUsed.Add(Recipe.QuickElixer);
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        AddLevel(workingLevel);

        //Level 5 - Pooch City
        workingLevel = new LevelDefinition(LevelID.L5, "Pup Harbor", LevelID.L6, 400);
        workingLevel.MainObjectiveDescription = "Get 70% of all market share before 25 days are over to win!";
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .8f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 25));
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        workingLevel.RecipesUsed.Add(Recipe.FleaPoultice);
        AddLevel(workingLevel);

        //Level 6 - Pooch City
        workingLevel = new LevelDefinition(LevelID.L6, "South Paw", LevelID.L7, 600);
        workingLevel.MainObjectiveDescription = "Get 75% of all market share before 25 days are over to win!";
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .8f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 25));
        workingLevel.RecipesUsed.Add(Recipe.QuickElixer);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        workingLevel.RecipesUsed.Add(Recipe.FleaPoultice);
        AddLevel(workingLevel);

        //Level 7 - Pooch City
        workingLevel = new LevelDefinition(LevelID.L7, "Lazy Shire", LevelID.L8, 500);
        workingLevel.MainObjectiveDescription = "Get 80% of all market share before 20 days are over to win!";
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .8f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 20));
        workingLevel.RecipesUsed.Add(Recipe.QuickElixer);
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.RecipesUsed.Add(Recipe.FleaPoultice);
        AddLevel(workingLevel);

        //Level 8 - Pooch City
        workingLevel = new LevelDefinition(LevelID.L8, "Pooch City", LevelID.L9, 400);
        workingLevel.MainObjectiveDescription = "Get 85% of all market share before 20 days are over to win!";
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .85f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 20));
        workingLevel.RecipesUsed.Add(Recipe.QuickElixer);
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        AddLevel(workingLevel);

        //Level 8 - Pooch City
        workingLevel = new LevelDefinition(LevelID.L9, "Dogtropolis", LevelID.None, 600);
        workingLevel.MainObjectiveDescription = "Get 90% of all market share before 20 days are over to win!";
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .9f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 20));
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        workingLevel.RecipesUsed.Add(Recipe.FleaPoultice);
        AddLevel(workingLevel);
    }

    void AddLevel(LevelDefinition level)
    {
        level.FinishLevel();
        LevelDefinitions.Add(level.myID, level);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnApplicationQuit()
    {
        SaveTool.Save();
    }
}
