using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] string sceneName = "Game";

    public void SwitchScene() {
        SceneManager.LoadScene(sceneName);
    }
}
