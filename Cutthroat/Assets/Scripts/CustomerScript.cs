using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum CustomerState {Shopping,Entering,Waiting,Leaving,HeadingHome,AtHome};

public class CustomerScript : MonoBehaviour {

    CustomerState myState;
    NavMeshAgent agent;
    Animator animator;
    GameObject moveTarget;
    GameObject home;
    StoreBase targetedStore;
    Recipe desiredProduct;
    Inspectable inspectorData;

    public Dictionary<StoreBase, float> StoreFavorability;
    public Dictionary<StoreBase, float> StoreAwareness;
    List<StoreBase> StoresSeenToday;
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
    const float sleepTime = 8f;

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

    
    public int myPosInLine;

    float playTime = 0;

    void Start()
    {
        GameManager.AllCustomers.Add(this);
        inspectorData = GetComponent<Inspectable>();
        gameObject.name = PuppyNames.AllNames[Random.Range(0, PuppyNames.AllNames.Length)];
        inspectorData.Name = gameObject.name;

        StoreFavorability = new Dictionary<StoreBase, float>();
        StoreAwareness = new Dictionary<StoreBase, float>();
        StoresVisitedToday = new List<StoreBase>();
        StoresSeenToday = new List<StoreBase>();
        
     //   debugStringList = new List<string>();
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
        
        
       // debugStringArray = debugStringList.ToArray();
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
                    inspectorData.AddUpdate("A new day. I need " + desiredProduct.ToString() + ". I'll try " + targetedStore.gameObject.name + " next.");
                }

                if (Vector3.Distance(transform.position, targetedStore.transform.position) < interactionRange)
                    EnterStore();

                break;

            case CustomerState.Waiting:
                agent.stoppingDistance = 3f;
                WaitInLine();
                break;

            case CustomerState.HeadingHome:
                agent.stoppingDistance = 2.5f;
                interactionRange = 6f;
                if (Vector3.Distance(transform.position, moveTarget.transform.position) < interactionRange)
                {

                    inspectorData.AddUpdate("Home sweet home!");
                    sleepTimeRemaining = sleepTime;
                    myState = CustomerState.AtHome;
                }
                break;

            case CustomerState.AtHome:
                agent.stoppingDistance = 50f;
                sleepTimeRemaining -= Time.deltaTime;
                if (sleepTimeRemaining > 0)
                {
                    animator.SetBool("Sleeping", true);
                    agent.speed = 0;
                }
                if (sleepTimeRemaining < 0)
                {
                    if(sleepTimeRemaining <= -1)
                    {
                        agent.speed = defaultMoveSpeed;
                        
                    }
                    animator.SetBool("Sleeping", false);
                    StoresVisitedToday = new List<StoreBase>();
                    StoresSeenToday = new List<StoreBase>();
                    PickNewProduct();
                    targetStore(PickNextStore().gameObject);
                    inspectorData.AddUpdate("Morning already? I need " + desiredProduct.ToString() + ". I'll try " + targetedStore.gameObject.name + " next.");
                }
                break;

