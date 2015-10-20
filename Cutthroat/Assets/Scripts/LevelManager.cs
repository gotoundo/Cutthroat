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

        workingLevel.AddIntroDialog(PortraitID.Shihzu, "Welcome!", "Great. Another competator? That's the last thing I need.");
        workingLevel.AddIntroDialog("Um, hi?", "Look, the basics are: buy ingredients so puppies can buy your potions.");
        workingLevel.AddIntroDialog("But...", "You'll win this town if you can get to 40% market share within 30 days. I doubt you will.");
        workingLevel.AddIntroDialog("Ok Boss!");

        //workingLevel.MainObjectiveDescription = "Welcome to Puppy Potions! To win this game you have to become the most popular potion shop in town. Buy ingredients when they're cheap and adjust potion prices to remain profitable. Don't forget to buy marketing and upgrade your store! \n\n Get to 50% popularity before 30 days to win this town.";
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .40f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 30));
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        AddLevel(workingLevel);

        //Level 2 - The Forest
        workingLevel = new LevelDefinition(LevelID.L2, "The Forest", LevelID.L3, 600);
        workingLevel.AddIntroDialog(PortraitID.Shihzu, workingLevel.Title, "Get 45% market share before 30 days are over to win this town.");
        workingLevel.AddIntroDialog("Ok Boss!");
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .45f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 30));
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        AddLevel(workingLevel);

        //Level 3 - Doggerton
        workingLevel = new LevelDefinition(LevelID.L3, "Doggerton", LevelID.L4, 400);
        workingLevel.AddIntroDialog(PortraitID.Shihzu, workingLevel.Title, "Get 50% market share before 25 days are over to win this town.");
        workingLevel.AddIntroDialog("Ok Boss!");
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .50f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 25));
        workingLevel.RecipesUsed.Add(Recipe.QuickElixer);
        workingLevel.RecipesUsed.Add(Recipe.FleaPoultice);
        AddLevel(workingLevel);

        //Level 4 - Pooch City
        workingLevel = new LevelDefinition(LevelID.L4, "Husky Village", LevelID.L5, 500);
        workingLevel.AddIntroDialog(PortraitID.Shihzu, workingLevel.Title, "Get 55% market share before 25 days are over to win this town.");
        workingLevel.AddIntroDialog("Ok Boss!");
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .55f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 25));
        workingLevel.RecipesUsed.Add(Recipe.QuickElixer);
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        AddLevel(workingLevel);

        //Level 5 - Pooch City
        workingLevel = new LevelDefinition(LevelID.L5, "Pup Harbor", LevelID.L6, 400);
        workingLevel.AddIntroDialog(PortraitID.Shihzu, workingLevel.Title, "Get 60% market share before 25 days are over to win this town.");
        workingLevel.AddIntroDialog("Ok Boss!");
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .60f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 25));
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        workingLevel.RecipesUsed.Add(Recipe.FleaPoultice);
        AddLevel(workingLevel);

        //Level 6 - Pooch City
        workingLevel = new LevelDefinition(LevelID.L6, "South Paw", LevelID.L7, 600);
        workingLevel.AddIntroDialog(PortraitID.Shihzu, workingLevel.Title, "Get 65% market share before 25 days are over to win this town.");
        workingLevel.AddIntroDialog("Ok Boss!");
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .65f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 25));
        workingLevel.RecipesUsed.Add(Recipe.QuickElixer);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        workingLevel.RecipesUsed.Add(Recipe.FleaPoultice);
        AddLevel(workingLevel);

        //Level 7 - Pooch City
        workingLevel = new LevelDefinition(LevelID.L7, "Lazy Shire", LevelID.L8, 500);
        workingLevel.AddIntroDialog(PortraitID.Shihzu, workingLevel.Title, "Get 70% market share before 25 days are over to win this town.");
        workingLevel.AddIntroDialog("Ok Boss!");
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .7f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 20));
        workingLevel.RecipesUsed.Add(Recipe.QuickElixer);
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.RecipesUsed.Add(Recipe.FleaPoultice);
        AddLevel(workingLevel);

        //Level 8 - Pooch City
        workingLevel = new LevelDefinition(LevelID.L8, "Pooch City", LevelID.L9, 400);
        workingLevel.AddIntroDialog(PortraitID.Shihzu, workingLevel.Title, "Get 75% market share before 20 days are over to win this town.");
        workingLevel.AddIntroDialog("Ok Boss!");
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .75f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 20));
        workingLevel.RecipesUsed.Add(Recipe.QuickElixer);
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        AddLevel(workingLevel);

        //Level 9 - Dogtropolis
        workingLevel = new LevelDefinition(LevelID.L9, "Dogtropolis", LevelID.None, 600);
        workingLevel.AddIntroDialog(PortraitID.Shihzu, workingLevel.Title, "Get 80% market share before 20 days are over to win this town.");
        workingLevel.AddIntroDialog("Ok Boss!");
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .8f));
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
