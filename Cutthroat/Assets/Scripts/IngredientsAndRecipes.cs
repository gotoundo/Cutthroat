using UnityEngine;
using System.Collections.Generic;


public enum Recipe { None, DreamPowder, PassionPotion, QuickElixer, FleaPoultice }
public enum Ingredient { Red, Purple, Blue, Green, Yellow, Orange, White, Black }

public class RecipeDescription
{
    public string Name;
    public string Description;
    public Recipe Type;
    public Sprite Sprite
    {
        get { return TextureManager.PotionTextures[Type]; }
    }


    public Dictionary<Ingredient, int> Ingredients;
    public RecipeDescription(Recipe Type, string Name, string Description)
    {
        Ingredients = new Dictionary<Ingredient, int>();
        this.Type = Type;
        this.Name = Name;
        this.Description = Description;

        GameManager.RecipeBook.Add(Type, this);
    }
}


public class IngredientDescription
{
    public string Name;
    public string Description;
    public Ingredient Type;
    int spriteID;
    public int DefaultPrice;
    public Sprite Sprite
    {
        get { return TextureManager.Main.IngredientIcons[spriteID]; }
    }

    public IngredientDescription(Ingredient Type, string Name, string Description, int spriteID, int DefaultPrice)
    {
        this.Type = Type;
        this.Name = Name;
        this.Description = Description;
        this.spriteID = spriteID;
        this.DefaultPrice = DefaultPrice;
        GameManager.IngredientBook.Add(Type, this);
    }
}