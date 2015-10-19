using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public Text Price;
    public Text Name;
    public Text CurrentLevel;
    public Text Description;
    public Button PurchaseButton;
    public StoreUpgrade.Type UpgradeType;
    public Image Icon;
    //StoreBase player;
    // Use this for initialization
    void Start()
    {
        Name.text = StoreUpgrade.Definitions[UpgradeType].Name;
        Description.text = StoreUpgrade.Definitions[UpgradeType].Description;
        Icon.overrideSprite = TextureManager.UpgradeTextures[UpgradeType];
    }

    // Update is called once per frame
    void Update()
    {
        CurrentLevel.text = "Level " + GameManager.Main.player.GetUpgrades()[UpgradeType];
        PurchaseButton.interactable = GameManager.Main.player.CanPurchaseUpgrade(UpgradeType);
        int goldPrice = GameManager.Main.player.NextUpgradeCost(UpgradeType);
        if (goldPrice == -1)
            Price.text = "Max Upgrade";
        else
            Price.text = "" + GameManager.Main.player.NextUpgradeCost(UpgradeType) + " Gold";
    }

    public void Purchase()
    {
        if (!GameManager.Main.player.TryBuyUpgrade(UpgradeType))
            Debug.Log("Failed to buy " + UpgradeType.ToString());
    }
}
