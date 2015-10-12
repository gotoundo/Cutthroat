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

    public LevelDefinition(LevelID myID,  bool testLevel = false)
    {
        this.myID = myID;
        Conditions = new List<LevelCondition>();
        RecipesUsed = new List<Recipe>();
        StartingIngredients = new Dictionary<Ingredient, int>();

        if (testLevel)
        {
            RecipesUsed.Add(Recipe.DreamPowder);
            FinishLevel();
        }
    }

    public void FinishLevel()
    {
        if(GameManager.RecipeBook == null)
            GameManager.LoadRecipes();

        foreach (Recipe recipe in RecipesUsed)
            foreach (Ingredient ingredient in GameManager.RecipeBook[recipe].Keys)
                if (!StartingIngredients.ContainsKey(ingredient))
                    StartingIngredients.Add(ingredient, startingIngredientQuantities);
    }

    //returns -1 for loss, 0 for not over, and 1 for won
    public int CheckGameOver()
    {
        foreach (LevelCondition condition in Conditions)
        {
            if(condition.trigger == TriggerFrequency.Continuous || Timepiece.CurrentDay > condition.deadline)
            {
                if (condition.qualifier == Qualifier.None)
                    return condition.ifPassedResult();

                float comparedValue = condition.metric == Metric.Gold ? GameManager.singleton.player.Gold : ProgressPanel.popularityPercent(GameManager.singleton.player);

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
}