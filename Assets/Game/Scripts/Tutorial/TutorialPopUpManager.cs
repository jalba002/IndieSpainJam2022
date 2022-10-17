using System.Collections;
using CosmosDefender;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopUpManager : MonoBehaviour
{
    Coroutine popUpCoroutine;
    Coroutine backgroundCoroutine;
    private PlayerInputs playerInputs;
    [SerializeField] private GameObject backgroundScreen;

    public static TutorialPopUpManager Instance;

    [SerializeField] private TutorialPopUp prefab;
    private TutorialPopUp popupInstance;

    [SerializeField] private TutorialConfig tutorial;

    private void Awake()
    {
        Instance = this;

        playerInputs = GetComponent<PlayerInputs>();
        popupInstance = Instantiate(prefab, backgroundScreen.transform.parent);
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
    }

    public void ActivateTutorial(TutorialConfig configSet, float delay)
    {
        //Should use a playerPref
        //currentCanvasActive = tutorialCanvases[index];
        popupInstance.Configure(configSet);
        StartCoroutine(ActivateAfterTimeCoroutine(popupInstance, delay));
    }

    private void Deactivate()
    {
        popUpCoroutine = StartCoroutine(FadeCoroutine(1f, 0f, 0.3f, popupInstance.gameObject));
        backgroundCoroutine = StartCoroutine(FadeCoroutine(0.5f, 0f, 0.3f, backgroundScreen));
        Time.timeScale = 1;
        playerInputs.SetInputMap(PlayerInputMaps.Ingame);
    }

    IEnumerator ActivateAfterTimeCoroutine(TutorialPopUp tutorialPopUp, float delay)
    {
        yield return new WaitForSeconds(delay);
        tutorialPopUp.gameObject.SetActive(true);
        popUpCoroutine = StartCoroutine(FadeCoroutine(0f, 1f, 0.3f, tutorialPopUp.gameObject));
        backgroundScreen.gameObject.SetActive(true);
        backgroundCoroutine = StartCoroutine(FadeCoroutine(0f, 0.5f, 0.3f, backgroundScreen));
        playerInputs.SetInputMap(PlayerInputMaps.Tutorial);
        Time.timeScale = 0;
    }

    IEnumerator FadeCoroutine(float start, float end, float duration, GameObject canvas)
    {
        float counter = 0f;
        float alpha;
        CanvasGroup canva = canvas.GetComponent<CanvasGroup>();

        while (counter < duration)
        {
            counter += Time.unscaledDeltaTime;
            alpha = Mathf.Lerp(start, end, counter / duration);

            canva.alpha = alpha;

            yield return null;
        }

        if (end == 0f)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    IEnumerator FadeCoroutine(float start, float end, float duration, Image background)
    {
        float counter = 0f;
        float alpha;
        Color newColor = new Color(0f, 0f, 0f, start);

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