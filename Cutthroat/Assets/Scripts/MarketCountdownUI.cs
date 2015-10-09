using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MarketCountdownUI : MonoBehaviour {

  //  public IngredientStore ingredientStore;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Text>().text = "Market Refresh In: \n" + (int)IngredientStore.Main.remainingCooldown;

    }
}
