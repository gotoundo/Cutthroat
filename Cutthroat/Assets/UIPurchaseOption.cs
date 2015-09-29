using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIPurchaseOption : MonoBehaviour {

    public Ingredient ItemSold;
    public Text ItemName;
    public Text ItemCost;
    public Button[] purchaseButton;
    StoreBase player;

    // Use this for initialization
    void Start () {
        ItemName.text = ItemSold.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        if(player == null)
            player = GameManager.singleton.player;
        
        for (int i = 0; i < purchaseButton.Length; i++)
        {
            int quantity = purchaseButton[i].GetComponent<ButtonQuantity>().Quantity;
            purchaseButton[i].interactable = player.CanPurchaseIngredient(IngredientStore.CurrentIngredientPrices[ItemSold], quantity);
            purchaseButton[i].GetComponentInChildren<Text>().text = "(x" + quantity + ") " + IngredientStore.CurrentIngredientPrices[ItemSold] * quantity + " Gold";
        }
        
    }

    public void AttemptPurchase(Button b)
    {
        player.TryBuyIngredients(ItemSold, IngredientStore.CurrentIngredientPrices[ItemSold], b.GetComponent<ButtonQuantity>().Quantity);
    }
}
