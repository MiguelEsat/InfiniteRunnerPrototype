using UnityEngine;

public class ShopOpenner : MonoBehaviour
{
    public GameObject ShopPanel;
    private bool activate_ = false;

    public void OpenPanel()
    {
        if(ShopPanel != null)
        {
            activate_ = !activate_;
            ShopPanel.SetActive(activate_);
        }
    }
}
