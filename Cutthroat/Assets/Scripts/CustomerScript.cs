using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum CustomerState {Shopping,Entering,Waiting,Leaving,HeadingHome,AtHome};

public class CustomerScript : MonoBehaviour {


    CustomerState myState;
    NavMeshAgent agent;
    Animator animator;
    GameObject moveTarget;
    public GameObject home;
    StoreBase targetedStore;
    Recipe desiredProduct;

    public Dictionary<StoreBase, float> StoreFavorability;
    public Dictionary<StoreBase, float> StoreAwareness;
    List<StoreBase> EncounteredStores;
    List<StoreBase> StoresVisitedToday;

    float defaultMoveSpeed = 3.5f;

    const float baseStoreWeight = 50;
    const float randomStoreChance = .10f;
    public const float maxFavorability = 100f;
    const float minFavorability = 0;
    public const float maxAwareness = 100f;
    const float minAwareness = 0f;
    const float AwarenessDecayPerSecond = .5f;
    const float FavorabilityDecayPerSecond = .2f;

    const float baseStoreAwareness = 20f;
    const float baseStoreFavorability = 0f;
    const float couldBuyFavorability = 10f;
    const float couldNotBuyFavorability = -5f;
    const float waitedForNothingFavorability = -10f;
    

    const int maxTrips = 3;
    const float maxWaitTime = 10f;
    const float sleepTime = 5f;

    float interactionRange = 10f;
    const float scanRange = 3f;
    const float scanCoolMin = .5f;
    const float scanCoolMax = 1.5f;
    
    public float currentWaitTime = 0f;
    public int currentNumTrips = 0;
    public float scanCooldown = 0f;
    public float sleepTimeRemaining = 0f;

    public float priceSensitivity = .35f;
    /* priceSensitivity is multiplied  against the margin. For example, a recipe has a base cost of 50, and is sold for 200.
       the recipe has a margin of 300%. If the price sensitivity is .2, then 300% * .2 = 60% chance to say too pricy!
    */  

    static int count;
    static int myCount;
    List<string> debugStringList;
    public int maxDebugStringLength = 7;
    public string[] debugStringArray;
    public string[] debugStoreOptions;
    public int myPosInLine;

    float playTime = 0;

    void Start()
    {
        GameManager.AllCustomers.Add(this);
        gameObject.name = "Customer " + home.GetComponent<HouseScript>().myCount + "-" + myCount;
        myCount = count;
        count++;

        StoreFavorability = new Dictionary<StoreBase, float>();
        StoreAwareness = new Dictionary<StoreBase, float>();
        EncounteredStores = new List<StoreBase>();
        StoresVisitedToday = new List<StoreBase>();
        debugStringList = new List<string>();
        myState = CustomerState.AtHome;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        PickNewProduct();

        foreach (StoreBase store in GameManager.AllStores)
            AddFavorability(store, store.startingFavorability);
    }

    // Update is called once per frame
    void Update()
    {
        RunStateLogic();

        if (moveTarget != null)
            agent.SetDestination(moveTarget.transform.position);

        foreach (StoreBase store in new List<StoreBase>(StoreAwareness.Keys))
            AddAwareness(store, -AwarenessDecayPerSecond * Time.deltaTime);
        foreach (StoreBase store in new List<StoreBase>(StoreFavorability.Keys))
            AddFavorability(store, -FavorabilityDecayPerSecond * Time.deltaTime);

        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
        animator.SetFloat("PlayTime", playTime);
        playTime = Mathf.Max(0, playTime -= Time.deltaTime);
        agent.speed = playTime > 0 ? 0 : defaultMoveSpeed;
        
        while (debugStringList.Count > maxDebugStringLength)
            debugStringList.RemoveAt(0);
        debugStringArray = debugStringList.ToArray();
    }

