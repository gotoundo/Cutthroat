using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum CustomerState {Shopping,Entering,Waiting,Leaving,HeadingHome,AtHome};

public class CustomerScript : MonoBehaviour {

    public CustomerState myState;
    public NavMeshAgent agent;
    public GameObject moveTarget;
    public GameObject home;
    public StoreBase targetedStore;
    public Recipe desiredProduct;

    Dictionary<StoreBase, float> StoreFavorability;
    Dictionary<StoreBase, float> StoreAwareness;
    List<StoreBase> EncounteredStores;

    const float baseWeight = 50;
    const float randomStoreChance = .10f;

    const float maxFavorability = 100f;
    const float minFavorability = -100f;
    const float maxAwareness = 100f;
    const float minAwareness = 0f;

    const float newStoreAwareness = 20f;
    const float newStoreFavorability = 0f;
    const float couldBuyFavorability = 10f;
    const float couldNotBuyFavorability = -5f;
    const float waitedForNothingFavorability = -10f;

    const int maxTrips = 3;
    const float maxWaitTime = 10f;
    const float sleepTime = 5f;

    const float interactionRange = 10f;
    const float scanRange = 3f;
    const float scanCoolMin = .5f;
    const float scanCoolMax = 1.5f;
    
    public float currentWaitTime = 0f;
    public int currentNumTrips = 0;
    public float scanCooldown = 0f;
    public float sleepTimeRemaining = 0f;

    static int count;
    static int myCount;
    List<string> debugStringList;
    public int maxDebugStringLength = 10;
    public string[] debugStringArray;
    public string[] debugStoreOptions;
    public int myPosInLine;

    void Start () {
        gameObject.name = "Customer " + home.GetComponent<HouseScript>().myCount+"-"+myCount;
        myCount = count;
        count++;

        StoreFavorability = new Dictionary<StoreBase, float>();
        StoreAwareness = new Dictionary<StoreBase, float>();
        EncounteredStores = new List<StoreBase>();
        debugStringList = new List<string>();
        myState = CustomerState.AtHome;

        agent = GetComponent<NavMeshAgent>();
        PickNewProduct();
    }

    // Update is called once per frame
    void Update()
    {
        switch (myState)
        {
            case CustomerState.Shopping:
                ScanForNewStores();
                if (targetedStore == null && GameObject.FindGameObjectsWithTag("Store").Length > 0)
                {
                    targetStore(PickNextStore().gameObject);
                    debugStringList.Add("A new day. I need " + desiredProduct.ToString() + ". I'll try " + targetedStore.gameObject.name + " next.");
                }

                if (Vector3.Distance(transform.position, targetedStore.transform.position) < interactionRange)
                    EnterStore();

                break;

            case CustomerState.Waiting:
                WaitInLine();
                break;

            case CustomerState.HeadingHome:
                if (Vector3.Distance(transform.position, moveTarget.transform.position) < interactionRange)
                {
                    debugStringList.Add("Home sweet home!");
                    sleepTimeRemaining = sleepTime;
                    myState = CustomerState.AtHome;
                }
                break;

            case CustomerState.AtHome:
                sleepTimeRemaining -= Time.deltaTime;
                if (sleepTimeRemaining <= 0)
                {
                    PickNewProduct();
                    targetStore(PickNextStore().gameObject);
                    debugStringList.Add("Morning already? I need " + desiredProduct.ToString() + ". I'll try " + targetedStore.gameObject.name + " next.");
                }
                break;

            default:
                break;
        }

        if (moveTarget != null)
            agent.SetDestination(moveTarget.transform.position);


        while (debugStringList.Count > maxDebugStringLength)
            debugStringList.RemoveAt(0);
        debugStringArray = debugStringList.ToArray();
    }

    StoreBase PickNextStore(StoreBase excludedStore = null) //
    {
        GameObject[] allstores = GameObject.FindGameObjectsWithTag("Store");
        if (StoreAwareness.Keys.Count <= 0 || Random.Range(0f, 1f) <= randomStoreChance)
            return allstores[Random.Range(0, allstores.Length)].GetComponent<StoreBase>();

        Dictionary<StoreBase, float> StoreWeights = new Dictionary<StoreBase, float>();
        float totalWeight = 0;
        foreach (StoreBase store in StoreAwareness.Keys)
        {
            //  if (excludedStore != null && (excludedStore.GetComponent<StoreBase>() != null && store != excludedStore.GetComponent<StoreBase>()))
            if (store != excludedStore)
            {
                float myWeight = Mathf.Max(1, baseWeight + StoreFavorability[store] + StoreAwareness[store]);
                StoreWeights.Add(store, myWeight);
                totalWeight += myWeight;
            }

        }

        List<string> debugList = new List<string>();
        foreach (StoreBase store in StoreWeights.Keys)
            debugList.Add(store.gameObject.name + " " + (int)(100 * StoreWeights[store] / totalWeight) + "%  (" + (int)StoreWeights[store] + "/" + (int)totalWeight + ")");
        debugStoreOptions = debugList.ToArray();

        float rolledWeight = Random.Range(0, totalWeight);

        foreach (StoreBase store in StoreWeights.Keys)
        {
            rolledWeight -= StoreWeights[store];
            if (rolledWeight <= 0)
                return store;

        }

        //if found nothing just pick one at random
        StoreBase totallyRandomStore = null;
        while (totallyRandomStore == null || totallyRandomStore == excludedStore)
            totallyRandomStore = allstores[Random.Range(0, allstores.Length)].GetComponent<StoreBase>();
        return totallyRandomStore;
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
        if (!StoreAwareness.ContainsKey(store))
            StoreAwareness.Add(store, newStoreAwareness);
        if (!StoreFavorability.ContainsKey(store))
            StoreFavorability.Add(store, newStoreFavorability);
    }

