using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RecipeUI : MonoBehaviour {

    public Image HotnessIndicator;

    public Text Price;
    public Text Name;
    //public Text Ingredients;
    public Recipe myRecipe;
    public Slider PriceSlider;
    public GameObject IngredientsRequired;
    public GameObject IngredientQuantityWidget;
    public Image RecipeIcon;
    StoreBase player;
    // Use this for initialization
    static int x = 0;

    void Awake()
    {

    }

    void Start () {

       // transform.localScale = new Vector3(1, 1, 1);
        x++;
        Name.text = GameManager.RecipeBook[myRecipe].Name;

        RecipeIcon.overrideSprite = TextureManager.PotionTextures[myRecipe];

        List<Ingredient>  StartingList = new List<Ingredient>(GameManager.RecipeBook[myRecipe].Ingredients.Keys);
        List<Ingredient> EndingList = new List<Ingredient>();
        foreach (Ingredient ingr in StartingList)
        {
            //Debug.Log(gameObject.name + myRecipe.ToString()+ingr.ToString());
            EndingList.Add(ingr);
            GameObject ingredientCount = Instantiate(IngredientQuantityWidget);
            ingredientCount.name = myRecipe.ToString() + " " + ingr.ToString();
            ingredientCount.transform.SetParent(IngredientsRequired.transform);
            IngredientUI ingredientUI = ingredientCount.GetComponent<IngredientUI>();
            ingredientUI.ingredient = ingr;
            ingredientUI.count.text = "x" + GameManager.RecipeBook[myRecipe].Ingredients[ingr] + "";
        }

        PriceSlider.maxValue = IngredientStore.AverageRecipeCost(myRecipe) * 3;
        PriceSlider.value = IngredientStore.AverageRecipeCost(myRecipe) * 1.2f;
    }
	
	// Update is called once per frame
	void Update () {
        if (player == null)
            player = GameManager.Main.player;

        
        player.SetPrice(myRecipe, Mathf.RoundToInt(PriceSlider.value));
        Price.text = "$"+player.ProductCost(myRecipe);
    }

    public void ModifyPrice(int amount = 1)
    {
        player.ModifyPrice(myRecipe, amount);
    }
}
