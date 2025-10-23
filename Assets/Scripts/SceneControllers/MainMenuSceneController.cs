using UnityEngine;

public class MainMenuSceneController : MonoBehaviour
{
    public GameObject quitMenuPanel;

    private void Start()
    {
        SetActiveMenuPanel(false);
        BGMManager.instance.ChangeSongTo(BGMSong.titleSong);
    }

    public void PlayGame()
    {
        GameManager.instance.ChangeLevelTo(1);
    }

    public void SetActiveMenuPanel(bool active) => quitMenuPanel.SetActive(active);
}
