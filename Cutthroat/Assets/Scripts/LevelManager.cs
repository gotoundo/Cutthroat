﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LevelID { None, L1, L2, L3, L4, L5, L6 }

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
        
        if(!SaveTool.Load())
        {
            SaveData.current = new SaveData();
            SaveData.current.UnlockedLevels.Add(LevelID.L1);
            SaveTool.Save();
        }


        LoadLevelData();
    }

    void LoadLevelData()
    {
        //Level 1 - Corgi Town
        LevelDefinition workingLevel = new LevelDefinition(LevelID.L1);
        workingLevel.Title = "Corgi Town";
        workingLevel.MainObjectiveDescription = "Welcome to Puppy Potions! To win this game you have to become the most popular potion shop in town. Buy ingredients when they're cheap and adjust potion prices to remain profitable. Don't forget to buy marketing and upgrade your store! \n\n Get to 50% popularity before 30 days to win this town.";
        workingLevel.StartingGold = 600;
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .5f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 30));
        workingLevel.WinUnlock = LevelID.L2;
        AddLevel(workingLevel);

        //Level 2 - The Forest
        workingLevel = new LevelDefinition(LevelID.L2);
        workingLevel.Title = "The Forest";
        workingLevel.MainObjectiveDescription = "Get 60% of all market share before 25 days are over to win!";
        workingLevel.StartingGold = 500;
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .6f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 25));
        workingLevel.WinUnlock = LevelID.L3;
        AddLevel(workingLevel);

        //Level 3 - Doggerton
        workingLevel = new LevelDefinition(LevelID.L3);
        workingLevel.Title = "Doggerton";
        workingLevel.MainObjectiveDescription = "Get 70% of all market share before 25 days are over to win!";
        workingLevel.StartingGold = 500;
        workingLevel.RecipesUsed.Add(Recipe.QuickElixer);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .7f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 25));
        workingLevel.WinUnlock = LevelID.L4;
        AddLevel(workingLevel);

        //Level 4 - Pooch City
        workingLevel = new LevelDefinition(LevelID.L4);
        workingLevel.Title = "Pooch City";
        workingLevel.MainObjectiveDescription = "Get 80% of all market share before 25 days are over to win!";
        workingLevel.StartingGold = 500;
        workingLevel.RecipesUsed.Add(Recipe.QuickElixer);
        workingLevel.RecipesUsed.Add(Recipe.DreamPowder);
        workingLevel.RecipesUsed.Add(Recipe.PassionPotion);
        workingLevel.Conditions.Add(new LevelCondition(Result.Win, TriggerFrequency.Continuous, Qualifier.GreaterThan, Metric.PopularityPercent, .8f));
        workingLevel.Conditions.Add(new LevelCondition(Result.Lose, 25));
        workingLevel.WinUnlock = LevelID.L5;
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