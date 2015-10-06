using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MarketingUI : MonoBehaviour {
    public Button BuyButton;
    public Slider PriceSlider;
    public Text PriceText;
    

    
    

    int currentCost = 0;

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        currentCost = Mathf.RoundToInt(PriceSlider.value*StoreBase.MaxMarketingCost);

        PriceText.text = ""+ currentCost + " Gold";
        BuyButton.interactable = currentCost <= GameManager.singleton.player.Gold;
	}

    public void BuyMarketing()
    {
        GameManager.singleton.player.BuyMarketing(currentCost);
        PriceSlider.value = 0f;
    }
}
