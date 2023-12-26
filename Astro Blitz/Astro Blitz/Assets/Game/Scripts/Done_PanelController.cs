using UnityEngine.SceneManagement;
using UnityEngine;

public class Done_PanelController : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("mainscene");
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("homescene");
    }
}
