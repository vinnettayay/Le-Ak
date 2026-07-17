using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadAnyScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
