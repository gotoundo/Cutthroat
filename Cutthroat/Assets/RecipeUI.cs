using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour {

    public Text Price;
    public Text Name;
    public Text Ingredients;
    public Recipe myRecipe;
    public Slider progressSlider;
    StoreBase player;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null)
            player = GameManager.singleton.player;

        Name.text = myRecipe.ToString();
        Price.text = ""+player.ProductCost(myRecipe);
        Ingredients.text = "";
        foreach(Ingredient ingr in GameManager.singleton.recipeBook[myRecipe].Keys)
        {
            if (!Ingredients.text.Equals(""))
                Ingredients.text += "\n";
            Ingredients.text += ingr.ToString() + " " + GameManager.singleton.recipeBook[myRecipe][ingr];
        }

    }

    public void ModifyPrice(int amount = 1)
    {
        player.ModifyPrice(myRecipe, amount);
    }
}
