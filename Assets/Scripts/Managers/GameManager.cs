using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    public Player player_;
    public FloorChunk floor_chunk_;

    public TMP_Text timer_text;
    public float mini_game_timer = 300.0f;

    [SerializeField] private float speed_increment_ = 0.2f;
    [SerializeField] private float delay_ = 5.0f;

    public bool is_scene_changing = false;

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

        player_.PlayerControl();
        UpdateTimer();
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


        if (SceneManager.GetActiveScene().name == "MinigameScene")
        {
            StartCoroutine(SlightIncrement());
        }
        else
        {
            StopCoroutine(SlightIncrement());
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
                is_scene_changing = true;
                int current_index = SceneManager.GetActiveScene().buildIndex;
                int previous_index = Mathf.Max(0, current_index - 1);
                SceneManager.LoadScene(previous_index);

                mini_game_timer = 300.0f;
            }
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

    public Player player()
    {
        return player_;
    }
}
