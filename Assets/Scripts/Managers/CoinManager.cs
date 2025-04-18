using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    static public CoinManager instance;

    public float score_;
    private float coins_per_second_ = 0.1f;

    private float time_accumulator_ = 0.0f;
    private float update_interval_ = 1.0f;

    public TMP_Text score_text;
    public TMP_Text cps_text;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        score_ = PlayerPrefs.GetFloat("Coins", 0.0f);
        coins_per_second_ = PlayerPrefs.GetFloat("CPS", 0.0f);
    }

    public void EarnCoins(float amount)
    {
        score_ += amount;

        PlayerPrefs.SetFloat("Coins", score_);
    }

    void UpdateCoinsPerSecond(float amount)
    {
        coins_per_second_ += amount;

        PlayerPrefs.SetFloat("CPS", coins_per_second_);
    }

    public void UpdateCoins()
    {
        time_accumulator_ += Time.deltaTime;
        if (time_accumulator_ > update_interval_)
        {
            EarnCoins(coins_per_second_ * update_interval_);
            time_accumulator_ -= time_accumulator_;
        }
    }
    public bool SpendCoins(float amount)
    {
        if (score_ > amount)
        {
            score_ -= amount;
            Debug.Log("Gastaste: " + amount + ", Te quedan: " + score_);
            return true;
        }
        return false;
    }
    public void UpdateTextOnScreen()
    {
        if (score_text)
        {
            score_text.text = "" + score_.ToString("F1"); 
        }

        if (cps_text)
        {
            cps_text.text = coins_per_second_.ToString("F1") + " Coins Per Second";
        }
    }
}
