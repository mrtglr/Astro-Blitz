using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Done_HomeSceneController : MonoBehaviour
{
    public Text highScoreText;
    private string highScoreKey = "HighScore";

    void Start()
    {
        int highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("mainscene");
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
