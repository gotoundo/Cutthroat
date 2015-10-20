using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IngredientUI : MonoBehaviour
{
    public Image image;
    public Text count;
    public Ingredient ingredient;
    public bool trackPlayerQuantities = false;

    const int CriticalQuantity = 4;
    const int DangerousQuantity = 8;

    bool hasBeenSetUp = false;

    void Update()
    {
        if(!hasBeenSetUp)
        {
            image.overrideSprite = GameManager.IngredientBook[ingredient].Sprite;// TextureManager.IngredientTextures[ingredient];
            hasBeenSetUp = true;
        }

        if (trackPlayerQuantities && GameManager.Main.player != null)
        {
            int quantity = GameManager.Main.player.GetIngredients()[ingredient];
            count.text = "" + quantity;
            if (quantity <= CriticalQuantity)
                count.color = Color.red;
            else if (quantity <= DangerousQuantity)
                count.color = Color.yellow;
            else
                count.color = Color.white;
        }
        
    }
}
