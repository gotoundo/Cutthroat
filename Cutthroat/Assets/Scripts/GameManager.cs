using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Recipe { DreamPowder, PassionPotion, QuickElixer }

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

    public static Dictionary<Recipe, Dictionary<Ingredient, int>> RecipeBook;
    public static Dictionary<Recipe, string> RecipeNames;
    
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
        RecipeBook = new Dictionary<Recipe, Dictionary<Ingredient, int>>();
        RecipeNames = new Dictionary<Recipe, string>();

        RecipeNames.Add(Recipe.DreamPowder, "Dream Powder");
        Dictionary<Ingredient, int> dreamPowderRecipe = new Dictionary<Ingredient, int>();
        dreamPowderRecipe.Add(Ingredient.Ruby, 2);
        RecipeBook.Add(Recipe.DreamPowder, dreamPowderRecipe);

        RecipeNames.Add(Recipe.PassionPotion, "Passion Potion");
        Dictionary<Ingredient, int> passionPotionRecipe = new Dictionary<Ingredient, int>();
        passionPotionRecipe.Add(Ingredient.Emerald, 2);
        passionPotionRecipe.Add(Ingredient.Topaz, 2);
        RecipeBook.Add(Recipe.PassionPotion, passionPotionRecipe);

        RecipeNames.Add(Recipe.QuickElixer, "Quick Elixer");
        Dictionary<Ingredient, int> quickElixerRecipe = new Dictionary<Ingredient, int>();
        quickElixerRecipe.Add(Ingredient.Ruby, 1);
        quickElixerRecipe.Add(Ingredient.Emerald, 1);
        quickElixerRecipe.Add(Ingredient.Sapphire, 1);
        RecipeBook.Add(Recipe.QuickElixer, quickElixerRecipe);
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
