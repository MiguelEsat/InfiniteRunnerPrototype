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
    public Sprite HelmetIcon;
    public Sprite ChestIcon;
    public Sprite PantsIcon;
    public Sprite BootsIcon;
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

        ShopItem sword = new ShopItem("Sword", "0.1", 1f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 0.1f });
        }, swordIcon);

        sword.Load();
        items.Add(sword);


        ShopItem hammer = new ShopItem("Hammer", "1", 10f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 1.0f });
        }, HammerIcon);
       
        hammer.Load();
        items.Add(hammer);

        ShopItem axe = new ShopItem("Axe", "10", 100f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 10.0f });
        }, AxeIcon);

        axe.Load();
        items.Add(axe);


        ShopItem bow = new ShopItem("Bow", "25", 200f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 25.0f });
        }, BowIcon);

        bow.Load();
        items.Add(bow);

        ShopItem helmet = new ShopItem("Helmet", "50", 500f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 50.0f });
        }, HelmetIcon);

        helmet.Load();
        items.Add(helmet);



        ShopItem chest = new ShopItem("ChestPlate", "100", 1000f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 100.0f });
        }, ChestIcon);

        chest.Load();
        items.Add(chest);

        ShopItem pants = new ShopItem("Leggins", "500", 10000f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 500.0f });
        }, PantsIcon);

        pants.Load();
        items.Add(pants);

        ShopItem boots = new ShopItem("Boots", "1.000", 100000f, () => {
            typeof(CoinManager)
                .GetMethod("UpdateCoinsPerSecond", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(CoinManager.instance, new object[] { 1000.0f });
        }, BootsIcon);

        boots.Load();
        items.Add(boots);




        // Instanciamos un botón para cada item
        foreach (var item in items)
        {
            GameObject obj = Instantiate(shopItemButtonPrefab, itemListParent);
            ShopItemButton btn = obj.GetComponent<ShopItemButton>();
            btn.Setup(item); // Aquí le pasamos los datos al botón
        }
 
    }



}