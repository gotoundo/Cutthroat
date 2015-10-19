using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class StoreUpgrade
{
    const int baseNumOfLevels = 8; //1st level counts as base "0"th upgrade level
    const int baseUpgradeCost = 100; //in gold

    public enum Type { Storefront, ProductionSpeed, Amenities };
    public static Dictionary<Type, StoreUpgrade> Definitions;


    public static void Initialize()
    {
        Definitions = new Dictionary<Type, StoreUpgrade>();
        Definitions.Add(Type.Storefront, new StoreUpgrade("Storefront", baseNumOfLevels, baseUpgradeCost, 1f, "Makes puppies who are walking by more likely to stop in."));
        Definitions.Add(Type.ProductionSpeed, new StoreUpgrade("Cauldron", baseNumOfLevels, baseUpgradeCost, 0.5f, "Increases speed of potion brewing, for shorter lines."));
        Definitions.Add(Type.Amenities, new StoreUpgrade("Amenities", baseNumOfLevels, baseUpgradeCost, 1f, "Causes puppies to have a positive experience at your store."));
    }

    public string Name;
    public string Description;

    public UpgradeLevel[] Levels;
    public StoreUpgrade(string Name, int levels, int costBase, float effectBase, string Description)
    {
        this.Name = Name;
        this.Description = Description;
        Levels = new UpgradeLevel[levels];
        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i] = new UpgradeLevel();
            Levels[i].cost = costBase * i;
            Levels[i].effect = i * effectBase;
        }
    }
    

    public static StoreUpgrade Storefront; //increases 
    public static StoreUpgrade ProductionSpeed; //increases speed that customers are served
    public static StoreUpgrade Amenities; //increases favorability upon visit

}

public class UpgradeLevel
{
    public int cost;
    public float effect;
}

