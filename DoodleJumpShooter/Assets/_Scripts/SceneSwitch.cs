using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{
    public static SceneSwitch Instance { get; private set; }
    public TextMeshProUGUI loadingText;
    public Image loadingImage;
    Animator anim;
    AsyncOperation LoadingSceneOperation;
    public static bool shouldPlayOpeningAnimation = false;
    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();

        if (shouldPlayOpeningAnimation) {
            anim.SetTrigger("SceneOpen");
        }
    }

    void Update()
    {
        if (LoadingSceneOperation != null) {
            loadingText.text = "Loading: " + Mathf.RoundToInt(LoadingSceneOperation.progress * 100) + "%";
            loadingImage.fillAmount = LoadingSceneOperation.progress;
        }
    }


    public static void SwitchToScene(string sceneName)
    {
        Instance.anim.SetTrigger("SceneClose");
        Instance.LoadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        Instance.LoadingSceneOperation.allowSceneActivation = false;
    }

    public void OnAnimOver() {
        LoadingSceneOperation.allowSceneActivation = true;
        shouldPlayOpeningAnimation = true;
    }
}
