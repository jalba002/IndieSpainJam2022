using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialModifier : MonoBehaviour
{
    [SerializeField]
    private Renderer[] renderers;

    [ColorUsage(true, true)]
    public Color goddessColor;

    [ColorUsage(true, true)]
    public Color goddessHairColor;

    private Color normalColor;

    private Color normalHairColor;

    public GameObject wingL;
    public GameObject wingR;


    [SerializeField]
    private float Delay;
    [SerializeField]
    private float ActivateDuration;
    [SerializeField]
    private float DeactivateDuration;
    [SerializeField]
    private float NormalFresnel;
    [SerializeField]
    private float GoddessFresnel;

    [SerializeField]
    private ParticleSystem[] GoddessParticles;

    [SerializeField]
    private StudioEventEmitter GoddessActivateSoundRef;

    [SerializeField]
    private StudioEventEmitter GoddessDeactivateSoundRef;

    private void Start()
    {
        NormalFresnel = renderers[0].material.GetFloat("_FresnelPower");
        normalColor = renderers[0].material.GetColor("_GalaxyColor");

        normalHairColor = renderers[1].material.GetColor("_GalaxyColor");

        foreach (var item in GoddessParticles)
        {
            item.Stop();
        }
        wingL.SetActive(false);
        wingR.SetActive(false);
    }

    public void ChangeMaterial(bool goddessState)
    {
        if (goddessState)
        {
            StartCoroutine(MaterialChangeCoroutine(NormalFresnel, GoddessFresnel, ActivateDuration, Delay, normalColor, goddessColor, normalHairColor, goddessHairColor));
            foreach (var item in GoddessParticles)
            {
                item.Play();
            }
            GoddessActivateSoundRef.Play();
            wingL.SetActive(true);
            wingR.SetActive(true);
        }
        else
        {
            StartCoroutine(MaterialChangeCoroutine(GoddessFresnel, NormalFresnel, DeactivateDuration, 0f, goddessColor, normalColor, goddessHairColor, normalHairColor));
            foreach (var item in GoddessParticles)
            {
                item.Stop();
            }
            GoddessDeactivateSoundRef.Play();
            wingL.SetActive(false);
            wingR.SetActive(false);
        }
    }

    IEnumerator MaterialChangeCoroutine(float start, float end, float duration, float delay, Color startingColor, Color endingColor, Color startingHairColor, Color endingHairColor)
    {
        float counter = 0;
        float lerpedValue;
        Color lerpedColor;
        Color lerpedHairColor;

        yield return new WaitForSeconds(delay);

        while (counter < duration)
        {
            counter += Time.deltaTime;

            lerpedValue = Mathf.Lerp(start, end, counter / duration);
            lerpedColor = Color.Lerp(startingColor, endingColor, counter / duration);
            lerpedHairColor = Color.Lerp(startingHairColor, endingHairColor, counter / duration);
            renderers[0].material.SetFloat("_FresnelPower", lerpedValue);
            renderers[0].material.SetColor("_GalaxyColor", lerpedColor);
            renderers[1].material.SetColor("_GalaxyColor", lerpedHairColor);

            yield return null;
        }

        renderers[0].material.SetFloat("_FresnelPower", end);
        renderers[0].material.SetColor("_GalaxyColor", endingColor);
        renderers[1].material.SetColor("_GalaxyColor", endingHairColor);
    }

    IEnumerator GoddessCoroutine(float duration)
    {
        float counter = renderers[0].material.GetFloat("_FresnelPower");
        float lerpedValue;

        yield return new WaitForSeconds(Delay);

        while (counter < duration)
        {
            counter += Time.deltaTime;

            lerpedValue = Mathf.Lerp(GoddessFresnel, NormalFresnel, counter / duration);
            renderers[0].material.SetFloat("_FresnelPower", lerpedValue);

            yield return null;
        }

        renderers[0].material.SetFloat("_FresnelPower", NormalFresnel);
    }
}
