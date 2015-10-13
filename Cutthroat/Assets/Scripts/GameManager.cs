using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Recipe { DreamPowder, PassionPotion, QuickElixer }

public class RecipeDescription
{
    public string Name;
    public string Description;
    public Recipe Type;
    public int SpriteID;
    public Dictionary<Ingredient, int> Ingredients;
    public RecipeDescription(Recipe Type, string Name, int SpriteID)
    {
        Ingredients = new Dictionary<Ingredient, int>();
        this.Type = Type;
        this.Name = Name;
        this.SpriteID = SpriteID;
        GameManager.RecipeBook.Add(Type, this);
    }
}

//this guy is recreated every time we load a level scene
public class GameManager : MonoBehaviour {

    public static GameManager singleton;
    public static List<StoreBase> AllStores;
	public static List<CustomerScript> AllCustomers;
    public static List<HouseScript> AllHouses;

    public GameObject playerStore;
    public GameObject inspectorPanel;
    public GameObject SelectedObject;
    public GameObject IntroPanel;
    public GameObject WinPanel;
    public GameObject LosePanel;

    public Text playerIngredientDisplay;
    public Text playerGoldDisplay;

    public static Dictionary<Recipe, RecipeDescription> RecipeBook;
   // public static Dictionary<Recipe, string> RecipeNames;
    
    public StoreBase player;
    public LevelDefinition CurrentLevel;
    public bool gameRunning;

    public bool autoWin = false;
    public bool autoLose = false;

    // Use this for initialization
    void Awake () {
        AllStores = new List<StoreBase>();
        AllCustomers = new List<CustomerScript>();
        AllHouses = new List<HouseScript>();

        player = playerStore.GetComponent<StoreBase>();
        LoadRecipes();

        singleton = this;
        gameRunning = false;

        CurrentLevel = LevelManager.SelectedLevel != null ? LevelManager.SelectedLevel : new LevelDefinition(LevelID.None, true);

        Zeitgeist.Initialize(CurrentLevel);
        StoreUpgrade.Initialize();
        GetComponent<IngredientStore>().Initialize();
    }

    public static void LoadRecipes()
    {
        RecipeBook = new Dictionary<Recipe, RecipeDescription>();
        // RecipeNames = new Dictionary<Recipe, string>();

        RecipeDescription dreamPowderRecipe = new RecipeDescription(Recipe.DreamPowder,"Dream Powder",0);
        dreamPowderRecipe.Ingredients.Add(Ingredient.Ruby, 2);
        dreamPowderRecipe.Description = "Allows puppies with insomnia to gently fall asleep.";

        RecipeDescription passionPotionRecipe = new RecipeDescription(Recipe.PassionPotion, "Passion Potion", 0);
        passionPotionRecipe.Ingredients.Add(Ingredient.Emerald, 1);
        passionPotionRecipe.Ingredients.Add(Ingredient.Topaz, 2);
        passionPotionRecipe.Description = "Gives a boost to energy so a puppy can play all day!";

        RecipeDescription quickElixerRecipe = new RecipeDescription(Recipe.QuickElixer, "Quick Elixer", 0);
        quickElixerRecipe.Ingredients.Add(Ingredient.Ruby, 1);
        quickElixerRecipe.Ingredients.Add(Ingredient.Emerald, 1);
        quickElixerRecipe.Ingredients.Add(Ingredient.Sapphire, 1);
        quickElixerRecipe.Description = "Drinking this elixer allows the puppy to move super fast.";
    }

    void Start()
    {
        AudioManager.Main.Source.clip = AudioManager.Main.Music[0];
        AudioManager.Main.Source.Play();

        IntroPanel.SetActive(true);
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
    }

    public void BeginPlay()
    {
        gameRunning = true;
        AudioManager.Main.BarkingDogs = true;
        foreach (HouseScript house in AllHouses)
            house.SpawnCustomers();
    }



    // Update is called once per frame

    bool saved = false;
    void Update()
    {
        playerGoldDisplay.text = ""+playerStore.GetComponent<StoreBase>().Gold+" Gold";

        playerIngredientDisplay.text = "-- Ingredients --";
        foreach (Ingredient ingr in player.GetIngredients().Keys)
            playerIngredientDisplay.text += "\n" + ingr.ToString() + ":  " + player.GetIngredients()[ingr];

        if(autoWin || gameRunning && CurrentLevel.HasWon())
        {
            gameRunning = false;
            WinPanel.SetActive(true);
            if (!saved)
            {
                saved = true;
                SaveData.VictoryUnlock(CurrentLevel.WinUnlock);
                SaveTool.Save();
            }
        }
        else if (autoLose || gameRunning && CurrentLevel.HasLost())
        {
            gameRunning = false;
            LosePanel.SetActive(true);
        }

        
    }

    public void MakeSelection(GameObject newSelection)
    {
        inspectorPanel.SetActive(true);

        if (SelectedObject != null)
            SelectedObject.GetComponent<Inspectable>().Deselect();

        newSelection.GetComponent<Inspectable>().Select();
        SelectedObject = newSelection;
    }

    public void CloseInspector()
    {
        if (SelectedObject != null)
            SelectedObject.GetComponent<Inspectable>().Deselect();
        inspectorPanel.SetActive(false);
    }

}
