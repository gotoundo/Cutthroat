using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Recipe { None, DreamPowder, PassionPotion, QuickElixer, FleaPoultice }

public class RecipeDescription
{
    public string Name;
    public string Description;
    public Recipe Type;
    public Sprite Sprite
    {
        get { return TextureManager.PotionTextures[Type]; }
    }


    public Dictionary<Ingredient, int> Ingredients;
    public RecipeDescription(Recipe Type, string Name)
    {
        Ingredients = new Dictionary<Ingredient, int>();
        this.Type = Type;
        this.Name = Name;
        
        GameManager.RecipeBook.Add(Type, this);
    }
}

//this guy is recreated every time we load a level scene
public class GameManager : MonoBehaviour {

    public static GameManager Main;
    public static List<StoreBase> AllStores;
	public static List<CustomerScript> AllCustomers;
    public static List<HouseScript> AllHouses;
    public static Dictionary<Recipe, RecipeDescription> RecipeBook;

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
    void Awake () {
        AllStores = new List<StoreBase>();
        AllCustomers = new List<CustomerScript>();
        AllHouses = new List<HouseScript>();

        player = playerStore.GetComponent<StoreBase>();
        LoadRecipes();

        Main = this;
        gameRunning = true;

        CurrentLevel = LevelManager.SelectedLevel != null ? LevelManager.SelectedLevel : new LevelDefinition(LevelID.None,"Default Level",LevelID.None,500, true);
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

            foreach (StoryEventData choiceData in storyData.Choices)
            {
                GameObject choiceObject = Instantiate(storyWindow.OptionsButtonTemplate);
                choiceObject.transform.SetParent(storyWindow.OptionsPanel.transform);
                StoryChoiceUI choiceUI = choiceObject.GetComponent<StoryChoiceUI>();
                choiceUI.ButtonText.text = choiceData.ButtonText;
                choiceUI.StoryResult = choiceData;
            }

            if(storyData.Choices.Count == 0)
            {
                gameRunning = true;
                StoryPanel.SetActive(false);
            }
        }
    }

    public static void LoadRecipes()
    {
        RecipeBook = new Dictionary<Recipe, RecipeDescription>();

        RecipeDescription dreamPowderRecipe = new RecipeDescription(Recipe.DreamPowder,"Dream Powder");
        dreamPowderRecipe.Ingredients.Add(Ingredient.Topaz, 1);
        dreamPowderRecipe.Ingredients.Add(Ingredient.Sapphire, 1);
        dreamPowderRecipe.Description = "Allows puppies with insomnia to gently fall asleep.";

        RecipeDescription passionPotionRecipe = new RecipeDescription(Recipe.PassionPotion, "Passion Potion");
        passionPotionRecipe.Ingredients.Add(Ingredient.Ruby, 2);
        passionPotionRecipe.Description = "Gives a boost to energy so a puppy can play all day!";

        RecipeDescription quickElixerRecipe = new RecipeDescription(Recipe.QuickElixer, "Quick Elixer");
        quickElixerRecipe.Ingredients.Add(Ingredient.Ruby, 1);
        quickElixerRecipe.Ingredients.Add(Ingredient.Amber, 1);
        quickElixerRecipe.Ingredients.Add(Ingredient.Topaz, 1);
        quickElixerRecipe.Description = "Drinking this elixer allows the puppy to move super fast.";

        RecipeDescription fleaPoulticeRecipe = new RecipeDescription(Recipe.FleaPoultice, "Flea Poultice");
        fleaPoulticeRecipe.Ingredients.Add(Ingredient.Ruby, 1);
        fleaPoulticeRecipe.Ingredients.Add(Ingredient.Emerald, 1);
        fleaPoulticeRecipe.Description = "Applying this lotion removes a nasty case of the fleas.";
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
