using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public class PlayerStatsWindowUI : MonoBehaviour
{
    public Text GoldCount;
    public GameObject IngredientsTray;
    public GameObject IngredientCountTemplate;
    List<IngredientUI> IngredientCounts;
    bool hasBeenSetUp = false;



    // Use this for initialization
    void Start()
    {
        IngredientCounts = new List<IngredientUI>();
    }

    // Update is called once per frame
    void Update()
    {
        TrySetup();
        if (hasBeenSetUp)
            UpdateStatsWindow();
    }
    
    void TrySetup()
    {
        if (!hasBeenSetUp && GameManager.Main.player!=null)
        {
            hasBeenSetUp = true;
            foreach (Ingredient ingr in GameManager.Main.player.GetIngredients().Keys)
            {
                GameObject ingredientCount = Instantiate(IngredientCountTemplate);
                ingredientCount.transform.SetParent(IngredientsTray.transform);
                IngredientUI ingredientDisplay = ingredientCount.GetComponent<IngredientUI>();
                IngredientCounts.Add(ingredientDisplay);
                ingredientDisplay.trackPlayerQuantities = true;
                ingredientDisplay.ingredient = ingr;
                
            }
        }
    }

    void UpdateStatsWindow()
    {
        GoldCount.text = "" + GameManager.Main.playerStore.GetComponent<StoreBase>().Gold + " Gold";

        
    }
}

