using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ForecastUI : MonoBehaviour {
    public Text DebugText;
    int currentDay = 0;

	// Use this for initialization
	void Start () {
        currentDay = Timepiece.CurrentDay;
        ReloadText();
	
	}
	
	// Update is called once per frame
	void Update () {
        if (currentDay != Timepiece.CurrentDay)
        {
            currentDay = Timepiece.CurrentDay;
            ReloadText();
        }
    }

    void ReloadText()
    {
        
        DebugText.text = "";
        foreach (KeyValuePair<Ingredient, int> entry in IngredientStore.NextIngredientPrices)
        {
            DebugText.text += entry.Key.ToString() + " will cost " + entry.Value + "\n";
        }
        foreach (Recipe recipe in Zeitgeist.NextRecipePopularities.KeyList())
        {
            DebugText.text += recipe.ToString() + " will have popularity of " + Zeitgeist.RecipePopularities.ChanceOfItem(recipe, true) + "%\n";
        }
    }
}
