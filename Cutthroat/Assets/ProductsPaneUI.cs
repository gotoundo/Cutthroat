using UnityEngine;
using System.Collections;

public class ProductsPaneUI : MonoBehaviour {
    public GameObject RowTemplate;
	// Use this for initialization
	void Start () {
        foreach(Recipe recipe in GameManager.singleton.CurrentLevel.RecipesUsed)
        {
            GameObject recipeRow = (GameObject)Instantiate(RowTemplate);
            recipeRow.transform.SetParent(transform);
            recipeRow.GetComponent<RecipeUI>().myRecipe = recipe;
        }
       /* RowTemplate.SetActive(false);
        Destroy(RowTemplate);*/
	}
	
	// Update is called once per frame
	void Update () {
        //
    }
}
