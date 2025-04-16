using Unity.VisualScripting;
using UnityEngine;

public class DashButton : MonoBehaviour
{
    public void OnDashButtonClicked()
    {
        if (GameManager.instance.player())
        {
            GameManager.instance.player().StartDashing();
        }
    }
}
