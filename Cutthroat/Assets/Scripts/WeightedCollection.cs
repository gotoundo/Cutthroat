//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class WeightedCollection<TKey>
{
    Dictionary<TKey, float> Collection;
    public WeightedCollection()
    {
        Collection = new Dictionary<TKey, float>();
    }

    float TotalWeight()
    {
        float total = 0;
        foreach (TKey item in KeyList())
            total+=Collection[item];
        return total;
    }

    public float ChanceOfItem(TKey key, bool asIntPercentage = false)
    {
        if (!asIntPercentage)
            return Collection[key] / TotalWeight();
        else
            return Mathf.RoundToInt(100 * Collection[key] / TotalWeight());
    }

    void EnsurePresence(TKey key)
    {
        if (!Collection.ContainsKey(key))
            Collection.Add(key, 0);
    }

    public void AddWeight(TKey key, float weight)
    {
        EnsurePresence(key);
        Collection[key] += weight;
    }

    public bool RemoveItem(TKey key)
    {
        return Collection.Remove(key);
    }
    
    public List<TKey> KeyList()
    {
        return new List<TKey>(Collection.Keys);
    }

    public void FlattenAllWeights(float weight)
    {
        foreach (TKey item in KeyList())
            Collection[item] = weight;
    }

    public float this[TKey key]
    {
        get
        {
            return Collection[key];
        }
        set
        {
            Collection[key] = value;
        }
    }


    public TKey RollRandomItem()
    {
        float totalWeight = 0;
        foreach (TKey item in KeyList())
            totalWeight += Collection[item];

        float rolledWeight = Random.Range(0, totalWeight);

        foreach (TKey item in KeyList())
        {
            rolledWeight -= Collection[item];
            if (rolledWeight <= 0)
                return item;
        }

        throw new System.Exception("Error rolling for item in this weighted collection "+"(Size"+ Collection.Count+") :" +Collection.ToString());
    }

}

