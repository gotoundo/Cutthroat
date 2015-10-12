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
        Definitions.Add(Type.Storefront, new StoreUpgrade("Storefront", numberOfLevels));
        Definitions.Add(Type.ProductionSpeed, new StoreUpgrade("Production Speed", numberOfLevels));
        Definitions.Add(Type.Amenities, new StoreUpgrade("Amenities", numberOfLevels));
    }

    public string Name;

    public UpgradeLevel[] Levels;
    public StoreUpgrade(string Name, int levels)
    {
        this.Name = Name;
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

