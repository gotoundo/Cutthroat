using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ProgressPanel : MonoBehaviour {

    public static float popularityPercent(StoreBase storeToCheck)
    {
        Dictionary<StoreBase, float> StoreFavorabilities = new Dictionary<StoreBase, float>();
        float totalFavorability = 0f;

        foreach (StoreBase store in GameManager.AllStores)
        {
            StoreFavorabilities.Add(store, 0f);
            foreach (CustomerScript customer in GameManager.AllCustomers)
            {
                if (customer.StoreFavorability.ContainsKey(store))
                {
                    StoreFavorabilities[store] += customer.StoreFavorability[store];
                    totalFavorability += customer.StoreFavorability[store];
                }
            }
        }

        return StoreFavorabilities[storeToCheck] / totalFavorability;

    }

	public Slider mySlider;
    public Text storeName;
	public Text debugText;
    public StoreBase myStore;

	// Use this for initialization
	void Start () {
        storeName.text = myStore.Name;
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        DisplayFavorability();
    }

    private void DisplayFavorability()
    {
       /* Dictionary<StoreBase, float> StoreFavorabilities = new Dictionary<StoreBase, float>();
        float totalFavorability = 0f;

        foreach (StoreBase store in GameManager.AllStores)
        {
            StoreFavorabilities.Add(store, 0f);
            foreach (CustomerScript customer in GameManager.AllCustomers)
            {
                if (customer.StoreFavorability.ContainsKey(store))
                {
                    StoreFavorabilities[store] += customer.StoreFavorability[store];
                    totalFavorability += customer.StoreFavorability[store];
                }
            }
        }*/

        mySlider.value = popularityPercent(myStore);

        debugText.text = "" + Mathf.RoundToInt(100* mySlider.value) +"%";
    }
}
