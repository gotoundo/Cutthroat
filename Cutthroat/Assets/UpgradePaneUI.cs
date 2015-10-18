using UnityEngine;
using System.Collections;

public class UpgradePaneUI : MonoBehaviour {
    public GameObject UpgradeRowTemplate;
    public StoreUpgrade.Type[] UpgradesOffered = { StoreUpgrade.Type.Storefront, StoreUpgrade.Type.ProductionSpeed, StoreUpgrade.Type.Amenities };

	// Use this for initialization
	void Start () {
        foreach (StoreUpgrade.Type upgradeID in UpgradesOffered)
        {
            GameObject upgradeObject = Instantiate(UpgradeRowTemplate);
            upgradeObject.transform.SetParent(transform);
            UpgradeUI upgradeUI = upgradeObject.GetComponent<UpgradeUI>();
            upgradeUI.UpgradeType = upgradeID;
        }
	
	}
}
