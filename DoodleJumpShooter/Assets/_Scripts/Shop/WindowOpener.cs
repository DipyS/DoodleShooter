using UnityEngine;

public class WindowOpener : MonoBehaviour
{
    [SerializeField] GameObject window;
    public void Show() {
        window.SetActive(true);
    }
    public void Hide() {
        window.SetActive(false);
    }
}