    private void targetStore(GameObject store)
    {
        moveTarget = store;
        targetedStore = store.GetComponent<StoreBase>();
        myState = CustomerState.Shopping;
        

    }

    private void targetHome()
    {
        moveTarget = home;
        myState = CustomerState.HeadingHome;
    }

    private void ScanForNewStores()
    {
        GameObject[] initalStores = GameObject.FindGameObjectsWithTag("Store");
        List<GameObject> stores = new List<GameObject>();
        foreach(GameObject o in initalStores)
        {
            if (o.GetComponent<StoreBase>() != null)
                stores.Add(o);
            else
                Debug.Log(o.name + " has no attached store script???");
        }


        for(int i = 0; i< stores.Count;i++)
        {
            if (Vector3.Distance(gameObject.transform.position, stores[i].transform.position) <= scanRange)
            {
                StoreBase store = stores[i].GetComponent<StoreBase>();

                if (!EncounteredStores.Contains(store))
                {
                    EncounteredStores.Add(store);
                    AddAwareness(store, store.PassbyAwarenessBonus());
                }
            }
        }
    }

    private void EnterStore()
    {
        AddFavorability(targetedStore, targetedStore.WalkInFavorabilityBonus());

        debugStringList.Add("Going inside " + targetedStore.gameObject.name + ".");
        

        if (targetedStore.CanMakeProduct(desiredProduct))
        {
            debugStringList.Add(targetedStore.gameObject.name + " sells " + desiredProduct.ToString() + "! I'm getting in line.");
            currentWaitTime = 0;
            myState = CustomerState.Waiting;
        }
        else
        {
            debugStringList.Add(targetedStore.gameObject.name + " doesn't have any "+desiredProduct.ToString()+".");
            LeaveStore();
            TryAnotherStore();
         }

    }

    private void WaitInLine()
    {
        currentWaitTime += Time.deltaTime;

        if(!targetedStore.CustomerQueue.Contains(this))
            targetedStore.CustomerQueue.Add(this);

        myPosInLine = targetedStore.CustomerQueue.IndexOf(this);
        if (myPosInLine <= 0)
            moveTarget = targetedStore.gameObject;
        else
            moveTarget = targetedStore.CustomerQueue[myPosInLine - 1].gameObject;

        if (currentWaitTime > maxWaitTime)
        {
            debugStringList.Add("The wait at "+ targetedStore.gameObject.name + " is too long!");
            LeaveStore();
            TryAnotherStore();
        }
    }

    public void LeaveStore()
    {
        currentWaitTime = 0;
        targetedStore.CustomerQueue.Remove(this);
        debugStringList.Add("I'm leaving " + targetedStore.gameObject.name + ".");
        targetedStore = null;
    }
    
    public void AttemptTransaction()
    {
        if (targetedStore.TryBuyProduct(desiredProduct)) //returns success boolean
        {
            
            AddFavorability(targetedStore, couldBuyFavorability);
            debugStringList.Add("I was able to buy " + desiredProduct.ToString() + " from " + targetedStore.gameObject.name + "! Time to go home.");
            LeaveStore();
            targetHome();
        }
        else
        {
            AddFavorability(targetedStore, waitedForNothingFavorability);
            debugStringList.Add(targetedStore.gameObject.name + " ran out of " + desiredProduct.ToString() + "! ");
            LeaveStore();
            TryAnotherStore();
        }
    }

    private void TryAnotherStore()
    {
        GameObject oldTarget = null;
        if (targetedStore != null)
            oldTarget = targetedStore.gameObject;

        if (oldTarget != null && oldTarget.GetComponent<StoreBase>() != null)
            targetStore(PickNextStore(oldTarget.GetComponent<StoreBase>()).gameObject);
        else
            targetStore(PickNextStore().gameObject);

        currentNumTrips++;



        if (currentNumTrips >= maxTrips)
        {
            targetHome();
            currentNumTrips = 0;
            debugStringList.Add("I'm just giving up and going home.");
        }
        else
            debugStringList.Add("I'll try " + moveTarget.gameObject.name + " next. I'll try a max of " + (maxTrips - currentNumTrips) + " more stores.");
    }

    void PickNewProduct()
    {
        List<Recipe> allRecipes = new List<Recipe>();
        allRecipes.Add(Recipe.DreamPowder);
        allRecipes.Add(Recipe.PassionPotion);

        desiredProduct = allRecipes[Random.Range(0, allRecipes.Count)];
    }
}
