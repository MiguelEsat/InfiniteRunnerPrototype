using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DashButton : MonoBehaviour
{
    [SerializeField] private Button button_;
    [SerializeField] private Image image_frame_;

    private void Update()
    {
        ChangeUpButton();
    }

    public void OnDashButtonClicked()
    {
        if (GameManager.instance.player())
        {
            GameManager.instance.player().StartDashing();
        }
    }

    void ChangeUpButton()
    {
        ColorBlock prev_color = button_.colors;
        if (button_)
        {
            Image button_image = button_.GetComponent<Image>();
            if (button_image)
            {
                Color image_color = button_image.color;
                Color frame_color = image_frame_.color;

                if (GameManager.instance.player().is_on_cooldown)
                {
                    image_color.a = 0.5f;
                    frame_color.a = 0.5f;
                }
                else
                {
                    image_color.a = 1.0f;
                    frame_color.a = 1.0f;
                }

                button_image.color = image_color;
                image_frame_.color = frame_color;
            }
        }
    }
}
