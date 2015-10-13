using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MarketCountdownUI : MonoBehaviour {
    public string Prefix;
  //  public IngredientStore ingredientStore;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Text>().text = Prefix + (int)IngredientStore.Main.remainingCooldown;

    }
}
