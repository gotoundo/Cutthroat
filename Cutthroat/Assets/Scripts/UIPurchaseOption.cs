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
    public Text MarketIndicator;
    StoreBase player;

    // Use this for initialization
    void Start () {
        //transform.localScale = new Vector3(1, 1, 1); //no idea why this is necissary but on the mac it sets it to ~2,2,2 for some reason
        gemIcon.overrideSprite = TextureManager.IngredientTextures[ItemSold];
        MarketIndicator.gameObject.SetActive(false);
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

        if (GameManager.singleton.gameRunning)
        {
            if (IngredientStore.CurrentIngredientPrices[ItemSold] > IngredientStore.DefaultIngredientPrices[ItemSold] * 1.25)
            {
                MarketIndicator.gameObject.SetActive(true);
                MarketIndicator.color = Color.red;
                MarketIndicator.text = "Yikes!";
            }
            else if (IngredientStore.CurrentIngredientPrices[ItemSold] < IngredientStore.DefaultIngredientPrices[ItemSold] * .75f)
            {
                MarketIndicator.gameObject.SetActive(true);
                MarketIndicator.color = Color.green;
                MarketIndicator.text = "Good Deal!";
            }
            else
            {
                MarketIndicator.gameObject.SetActive(false);
                MarketIndicator.color = Color.grey;
                MarketIndicator.text = "Reasonable";
            }
        }

    }

    public void AttemptPurchase(Button b)
    {
        player.TryBuyIngredients(ItemSold, b.GetComponent<ButtonQuantity>().Quantity);
    }
}