    private void RunStateLogic()
    {
        switch (myState)
        {
            case CustomerState.Shopping:
                agent.stoppingDistance = 9f;
                interactionRange = 10f;
                ScanForNewStores();
                if (targetedStore == null && GameManager.AllStores.Count > 0)
                {
                    targetStore(PickNextStore().gameObject);
                    debugStringList.Add("A new day. I need " + desiredProduct.ToString() + ". I'll try " + targetedStore.gameObject.name + " next.");
                }

                if (Vector3.Distance(transform.position, targetedStore.transform.position) < interactionRange)
                    EnterStore();

                break;

            case CustomerState.Waiting:
                agent.stoppingDistance = 3f;
                WaitInLine();
                break;

            case CustomerState.HeadingHome:
                agent.stoppingDistance = 1f;
                interactionRange = 2f;
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
                    StoresVisitedToday = new List<StoreBase>();
                    PickNewProduct();
                    targetStore(PickNextStore().gameObject);
                    debugStringList.Add("Morning already? I need " + desiredProduct.ToString() + ". I'll try " + targetedStore.gameObject.name + " next.");
                }
                break;

            default:
                break;
        }
    }

    private StoreBase PickNextStore(List<StoreBase> excludedStores = null) //
    {
        if (excludedStores == null)
            excludedStores = new List<StoreBase>();

        if (StoreAwareness.Keys.Count <= 0 || Random.Range(0f, 1f) <= randomStoreChance)
            return GameManager.AllStores[Random.Range(0, GameManager.AllStores.Count)];//.GetComponent<StoreBase>();

        Dictionary<StoreBase, float> StoreWeights = new Dictionary<StoreBase, float>();
        float totalWeight = 0;
        foreach (StoreBase store in StoreAwareness.Keys)
        {
            if (!excludedStores.Contains(store))
            {
                float myWeight = Mathf.Max(1, baseStoreWeight + StoreFavorability[store] + StoreAwareness[store]);
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
        while (totallyRandomStore == null || excludedStores.Contains(totallyRandomStore))
            totallyRandomStore = GameManager.AllStores[Random.Range(0, GameManager.AllStores.Count)].GetComponent<StoreBase>();
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

    private void StoreCheck(StoreBase store)
    {
        if (!StoreAwareness.ContainsKey(store))
            StoreAwareness.Add(store, baseStoreAwareness);
        if (!StoreFavorability.ContainsKey(store))
            StoreFavorability.Add(store, baseStoreFavorability);
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
        for(int i = 0; i< GameManager.AllStores.Count;i++)
        {
            if (Vector3.Distance(gameObject.transform.position, GameManager.AllStores[i].gameObject.transform.position) <= scanRange)
            {
                StoreBase store = GameManager.AllStores[i];

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
        StoresVisitedToday.Add(targetedStore);
        AddFavorability(targetedStore, targetedStore.WalkInFavorabilityBonus());

        debugStringList.Add("Going inside " + targetedStore.gameObject.name + ".");
        
        if (targetedStore.CanMakeProduct(desiredProduct))
        {
            float failureChance = Mathf.Max(0, ((targetedStore.ProductCost(desiredProduct) / IngredientStore.AverageRecipeCost(desiredProduct)) - 1) * priceSensitivity);
            
            if (Random.Range(0f,1f) > failureChance)
            {//Success!
                debugStringList.Add(targetedStore.gameObject.name + " sells " + desiredProduct.ToString() + "! I'm getting in line.");
                currentWaitTime = 0;
                myState = CustomerState.Waiting;
            }
            else
            {
                debugStringList.Add(targetedStore.gameObject.name + " is charging too much for " + desiredProduct.ToString() + "!");
                GetComponentInParent<OverheadIconManager>().ShowIcon(TextureManager.singleton.OverheadIcons[4], 1.5f);
                LeaveStore();
                TryAnotherStore();
            }
        }
        else
        {
            debugStringList.Add(targetedStore.gameObject.name + " doesn't have any "+desiredProduct.ToString()+".");
            GetComponentInParent<OverheadIconManager>().ShowIcon(TextureManager.singleton.OverheadIcons[3], 1.5f);
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
            GetComponentInParent<OverheadIconManager>().ShowIcon(TextureManager.singleton.OverheadIcons[2], 1.5f);
            LeaveStore();
            TryAnotherStore();
        }
    }

    private void LeaveStore()
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
            GetComponentInParent<OverheadIconManager>().ShowIcon(TextureManager.singleton.OverheadIcons[0], 1.5f);
            playTime = 2.6f;
            LeaveStore();
            targetHome();
        }
        else
        {
            AddFavorability(targetedStore, waitedForNothingFavorability);
            debugStringList.Add(targetedStore.gameObject.name + " ran out of " + desiredProduct.ToString() + "! ");
            GetComponentInParent<OverheadIconManager>().ShowIcon(TextureManager.singleton.OverheadIcons[3], 1.5f);
            LeaveStore();
            TryAnotherStore();
        }
    }

    private void TryAnotherStore()
    {
        currentNumTrips++;
        if (StoresVisitedToday.Count < StoreAwareness.Count)
        {
            targetStore(PickNextStore(StoresVisitedToday).gameObject);
        }
        else
            currentNumTrips = maxTrips;



        if (currentNumTrips >= maxTrips)
        {
            targetHome();
            currentNumTrips = 0;
            debugStringList.Add("I'm just giving up and going home.");
        }
        else
            debugStringList.Add("I'll try " + moveTarget.gameObject.name + " next. I'll try a max of " + (maxTrips - currentNumTrips) + " more stores.");
    }

    private void PickNewProduct()
    {
        List<Recipe> allRecipes = new List<Recipe>();
        allRecipes.Add(Recipe.DreamPowder);
        allRecipes.Add(Recipe.PassionPotion);

        desiredProduct = allRecipes[Random.Range(0, allRecipes.Count)];
    }
}
