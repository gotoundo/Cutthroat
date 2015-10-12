using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class StoreUpgrade
{
    const int numberOfLevels = 5;

    public enum Type { Storefront, ProductionSpeed, Amenities };
    public static Dictionary<Type, StoreUpgrade> Definitions;


    public static void Initialize()
    {
        Definitions = new Dictionary<Type, StoreUpgrade>();
        Definitions.Add(Type.Storefront, new StoreUpgrade("Storefront", numberOfLevels, "Makes puppies who are walking by more likely to stop in."));
        Definitions.Add(Type.ProductionSpeed, new StoreUpgrade("Production Speed", numberOfLevels, "Increases rate of potion creation, for shorter lines."));
        Definitions.Add(Type.Amenities, new StoreUpgrade("Amenities", numberOfLevels, "Causes puppies to have a positive experience at your store."));
    }

    public string Name;
    public string Description;

    public UpgradeLevel[] Levels;
    public StoreUpgrade(string Name, int levels, string Description)
    {
        this.Name = Name;
        this.Description = Description;
        Levels = new UpgradeLevel[levels];
        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i] = new UpgradeLevel();
            Levels[i].cost = 100 * i;
            Levels[i].effect = i;
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

