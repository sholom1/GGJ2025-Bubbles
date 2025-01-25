using UnityEngine;
using UnityEngine.SceneManagement;

public class BubblesSceneManager : MonoBehaviour
{

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
