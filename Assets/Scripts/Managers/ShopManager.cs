using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Prefab y UI")]
    public GameObject shopItemButtonPrefab;
    public Transform itemListParent;
    public Sprite swordIcon;
    public Sprite HammerIcon;
    public Sprite AxeIcon;
    public Sprite BowIcon;
    //public Sprite swordIcon;
    //public Sprite swordIcon;
    private List<ShopItem> items = new List<ShopItem>();



    void Start()
    {

        items.Add(new ShopItem("Sword", "0.1", 1f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 0.1f });
        }, swordIcon));
        items.Add(new ShopItem("Hammer", "1", 10f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 1.0f });
        }, HammerIcon));
        items.Add(new ShopItem("Axe", "10", 100f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 10.0f });
        }, AxeIcon));
        items.Add(new ShopItem("Bow", "25", 200f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 25.0f });
        }, BowIcon));



        // Instanciamos un botón para cada item
        foreach (var item in items)
        {
            GameObject obj = Instantiate(shopItemButtonPrefab, itemListParent);
            ShopItemButton btn = obj.GetComponent<ShopItemButton>();
            btn.Setup(item); // Aquí le pasamos los datos al botón
        }
    }


}