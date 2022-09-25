using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialPopUpManager : MonoBehaviour
{
    private bool activated = false;
    Coroutine popUpCoroutine;
    Coroutine backgroundCoroutine;
    private PlayerInputs playerInputs;
    [SerializeField]
    private CanvasGroup backgroundScreen;
    [SerializeField]
    private CanvasGroup[] tutorialCanvases;

    private CanvasGroup currentCanvasActive;

    public static TutorialPopUpManager Instance;

    private void Awake()
    {
        Instance = this;
        playerInputs = GetComponent<PlayerInputs>();
    }

    private void OnEnable()
    {
        playerInputs.OnDismissAction += Deactivate;
    }

    private void OnDisable()
    {
        playerInputs.OnDismissAction -= Deactivate;
    }

    private void Start()
    {
        ActivateTutorial(0, 2f);
    }

    public void ActivateTutorial(int index, float delay)
    {
        //Should use a playerPref
        currentCanvasActive = tutorialCanvases[index];
        activated = true;
        StartCoroutine(ActivateAfterTimeCoroutine(currentCanvasActive, delay));
    }

    private void Deactivate()
    {
        popUpCoroutine = StartCoroutine(FadeCoroutine(1f, 0f, 0.3f, currentCanvasActive));
        backgroundCoroutine = StartCoroutine(FadeCoroutine(0.5f, 0f, 0.3f, backgroundScreen));
        Time.timeScale = 1;
        playerInputs.SetInputMap(PlayerInputMaps.Ingame);
    }

    IEnumerator ActivateAfterTimeCoroutine(CanvasGroup tutorialPopUp, float delay)
    {
        yield return new WaitForSeconds(delay);
        tutorialPopUp.gameObject.SetActive(true);
        popUpCoroutine = StartCoroutine(FadeCoroutine(0f, 1f, 0.3f, tutorialPopUp));
        backgroundScreen.gameObject.SetActive(true);
        backgroundCoroutine = StartCoroutine(FadeCoroutine(0f, 0.5f, 0.3f, backgroundScreen));
        playerInputs.SetInputMap(PlayerInputMaps.Tutorial);
        Time.timeScale = 0;
    }

    IEnumerator FadeCoroutine(float start, float end, float duration, CanvasGroup canvas)
    {
        float counter = 0f;
        float alpha;

        while (counter < duration)
        {
            counter += Time.unscaledDeltaTime;
            alpha = Mathf.Lerp(start, end, counter / duration);

            canvas.alpha = alpha;

            yield return null;
        }

        if(end == 0f)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    IEnumerator FadeCoroutine(float start, float end, float duration, Image background)
    {
        float counter = 0f;
        float alpha;
        Color newColor = new Color(0f,0f,0f, start);

        while (counter < duration)
        {
            counter += Time.unscaledDeltaTime;
            alpha = Mathf.Lerp(start, end, counter / duration);

            newColor.a = alpha;
            background.color = newColor;

            yield return null;
        }
    }
}
