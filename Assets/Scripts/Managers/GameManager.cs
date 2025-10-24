using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Coroutine changeLevelCoroutine;
    private UI_Fade fadeUI;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
         
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeLevelTo(int levelBuildIndex)
    {
        if (changeLevelCoroutine != null)
            StopCoroutine(changeLevelCoroutine);

        StartCoroutine(ChangeLevelCoroutine(levelBuildIndex));
    }

    private IEnumerator ChangeLevelCoroutine(int levelBuildIndex)
    {
        GetFadeUI().FadeOut();

        yield return GetFadeUI().fadeCoroutine;

        SceneManager.LoadScene(levelBuildIndex);
    }

    private UI_Fade GetFadeUI()
    {
        if (fadeUI == null)
            fadeUI = FindFirstObjectByType<UI_Fade>();

        return fadeUI;
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
