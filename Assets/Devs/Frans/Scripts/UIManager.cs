using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
              Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        #if (UNITY_EDITOR)
              UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
              Application.Quit();
#elif (UNITY_WEBGL)
              Application.OpenURL("itch url https://gohanblade.itch.io/trident-pop");
#endif
    }
}
