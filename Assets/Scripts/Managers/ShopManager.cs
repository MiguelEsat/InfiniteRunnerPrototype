using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    static public ShopManager instance;

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

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        items.Add(new ShopItem("Sword", "0.1", 1f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 0.1f });
        }, swordIcon));
        
        items[0].price = PlayerPrefs.GetFloat("Price", 0.1f);
        items[0].level = PlayerPrefs.GetInt("Item Level", 0);

        items.Add(new ShopItem("Hammer", "1", 10f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 1.0f });
        }, HammerIcon));

        items[1].price = PlayerPrefs.GetFloat("Price", 1.0f);
        items[1].level = PlayerPrefs.GetInt("Item Level", 0);

        items.Add(new ShopItem("Axe", "10", 100f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 10.0f });
        }, AxeIcon));

        items[2].price = PlayerPrefs.GetFloat("Price", 1.0f);
        items[2].level = PlayerPrefs.GetInt("Item Level", 0);

        items.Add(new ShopItem("Bow", "25", 200f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 25.0f });
        }, BowIcon));
        items[3].price = PlayerPrefs.GetFloat("Price", 25.0f);
        items[3].level = PlayerPrefs.GetInt("Item Level", 0);

        // Instanciamos un botón para cada item
        foreach (var item in items)
        {
            GameObject obj = Instantiate(shopItemButtonPrefab, itemListParent);
            ShopItemButton btn = obj.GetComponent<ShopItemButton>();
            btn.Setup(item); // Aquí le pasamos los datos al botón
        }
 
    }

    private void OnApplicationQuit()
    {
        foreach (var item in items)
            item.SaveItems();
    }

}