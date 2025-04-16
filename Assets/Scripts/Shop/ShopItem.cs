using System;
using UnityEngine;
using UnityEngine.U2D;

[Serializable]
public class ShopItem
{
    public string itemName;
    public string description;
    public float price;
    public int level;
    public float priceMultiplier;
    public Action onPurchase;
    public Sprite icon;
    public string PriceKey => "ShopItem_" + itemName + "_Price";
    public string LevelKey => "ShopItem_" + itemName + "_Level";

    public ShopItem(string name, string desc, float cost, Action effect, Sprite sprite, float multiplier = 1.5f)
    {
        itemName = name;
        description = desc;
        price = cost;
        onPurchase = effect;
        priceMultiplier = multiplier;
        icon = sprite;
        level = 0;
    }

    public void Buy()
    {
        level++;
        price = (float)Math.Round(price * priceMultiplier, 2);


        onPurchase?.Invoke();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat(PriceKey, price);
        PlayerPrefs.SetInt(LevelKey, level);
    }

    public void Load()
    {
        price = PlayerPrefs.GetFloat(PriceKey, price); // Usa valor actual como predeterminado
        level = PlayerPrefs.GetInt(LevelKey, level);
    }
}
