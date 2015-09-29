using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIPurchaseOption : MonoBehaviour {

    public Ingredient ItemSold;
    public Text ItemName;
    public Text ItemCost;
    public Button purchaseButton;
    StoreBase player;
    // Use this for initialization
    void Start () {
        ItemName.text = ItemSold.ToString();
        

    }
	
	// Update is called once per frame
	void Update () {
        if(player == null)
            player = GameManager.singleton.player;

        ItemCost.text = "" + IngredientStore.IngredientPrices[ItemSold] + " Gold";
        purchaseButton.interactable = player.Gold >= IngredientStore.IngredientPrices[ItemSold];

    }

    public void AttemptPurchase()
    {
        Dictionary<Ingredient, int> playerIngredients = player.GetIngredients();
        if (player.Gold >= IngredientStore.IngredientPrices[ItemSold])
        {
            player.Gold -= IngredientStore.IngredientPrices[ItemSold];
            playerIngredients[ItemSold]++;
        }
        
    }
}
