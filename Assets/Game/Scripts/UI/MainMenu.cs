using System;
using System.Collections;
using System.Collections.Generic;
using CosmosDefender;
using FMODUnity;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum MenuState
{
    Main,
    Options,
    Controls,
    Credits,
    Shop
}

public class MainMenu : MonoBehaviour
{
    private MenuState currentMenu = MenuState.Main;
    private MenuState previousMenu;

    private CanvasGroup canvas;
    public CanvasFadeIn LoadingScreen;
    public CanvasFadeIn OptionsCanvas;
    public CanvasFadeIn ControlsCanvas;
    public CanvasFadeIn CreditsCanvas;
    public CanvasFadeIn ShopCanvas;
    public float Duration = 0.4f;

    [SerializeField]
    private Button startGameButton;

    [SerializeField]
    private Button optionsButton;

    [SerializeField]
    private Button creditsButton;

    [SerializeField]
    private Button quitGameButton;

    [SerializeField]
    private Button shareButton;

    [SerializeField]
    private Button shopButton;

    [SerializeField]
    private GameStateCanvasTable gameStateCanvasTable;

    [SerializeField]
    private GameStateButtonTable gameStateButtonTable;

    public Animator protagonistAnimator;

    [SerializeField]
    private MenuController menuController;

    //public StudioEventEmitter HoverSoundRef;
    public StudioEventEmitter ClickSoundRef;
    public StudioEventEmitter BackgroundMusic;

    private readonly string twitterNameParameter = "Juega a este increíble juego creado por @andrew_raaya @JordiAlbaDev @Sergisggs @GuillemLlovDev @Belmontes_ART @montane @ovillaloboss_ y @RenderingCode hecho para la #IndieSpainJam (@IndieDevDay @spaingamedevs)!\n\nAquí tenéis el link:\n\n";
    private readonly string twitterDescriptionParam = "";
    private readonly string twitterAdress = "https://twitter.com/intent/tweet";
    private readonly string miniGameJamLink = "https://andrew-raya.itch.io/cosmic-defender";

    private void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        LoadingScreen.gameObject.SetActive(true);
        LoadingScreen.GetComponent<CanvasFadeIn>().FadeOut();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 1f;

        gameStateCanvasTable.Add(MenuState.Main, GetComponent<CanvasFadeIn>());
        gameStateCanvasTable.Add(MenuState.Options, OptionsCanvas);
        gameStateCanvasTable.Add(MenuState.Controls, ControlsCanvas);
        gameStateCanvasTable.Add(MenuState.Credits, CreditsCanvas);
        gameStateCanvasTable.Add(MenuState.Shop, ShopCanvas);

        foreach (var item in gameStateCanvasTable)
        {
            if (item.Key == MenuState.Main)
                item.Value.FadeIn();
            else
                item.Value.FadeOut();
        }
    }

    public void LoadScene(string scene_name)
    {
        BackgroundMusic.Stop();
        canvas.interactable = false;
        LoadingScreen.FadeIn();
        //protagonistAnimator.SetTrigger("Start");
        StartCoroutine(LoadAfterFade(scene_name));

        PlayClickSound();
    }

    IEnumerator LoadAfterFade(string scene_name)
    {
        float counter = 0f;

        while (counter < Duration)
        {
            counter += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(1, 0, counter / Duration);

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        //SceneManager.LoadScene(scene_name);
        menuController.LoadGame();
    }

    public void PlayHoverSound()
    {
        //HoverSoundRef.Play();
    }

    public void PlayClickSound()
    {
        ClickSoundRef.Play();
    }

    public void MainMenuFade()
    {
        SetCanvas(MenuState.Main);
    }

    public void ControlsMenu()
    {
        SetCanvas(MenuState.Controls);
    }

    public void SetCanvas(MenuState newMenu)
    {
        previousMenu = currentMenu;
        currentMenu = newMenu;

        gameStateCanvasTable[newMenu].FadeIn();
        gameStateCanvasTable[previousMenu].FadeOut();

        PlayClickSound();
    }

    public void OptionsMenu()
    {
        SetCanvas(MenuState.Options);
    }

    public void CreditsMenu()
    {
        SetCanvas(MenuState.Credits);
    }

    public void ShopMenu()
    {
        SetCanvas(MenuState.Shop);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShareTwitter()
    {
        Application.OpenURL(twitterAdress + "?text=" + UnityWebRequest.EscapeURL(twitterNameParameter + "\n" + twitterDescriptionParam + "\n" + miniGameJamLink));
        PlayClickSound();
    }

    [Serializable]
    public class GameStateCanvasTable : UnitySerializedDictionary<MenuState, CanvasFadeIn> { }

    [Serializable]
    public class GameStateButtonTable : UnitySerializedDictionary<MenuState, ButtonsList> { }

    [Serializable]
    public class ButtonsList
    {
        public List<Button> buttonsList;
    }
}