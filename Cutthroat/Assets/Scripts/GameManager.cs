using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//this guy is recreated every time we load a level scene
public class GameManager : MonoBehaviour
{
    public static GameManager Main;
    public static List<StoreBase> AllStores;
    public static List<CustomerScript> AllCustomers;
    public static List<HouseScript> AllHouses;
    public static Dictionary<Recipe, RecipeDescription> RecipeBook;
    public static Dictionary<Ingredient, IngredientDescription> IngredientBook;

    public GameObject playerStore;
    public GameObject inspectorPanel;
    public GameObject SelectedObject;
    public GameObject IntroPanel;
    public GameObject StoryPanel;
    public GameObject WinPanel;
    public GameObject LosePanel;

    public StoreBase player;
    public LevelDefinition CurrentLevel;
    public bool gameRunning;

    public bool autoWin = false;
    public bool autoLose = false;

    // Use this for initialization
    void Awake()
    {
        AllStores = new List<StoreBase>();
        AllCustomers = new List<CustomerScript>();
        AllHouses = new List<HouseScript>();

        player = playerStore.GetComponent<StoreBase>();
        LoadIngredients();
        LoadRecipes();

        Main = this;
        gameRunning = true;

        CurrentLevel = LevelManager.SelectedLevel != null ? LevelManager.SelectedLevel : new LevelDefinition(LevelID.None, "Default Level", LevelID.None, 500, true);
        CurrentLevel.ResetConditions();
        Zeitgeist.Initialize(CurrentLevel);
        StoreUpgrade.Initialize();
        GetComponent<IngredientStore>().Initialize();
    }

    void Start()
    {
        AudioManager.Main.Source.clip = AudioManager.Main.Music[0];
        AudioManager.Main.Source.Play();


        //IntroPanel.SetActive(true);
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
    }

    bool puppiesCreated = false;
    void Update()
    {
        if (!puppiesCreated)
            CreatePuppies();

        if (gameRunning)
        {

            List<StoryEventData> TriggeredEvents = CurrentLevel.TriggerStoryEvent();
            RunTriggeredEvents(TriggeredEvents);
        }

        CheckWinOrLose();
    }

    public void RunTriggeredEvents(List<StoryEventData> TriggeredEvents)
    {
        if (TriggeredEvents.Count > 0)
        {
            gameRunning = false;
            StoryPanel.SetActive(true);
            StoryEventData storyData = TriggeredEvents[0];
            StoryWindowUI storyWindow = StoryPanel.GetComponent<StoryWindowUI>();

            storyWindow.Dialog.text = storyData.Description;
            storyWindow.Title.text = storyData.EventTitle;
            storyWindow.NPCName.text = storyData.NPCName;
            storyWindow.Portrait.overrideSprite = TextureManager.PortraitTextures[storyData.Portrait];

            if (storyWindow.OptionsButtons == null)
                storyWindow.OptionsButtons = new List<GameObject>();
            else
            {
                foreach (GameObject o in storyWindow.OptionsButtons)
                    Destroy(o);
            }

            foreach (StoryEventData choiceData in storyData.Choices)
            {
                GameObject choiceObject = Instantiate(storyWindow.OptionsButtonTemplate);
                storyWindow.OptionsButtons.Add(choiceObject);
                choiceObject.transform.SetParent(storyWindow.OptionsPanel.transform);
                StoryChoiceUI choiceUI = choiceObject.GetComponent<StoryChoiceUI>();
                choiceUI.ButtonText.text = choiceData.ButtonText;
                choiceUI.StoryResult = choiceData;
            }

            if (storyData.Choices.Count == 0)
            {
                gameRunning = true;
                StoryPanel.SetActive(false);
            }
        }
    }

    public static void LoadRecipes()
    {
        RecipeBook = new Dictionary<Recipe, RecipeDescription>();

        RecipeDescription dreamPowderRecipe = new RecipeDescription(Recipe.DreamPowder, "Dream Powder", "Allows puppies with insomnia to gently fall asleep.");
        dreamPowderRecipe.Ingredients.Add(Ingredient.Yellow, 1);
        dreamPowderRecipe.Ingredients.Add(Ingredient.Blue, 1);

        RecipeDescription passionPotionRecipe = new RecipeDescription(Recipe.PassionPotion, "Passion Potion", "Gives a boost to energy so a puppy can play all day!");
        passionPotionRecipe.Ingredients.Add(Ingredient.Red, 2);

        RecipeDescription quickElixerRecipe = new RecipeDescription(Recipe.QuickElixer, "Quick Elixer", "Drinking this elixer allows the puppy to move super fast.");
        quickElixerRecipe.Ingredients.Add(Ingredient.Red, 1);
        quickElixerRecipe.Ingredients.Add(Ingredient.Orange, 1);
        quickElixerRecipe.Ingredients.Add(Ingredient.Yellow, 1);

        RecipeDescription fleaPoulticeRecipe = new RecipeDescription(Recipe.FleaPoultice, "Flea Poultice", "Applying this lotion removes a nasty case of the fleas.");
        fleaPoulticeRecipe.Ingredients.Add(Ingredient.Red, 1);
        fleaPoulticeRecipe.Ingredients.Add(Ingredient.Green, 1);
    }

    public static void LoadIngredients()
    {
        IngredientBook = new Dictionary<Ingredient, IngredientDescription>();
        new IngredientDescription(Ingredient.Red, "Spices", "Hot spices", 0, 20);
        new IngredientDescription(Ingredient.Orange, "Herbs", "Exotic flowers", 1, 15);
        new IngredientDescription(Ingredient.Yellow, "Mushroom", "Mystic fungi", 2, 5);
        new IngredientDescription(Ingredient.Green, "Eggs", "Unhatched drake eggs", 3, 15);
        new IngredientDescription(Ingredient.Blue, "Arctic Ice", "Magically frozen water", 4, 10);
        new IngredientDescription(Ingredient.Purple, "Fish", "From the deepest, darkest lakes", 5, 25);
        new IngredientDescription(Ingredient.White, "Bone", "Recovered from the ziggurat", 6, 8);
        new IngredientDescription(Ingredient.Black, "Onyx", "Necromancer's treasure", 7, 50);
    }


    void CreatePuppies()
    {
        puppiesCreated = true;
        gameRunning = true;
        AudioManager.Main.BarkingDogs = true;
        foreach (HouseScript house in AllHouses)
            house.SpawnCustomers();
    }


    bool saved = false;
    void CheckWinOrLose()
    {



        if (autoWin || gameRunning && CurrentLevel.HasWon())
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