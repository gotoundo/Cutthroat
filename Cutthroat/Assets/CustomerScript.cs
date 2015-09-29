using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CustomerScript : MonoBehaviour {

    public NavMeshAgent agent;
    public GameObject target;
    public GameObject home;

    public bool shopping = true;
    public Recipe desiredProduct;

    Dictionary<StoreBase, float> StoreFavorability;
    Dictionary<StoreBase, float> StoreAwareness;
    const float baseWeight = 50;
    const float randomStoreChance = .10f;


    const float maxFavorability = 100f;
    const float minFavorability = -100f;
    const float maxAwareness = 100f;
    const float minAwareness = 0f;

    const float newStoreAwareness = 20f;
    const float couldBuyFavorability = 10f;
    const float couldNotBuyFavorability = -10f;

    const int maxTrips = 3;

    public int currentNumTrips = 0;

    public string debugString;
    public string[] debugStoreOptions;
    //public Custom
	// Use this for initialization
    static int count;
    static int myCount;
	void Start () {
        gameObject.name = "Customer " + home.GetComponent<HouseScript>().myCount+"-"+myCount;
        myCount = count;
        count++;

        StoreFavorability = new Dictionary<StoreBase, float>();
        StoreAwareness = new Dictionary<StoreBase, float>();

        agent = GetComponent<NavMeshAgent>();
        PickNewProduct();
    }

     StoreBase PickNextStore() //GameObject excludedStore = null
    {
        if (StoreAwareness.Keys.Count <= 0 || Random.Range(0f, 1f) <= randomStoreChance)
        {
            GameObject[] allstores = GameObject.FindGameObjectsWithTag("Store");
            return allstores[Random.Range(0, allstores.Length)].GetComponent<StoreBase>();
        }

        Dictionary<StoreBase, float> StoreWeights = new Dictionary<StoreBase, float>();
        float totalWeight = 0;
        foreach (StoreBase store in StoreAwareness.Keys)
        {
          //  if (excludedStore != null && (excludedStore.GetComponent<StoreBase>() != null && store != excludedStore.GetComponent<StoreBase>()))
            
                float myWeight = Mathf.Max(1, baseWeight + StoreFavorability[store] + StoreAwareness[store]);
                StoreWeights.Add(store, myWeight);
                totalWeight += myWeight;
            
        }

        List<string> debugList = new List<string>();
        foreach (StoreBase store in StoreWeights.Keys)
            debugList.Add(store.gameObject.name + " " + (int)(100 * StoreWeights[store] / totalWeight) + "%  (" + (int)StoreWeights[store] + "/" + (int)totalWeight + ")");
        debugStoreOptions = debugList.ToArray();

        float rolledWeight = Random.Range(0, totalWeight);

        foreach(StoreBase store in StoreWeights.Keys)
        {
            rolledWeight -= StoreWeights[store];
            if (rolledWeight <= 0)
                return store;
                
        }

        return null;
    }

    public void AddAwareness(StoreBase store, float amount)
    {
        StoreCheck(store);
        StoreAwareness[store] += amount;
        StoreAwareness[store] = Mathf.Min(maxAwareness, Mathf.Max(StoreAwareness[store], minAwareness));
    }

    public void AddFavorability(StoreBase store, float amount)
    {
        StoreCheck(store);
        StoreFavorability[store] += amount;
        StoreFavorability[store] = Mathf.Min(maxFavorability, Mathf.Max(StoreFavorability[store], minFavorability));
    }

    void StoreCheck(StoreBase store)
    {
        if (!StoreFavorability.ContainsKey(store))
            StoreFavorability.Add(store, 0);
        if (!StoreAwareness.ContainsKey(store))
            StoreAwareness.Add(store, 0);
    }


    // Update is called once per frame
    void Update () {

        if (shopping)
        {
            GameObject[] stores = GameObject.FindGameObjectsWithTag("Store");
            if (target == null && stores.Length > 0)
            {
                StoreBase newStore = PickNextStore();
                target = newStore.gameObject;

                debugString = "A new day. I need " + desiredProduct.ToString() + ". I'll try " + target.gameObject.name + " next.";

                if (!StoreAwareness.ContainsKey(newStore))
                    AddAwareness(newStore, newStoreAwareness);
            }

            if (Vector3.Distance(transform.position, target.transform.position) < 1)
            {//attempt purchase
                StoreBase store = target.GetComponent<StoreBase>();
                if (store.TryBuyProduct(desiredProduct)) //returns success boolean
                {
                    shopping = false;
                    AddFavorability(store, couldBuyFavorability);
                    debugString = "I was able to buy " + desiredProduct.ToString() + " from " + target.gameObject.name + "! Time to go home.";
                }
                else //try somewhere else
                {
                    AddFavorability(store, couldNotBuyFavorability);
                    GameObject oldTarget = target;
                    target = PickNextStore().gameObject;
                    if(target!=oldTarget)
                        currentNumTrips++;

                    debugString = oldTarget.gameObject.name + " didn't have " + desiredProduct.ToString() + ". ";
                    if (currentNumTrips >= maxTrips)
                        debugString += "I'm giving up and going home.";
                    else
                        debugString += "I'll try " + target.gameObject.name + " next. I'll try a max of " + (maxTrips - currentNumTrips) + " more stores.";
                   // Debug.Log(debugString);

                    if (currentNumTrips >= maxTrips)
                    {
                        shopping = false;
                        currentNumTrips = 0;
                    }

                }

            }
        }
        
        if(!shopping)
        {
            target = home;
            if (Vector3.Distance(transform.position, target.transform.position) < 1)
            {
                PickNewProduct();
                shopping = true;
                target = null;
            }
        }


        if(target!=null)
        agent.SetDestination(target.transform.position);

        //  agent.


    }

    void PickNewProduct()
    {
        List<Recipe> allRecipes = new List<Recipe>();
        allRecipes.Add(Recipe.DreamPowder);
        allRecipes.Add(Recipe.PassionPotion);

        desiredProduct = allRecipes[Random.Range(0, allRecipes.Count)];

        //desiredProduct = enum Recipe
    }
}
