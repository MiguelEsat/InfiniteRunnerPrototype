using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Diagnostics;

public class ShopItemButton : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI descriptionText;
    public Image iconImage;
    public Button buyButton;
    public Image backgroundImage;

    private ShopItem currentItem;


    public void Setup(ShopItem item)
    {
        currentItem = item;


        iconImage.sprite = item.icon;
        priceText.text = "Price: " + item.price.ToString("F1");
        levelText.text = "Lvl: " + item.level;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);
    }
    void Update()
    {
        if (priceText && levelText && descriptionText)
            UpdateButtonUI();
    }

    

    void BuyItem()
    {
        if (CoinManager.instance.SpendCoins(currentItem.price))
        {
            currentItem.Buy();
            currentItem.SaveItems();
            UpdateButtonUI();
            CoinManager.instance.UpdateTextOnScreen();
        }
      /*  else
        {
            Debug.Log("No tienes suficientes monedas");
        }*/
    }

    void UpdateButtonUI()
    {

        priceText.text = "Price: " + currentItem.price.ToString("F1");
        levelText.text = "Lvl: " + currentItem.level;
        descriptionText.text = "Cps: " + currentItem.description;




        bool canAfford = CoinManager.instance.score_ >= currentItem.price;

        if (canAfford)
        {
            buyButton.image.color = Color.green;
        }
        else
        {
            buyButton.image.color = Color.red;
        }
    }


}
