using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    public TextMeshProUGUI text_to_blink;
    public float blink_interval = 0.5f;

    private float timer_;

    void Update()
    {
        if (text_to_blink == null) return;

        timer_ += Time.deltaTime;
        if (timer_ >= blink_interval)
        {
            text_to_blink.enabled = !text_to_blink.enabled;
            timer_ -= timer_;
        }
    }
}
