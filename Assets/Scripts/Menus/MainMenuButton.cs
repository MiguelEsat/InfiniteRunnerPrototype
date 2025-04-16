using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void ChangeScene(string IdleScene)
    {
        SceneManager.LoadScene(IdleScene);
    }
}
