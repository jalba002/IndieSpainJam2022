using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    public CanvasFadeIn LoadingScreen;
    public CanvasGroup canvGroup;
    public bool isGamePaused = false;
    private CanvasFadeIn canvasFade;

    public static PauseManager Instance;
    PlayerInputsManager playerMenuInputs;

    private void Awake()
    {
        Instance = this;
        canvasFade = GetComponent<CanvasFadeIn>();
        playerMenuInputs = FindObjectOfType<PlayerInputsManager>();
    }

    public void Resume()
    {
        PauseGame();
        playerMenuInputs.ChangeInputs();
    }

    public void LoadScene(string scene_name)
    {
        LoadingScreen.gameObject.SetActive(true);
        LoadingScreen.FadeIn();
        StartCoroutine(LoadAsynchronously(scene_name));
    }

    IEnumerator LoadAsynchronously(string scene_name)
    {
        yield return new WaitForSecondsRealtime(1.5f);

        SceneManager.LoadScene(scene_name);

        yield return new WaitForSecondsRealtime(2f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            canvasFade.FadeIn();
        }
        else
        {
            canvasFade.FadeOut();
        }
        Time.timeScale = isGamePaused ? 0 : 1;
    }
}