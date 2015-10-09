using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureManager : MonoBehaviour {

    public static TextureManager Main;
    public static Dictionary<Recipe, Sprite> PotionTextures;
    public static Dictionary<Ingredient, Sprite> IngredientTextures;

    public Sprite[] OverheadIcons;
    public Sprite[] PotionIcons;
    public Sprite[] IngredientIcons;

    // Use this for initialization
    void Awake()
    {
        if (Main == null)
        {
            Main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        PotionTextures = new Dictionary<Recipe, Sprite>();
        PotionTextures.Add(Recipe.DreamPowder, PotionIcons[0]);
        PotionTextures.Add(Recipe.PassionPotion, PotionIcons[1]);

        IngredientTextures = new Dictionary<Ingredient, Sprite>();
        IngredientTextures.Add(Ingredient.Emerald, IngredientIcons[0]);
        IngredientTextures.Add(Ingredient.Ruby, IngredientIcons[1]);
        IngredientTextures.Add(Ingredient.Sapphire, IngredientIcons[2]);
        IngredientTextures.Add(Ingredient.Topaz, IngredientIcons[3]);
    }

    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
