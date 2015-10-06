using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ForecastUI : MonoBehaviour {
    public Text DebugText; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        DebugText.text = "";
        foreach (KeyValuePair<Ingredient, int> entry in IngredientStore.NextIngredientPrices)
        {
            DebugText.text += entry.Key.ToString() + " will cost " + entry.Value +"\n";
        }
       // DebugText.text

    }
}
