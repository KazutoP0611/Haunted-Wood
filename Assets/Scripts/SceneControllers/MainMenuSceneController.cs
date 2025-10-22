using UnityEngine;

public class MainMenuSceneController : MonoBehaviour
{
    public GameObject quitMenuPanel;

    private void Start()
    {
        SetActiveMenuPanel(false);
    }

    public void SetActiveMenuPanel(bool active) => quitMenuPanel.SetActive(active);
}
