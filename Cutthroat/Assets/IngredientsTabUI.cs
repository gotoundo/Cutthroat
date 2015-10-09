using UnityEngine;
using System.Collections;

public class IngredientsTabUI : MonoBehaviour {
    public GameObject IngredientTemplate;
	// Use this for initialization
	void Start () {
        foreach(Ingredient ingr in IngredientStore.CurrentIngredientPrices.Keys)
        {
            GameObject o = (GameObject)Instantiate(IngredientTemplate);
            o.transform.SetParent(transform);
            o.GetComponent<UIPurchaseOption>().ItemSold = ingr;
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
