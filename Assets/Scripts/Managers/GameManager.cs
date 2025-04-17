using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    public Player player_;
    public FloorChunk floor_chunk_;

    [Header("Minigame")]
    public TMP_Text timer_text;
    public TMP_Text win_text;
    public TMP_Text loss_text;

    public Image image_condition;

    public float mini_game_timer = 300.0f;
    public float screen_timer;

    [SerializeField] private float speed_increment_ = 0.2f;
    [SerializeField] private float delay_ = 5.0f;
    [SerializeField] private float coin_reward_ = 100000.0f;

    public bool is_scene_changing = false;
    public bool start_timer = false;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ReassignObjects();
    }

    void Update()
    {
        CoinManager.instance.UpdateCoins();
        CoinManager.instance.UpdateTextOnScreen();

        if (!start_timer)
        {
            player_.PlayerControl();
        }

        UpdateTimer();
        PlayerLoss();
    }

    private void FixedUpdate()
    {
        player_.UpdateCollisionBox();

        floor_chunk_.UpdateChunks();
        if (player_.is_dashing)
        {
            floor_chunk_.UpdateChunkSpeed();
        } else
        {
            if (SceneManager.GetActiveScene().name != "MinigameScene")
                floor_chunk_.ResetSpeed();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        is_scene_changing = false;
        ReassignObjects();
        mini_game_timer = 300.0f;
        start_timer = false;
        screen_timer = 0.0f;

        if (SceneManager.GetActiveScene().name == "MinigameScene")
        {
            StartCoroutine(SlightIncrement());
            timer_text.gameObject.SetActive(true);
        }
        else
        {
            StopCoroutine(SlightIncrement());
            timer_text.gameObject.SetActive(false);
        }
    }

    private void ReassignObjects()
    {
        player_ = FindAnyObjectByType<Player>();
        floor_chunk_ = FindAnyObjectByType<FloorChunk>();
    }

    private void ShowTimer ()
    {
        int minutes = Mathf.FloorToInt(mini_game_timer / 60);
        int seconds = Mathf.FloorToInt(mini_game_timer % 60);

        timer_text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateTimer()
    {
        if (SceneManager.GetActiveScene().name == "MinigameScene")
        {
            if (mini_game_timer > 0)
            {
                mini_game_timer -= Time.deltaTime;
                ShowTimer();
            } else
            {
                image_condition.gameObject.SetActive(true);
                win_text.gameObject.SetActive(true);
                screen_timer += Time.deltaTime;
            }
            GoBackToGame();
        } 
    }

    private IEnumerator SlightIncrement()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay_);

            if (floor_chunk_ != null)
            {
                floor_chunk_.IncrementSpeed(speed_increment_);

                yield return null;
            }
        }
    }

    public void GoBackToGame() { 
        if (screen_timer > 5.0f && mini_game_timer < 0)
        {
            is_scene_changing = true;
            CoinManager.instance.EarnCoins(coin_reward_);
            image_condition.gameObject.SetActive(false);
            win_text.gameObject.SetActive(false);

            int current_index = SceneManager.GetActiveScene().buildIndex;
            int previous_index = Mathf.Max(0, current_index - 1);
            SceneManager.LoadScene(previous_index);

            screen_timer -= screen_timer;
        }
    }

    public void PlayerLoss()
    {
        if (start_timer)
        {
            floor_chunk_.StopChunks();
            image_condition.gameObject.SetActive(true);
            loss_text.gameObject.SetActive(true);

            screen_timer += Time.deltaTime;
        }

        if (screen_timer > 5.0f && mini_game_timer > 0)
        {
            GameManager.instance.is_scene_changing = true;
            player_.PAnimator().SetBool("IsDead", false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

            image_condition.gameObject.SetActive(false);
            loss_text.gameObject.SetActive(false);

            screen_timer -= screen_timer;
        }
    }

    public Player player()
    {
        return player_;
    }
}