            default:
                break;
        }
    }

    private StoreBase PickNextStore()
    {
        List<StoreBase> excludedStores = StoresVisitedToday != null ? StoresVisitedToday : new List<StoreBase>();

        if (StoreAwareness.Keys.Count == 0 || Random.Range(0f, 1f) <= randomStoreChance || GameManager.AllStores.Count <= excludedStores.Count)
            return GameManager.AllStores[Random.Range(0, GameManager.AllStores.Count)];

        WeightedCollection<StoreBase> StoreWeights = new WeightedCollection<StoreBase>();
        foreach (StoreBase store in GameManager.AllStores)
        {
            if (!excludedStores.Contains(store))
            {
                if (StoreAwareness.ContainsKey(store))
                    StoreWeights.AddWeight(store, StoreAwareness[store]);
                if (StoreFavorability.ContainsKey(store))
                    StoreWeights.AddWeight(store, StoreFavorability[store]);
            }
        }

        return StoreWeights.RollRandomItem();
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

    public void SetHome(GameObject homeObject)
    {
        home = homeObject;
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
                if (!StoresSeenToday.Contains(store))
                {
                    StoresSeenToday.Add(store);
                    AddAwareness(store, store.PassbyAwarenessBonus());
                    if (myState == CustomerState.Shopping && targetedStore != store)
                        targetStore(PickNextStore().gameObject);
                }
            }
        }
    }

    private void EnterStore()
    {
        StoresVisitedToday.Add(targetedStore);
        AddFavorability(targetedStore, targetedStore.WalkInFavorabilityBonus());

       // inspectorData.AddUpdate("Going inside " + targetedStore.gameObject.name + ".");
        
        if (targetedStore.CanMakeProduct(desiredProduct))
        {
            float failureChance = Mathf.Max(0, ((targetedStore.ProductCost(desiredProduct) / IngredientStore.AverageRecipeCost(desiredProduct)) - 1) * priceSensitivity);
            
            if (Random.Range(0f,1f) > failureChance)
            {//Success!
                inspectorData.AddUpdate(targetedStore.gameObject.name + " sells " + desiredProduct.ToString() + "! I'm getting in line.");
                currentWaitTime = 0;
                myState = CustomerState.Waiting;
            }
            else
            {
                inspectorData.AddUpdate(targetedStore.gameObject.name + " is charging too much for " + desiredProduct.ToString() + "!");
                GetComponentInParent<OverheadIconManager>().ShowIcon(TextureManager.Main.OverheadIcons[4], 1.5f);
                LeaveStore();
                TryAnotherStore();
            }
        }
        else
        {
            inspectorData.AddUpdate(targetedStore.gameObject.name + " doesn't have any "+desiredProduct.ToString()+".");
            GetComponentInParent<OverheadIconManager>().ShowIcon(TextureManager.Main.OverheadIcons[3], 1.5f);
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
            inspectorData.AddUpdate("The wait at "+ targetedStore.gameObject.name + " is too long!");
            GetComponentInParent<OverheadIconManager>().ShowIcon(TextureManager.Main.OverheadIcons[2], 1.5f);
            LeaveStore();
            TryAnotherStore();
        }
    }

    private void LeaveStore()
    {
        currentWaitTime = 0;
        targetedStore.CustomerQueue.Remove(this);
        //inspectorData.AddUpdate("I'm leaving " + targetedStore.gameObject.name + ".");
        targetedStore = null;
    }

    public void AttemptTransaction()
    {
        if (targetedStore.TryBuyProduct(desiredProduct)) //returns success boolean
        {
            
            AddFavorability(targetedStore, couldBuyFavorability);
            inspectorData.AddUpdate("I was able to buy " + desiredProduct.ToString() + " from " + targetedStore.gameObject.name + "! Time to go home.");
            GetComponentInParent<OverheadIconManager>().ShowIcon(TextureManager.Main.PotionIcons[GameManager.RecipeBook[desiredProduct].SpriteID], 1.5f);
            playTime = 2.6f;
            LeaveStore();
            targetHome();
        }
        else
        {
            AddFavorability(targetedStore, waitedForNothingFavorability);
            inspectorData.AddUpdate(targetedStore.gameObject.name + " ran out of " + desiredProduct.ToString() + "! ");
            GetComponentInParent<OverheadIconManager>().ShowIcon(TextureManager.Main.OverheadIcons[3], 1.5f);
            LeaveStore();
            TryAnotherStore();
        }
    }

    private void TryAnotherStore()
    {
        currentNumTrips++;
        if (StoresVisitedToday.Count < StoreAwareness.Count)
            targetStore(PickNextStore().gameObject);
        else
            currentNumTrips = maxTrips;

        if (currentNumTrips >= maxTrips)
        {
            targetHome();
            currentNumTrips = 0;
            inspectorData.AddUpdate("I'm just giving up and going home.");
        }
        else
            inspectorData.AddUpdate("I'll try " + moveTarget.gameObject.name + " next."); //I'll try a max of " + (maxTrips - currentNumTrips) + " more stores."
    }

    private void PickNewProduct()
    {
        desiredProduct = Zeitgeist.RecipePopularities.RollRandomItem();
    }
}
