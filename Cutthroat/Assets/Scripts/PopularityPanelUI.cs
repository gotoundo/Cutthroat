using UnityEngine;
using System.Collections;

public class PopularityPanelUI : MonoBehaviour
{
    public GameObject Template;

    // Use this for initialization
    void Start()
    {

    }

    void SetUpStore(StoreBase store)
    {
        GameObject o = (GameObject)Instantiate(Template);
        o.transform.SetParent(transform);

        ProgressPanel panel = o.GetComponent<ProgressPanel>();
        panel.myStore = store;

        o.name = "Popularity of "+store.gameObject.name;
    }

    // Update is called once per frame
    bool firstTime = true;
    void Update()
    {
        if (firstTime)
        {
            firstTime = false;
            foreach (StoreBase store in GameManager.AllStores)
                SetUpStore(store);
        }
    }
}
