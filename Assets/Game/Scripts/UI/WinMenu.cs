using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using CosmosDefender;
using UnityEngine.UI;
using FMODUnity;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

//using FMODUnity;

public class WinMenu : MonoBehaviour
{
    private CanvasGroup canvas;
    public float Duration = 0.4f;

    //public StudioEventEmitter HoverSoundRef;
    //public StudioEventEmitter ClickSoundRef;
    public StudioEventEmitter BackgroundMusic;

    private readonly string twitterNameParameter = "Juega a este increible juego de @andrew_raaya @JordiAlbaDev @Sergisggs @GuillemLlovDev @Belmontes_ART @montane @ovillaloboss_ y @RenderingCode hecho para la #IndieSpainJam ! Aquí teneis el link:";
    private readonly string twitterDescriptionParam = "";
    private readonly string twitterAdress = "https://twitter.com/intent/tweet";
    private readonly string miniGameJamLink = "https://andrew-raya.itch.io/cosmic-defender";

    [SerializeField] private LocalizeStringEvent moneyTextLocalized;
    [SerializeField] private LocalizeStringEvent winTextLocalized;


    private void Start()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    public void LoadScene(string scene_name)
    {
        //BackgroundMusic.Stop();
        //
        //LoadingScreen.FadeIn();
        SceneManager.LoadScene(scene_name);
        //StartCoroutine(LoadAfterFade(scene_name));
    }

    public void Win(int money)
    {
        winTextLocalized.SetTable("Menus");
        winTextLocalized.SetEntry("GameWon");
        LoadMoneyText(money);
    }

    public void Loss(int money)
    {
        winTextLocalized.SetTable("Menus");
        winTextLocalized.SetEntry("GameLost");
        LoadMoneyText(money);
    }

    private void LoadMoneyText(int money)
    {
        moneyTextLocalized.StringReference = new LocalizedString
        {
            {"money", new IntVariable() {Value = money}}
        };
        
        moneyTextLocalized.SetTable("Menus");
        moneyTextLocalized.SetEntry("MoneySuggestion");
    }

    IEnumerator LoadAfterFade(string scene_name)
    {
        float counter = 0f;

        canvas.interactable = false;
        while (counter < Duration)
        {
            counter += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(1, 0, counter / Duration);

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(scene_name);
    }

    public void PlayHoverSound()
    {
        //HoverSoundRef.Play();
    }

    public void PlayClickSound()
    {
        //ClickSoundRef.Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShareTwitter()
    {
        Application.OpenURL(twitterAdress + "?text=" + UnityWebRequest.EscapeURL(twitterNameParameter + "\n" + twitterDescriptionParam + "\n" + miniGameJamLink));
    }
}