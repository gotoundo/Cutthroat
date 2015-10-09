using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIPurchaseOption : MonoBehaviour {

    public Ingredient ItemSold;
    public Text ItemName;
    public Text ItemCost;
    public Button[] purchaseButton;
    public Image gemIcon;
    StoreBase player;

    // Use this for initialization
    void Start () {
        
        gemIcon.overrideSprite = TextureManager.IngredientTextures[ItemSold];
    }
	
	// Update is called once per frame
	void Update () {
        if(player == null)
            player = GameManager.singleton.player;

        ItemName.text = ItemSold.ToString() + " (x"+player.GetIngredients()[ItemSold]+")";

        for (int i = 0; i < purchaseButton.Length; i++)
        {
            int quantity = purchaseButton[i].GetComponent<ButtonQuantity>().Quantity;
            purchaseButton[i].interactable = player.CanPurchaseIngredient(IngredientStore.CurrentIngredientPrices[ItemSold], quantity);
            purchaseButton[i].GetComponentInChildren<Text>().text = "(x" + quantity + ") " + IngredientStore.CurrentIngredientPrices[ItemSold] * quantity + " Gold";
        }
        
    }

    public void AttemptPurchase(Button b)
    {
        player.TryBuyIngredients(ItemSold, b.GetComponent<ButtonQuantity>().Quantity);
    }
}
