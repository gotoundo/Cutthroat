using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public enum Ingredient { Ruby, Sapphire, Emerald, Topaz }
public class IngredientStore : MonoBehaviour {
    
    public static Dictionary<Ingredient, int> IngredientPrices;


    // Use this for initialization
    void Start () {
        IngredientPrices = new Dictionary<Ingredient, int>();
        IngredientPrices.Add(Ingredient.Ruby, 5);
        IngredientPrices.Add(Ingredient.Sapphire, 3);
        IngredientPrices.Add(Ingredient.Emerald, 2);
        IngredientPrices.Add(Ingredient.Topaz, 1);

    }
	
	// Update is called once per frame
	void Update () {
        
                
	
	}
}
