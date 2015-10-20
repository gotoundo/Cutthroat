using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureManager : MonoBehaviour {

    public static TextureManager Main;
    public static Dictionary<Recipe, Sprite> PotionTextures;
    //public static Dictionary<Ingredient, Sprite> IngredientTextures;
    public static Dictionary<PortraitID, Sprite> PortraitTextures;
    public static Dictionary<StoreUpgrade.Type, Sprite> UpgradeTextures;

    public Sprite[] OverheadIcons;
    public Sprite[] PotionIcons;
    public Sprite[] IngredientIcons;
    public Sprite[] PortraitIcons;
    public Sprite[] UpgradeIcons;

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
        PotionTextures.Add(Recipe.DreamPowder, PotionIcons[5]);
        PotionTextures.Add(Recipe.PassionPotion, PotionIcons[1]);
        PotionTextures.Add(Recipe.QuickElixer, PotionIcons[3]);
        PotionTextures.Add(Recipe.FleaPoultice, PotionIcons[0]);

        /*IngredientTextures = new Dictionary<Ingredient, Sprite>();
        IngredientTextures.Add(Ingredient.Red, IngredientIcons[0]);
        IngredientTextures.Add(Ingredient.Orange, IngredientIcons[1]);
        IngredientTextures.Add(Ingredient.Yellow, IngredientIcons[2]);
        IngredientTextures.Add(Ingredient.Green, IngredientIcons[3]);
        IngredientTextures.Add(Ingredient.Blue, IngredientIcons[4]);
        IngredientTextures.Add(Ingredient.Purple, IngredientIcons[5]);*/


        PortraitTextures = new Dictionary<PortraitID, Sprite>();
        PortraitTextures.Add(PortraitID.Beagle, PortraitIcons[0]);
        PortraitTextures.Add(PortraitID.BostonTerrier, PortraitIcons[1]);
        PortraitTextures.Add(PortraitID.Bulldog, PortraitIcons[2]);
        PortraitTextures.Add(PortraitID.BullTerrier, PortraitIcons[3]);
        PortraitTextures.Add(PortraitID.Chihuahua, PortraitIcons[4]);
        PortraitTextures.Add(PortraitID.GermanShepherd, PortraitIcons[5]);
        PortraitTextures.Add(PortraitID.LabradorRetriever, PortraitIcons[6]);
        PortraitTextures.Add(PortraitID.PitBull, PortraitIcons[7]);
        PortraitTextures.Add(PortraitID.Pomeranian, PortraitIcons[8]);
        PortraitTextures.Add(PortraitID.Pug, PortraitIcons[9]);
        PortraitTextures.Add(PortraitID.Shihzu, PortraitIcons[10]);
        PortraitTextures.Add(PortraitID.SiberianHusky, PortraitIcons[11]);

        UpgradeTextures = new Dictionary<StoreUpgrade.Type, Sprite>();
        UpgradeTextures.Add(StoreUpgrade.Type.Amenities, UpgradeIcons[0]);
        UpgradeTextures.Add(StoreUpgrade.Type.ProductionSpeed, UpgradeIcons[1]);
        UpgradeTextures.Add(StoreUpgrade.Type.Storefront, UpgradeIcons[2]);
    }

    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
