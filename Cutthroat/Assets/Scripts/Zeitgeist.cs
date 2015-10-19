using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zeitgeist : MonoBehaviour
{
    public static WeightedCollection<Recipe> RecipePopularities;
    public static WeightedCollection<Recipe> NextRecipePopularities;

    public static Recipe TodaysTopPotion;
    public static Recipe TomorrowsTopPotion;

    public static void Initialize(LevelDefinition level)
    {
        RecipePopularities = new WeightedCollection<Recipe>();
        NextRecipePopularities = new WeightedCollection<Recipe>();

        foreach (Recipe recipe in level.RecipesUsed)
            NextRecipePopularities.AddWeight(recipe, 0);

        RandomizePopularities();
        RandomizePopularities();
    }
    
    public static void RandomizePopularities()
    {
        RecipePopularities = NextRecipePopularities;
        NextRecipePopularities = new WeightedCollection<Recipe>();

        foreach (Recipe recipe in RecipePopularities.KeyList())
            NextRecipePopularities.AddWeight(recipe, Random.Range(1, 10));

        TodaysTopPotion = TopPotion(RecipePopularities);
        TomorrowsTopPotion = TopPotion(NextRecipePopularities);
    }

    static Recipe TopPotion(WeightedCollection<Recipe> DailyPopularity)
    {
        Recipe winner = Recipe.None;
        float winningPopularity = 0;
        foreach (Recipe recipe in DailyPopularity.KeyList())
        {
            if(NextRecipePopularities[recipe]>=winningPopularity)
            {
                winner = recipe;
                winningPopularity = DailyPopularity[recipe];
            }
        }
        return winner;
    }

   
    

}
