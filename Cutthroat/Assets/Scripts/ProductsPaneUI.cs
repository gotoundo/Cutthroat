using UnityEngine;
using System.Collections.Generic;

public class ProductsPaneUI : MonoBehaviour {
    public GameObject RowTemplate;

    List<RecipeUI> RecipeRows;

    public static ProductsPaneUI Main;
	// Use this for initialization
	void Start () {
        Main = this;
        RecipeRows = new List<RecipeUI>();
        foreach(Recipe recipe in GameManager.Main.CurrentLevel.RecipesUsed)
        {
            GameObject recipeRow = (GameObject)Instantiate(RowTemplate);
            recipeRow.transform.SetParent(transform);
            recipeRow.GetComponent<RecipeUI>().myRecipe = recipe;
            RecipeRows.Add(recipeRow.GetComponent<RecipeUI>());
        }
        UpdateRecipesStatus();
       /* RowTemplate.SetActive(false);
        Destroy(RowTemplate);*/
    }

    public static void UpdateRecipesStatus()
    {
        foreach(RecipeUI recipeui in Main.RecipeRows)
        {
            recipeui.HotnessIndicator.gameObject.SetActive(Zeitgeist.TodaysTopPotion == recipeui.myRecipe);
        }
    }


	
	// Update is called once per frame
	void Update () {
        //
    }
}
