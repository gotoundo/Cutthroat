using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public Text Price;
    public Text Name;
    public Text CurrentLevel;
    public Button PurchaseButton;
    public StoreUpgrade.Type UpgradeType;
    //StoreBase player;
    // Use this for initialization
    void Start()
    {
        Name.text = UpgradeType.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        CurrentLevel.text = "Level " + GameManager.singleton.player.GetUpgrades()[UpgradeType];
        PurchaseButton.interactable = GameManager.singleton.player.CanPurchaseUpgrade(UpgradeType);
        int goldPrice = GameManager.singleton.player.NextUpgradeCost(UpgradeType);
        if (goldPrice == -1)
            Price.text = "Max Upgrade";
        else
            Price.text = "" + GameManager.singleton.player.NextUpgradeCost(UpgradeType) + " Gold";
    }

    public void Purchase()
    {
        if (!GameManager.singleton.player.TryBuyUpgrade(UpgradeType))
            Debug.Log("Failed to buy " + UpgradeType.ToString());
    }
}
