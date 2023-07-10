using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitIntro : MonoBehaviour
{
    public Animator ScreenFader;
    GameObject[] MusicComponent, SFXComponent;
    private int[] ScreenWidth = {800, 1024, 1152, 1280, 1280, 1440, 1600, 1920, 2560}, ScreenHeight = {600, 768, 864, 720, 1024, 900, 1200, 1080, 1440}; // [0, 8]
    bool leaving = false;

    void Start()
    {
        MusicComponent = GameObject.FindGameObjectsWithTag("Music");
        SFXComponent = GameObject.FindGameObjectsWithTag("SFX");

        ApplyPlayerSettings();
    }

    void ApplyPlayerSettings()
    {
        SetResolutionSettings();
        SetAudioSettings();

        Debug.Log("[Apply] => " + (int) PlayerPrefs.GetFloat("ScreenWidth") + " : " + (int) PlayerPrefs.GetFloat("ScreenHeight"));
    }

    void SetResolutionSettings()
    {
        float ResolutionIndex = PlayerPrefs.GetFloat("ResSlideVal");

        Screen.SetResolution(ScreenWidth[(int)ResolutionIndex], ScreenHeight[(int)ResolutionIndex], true);

        PlayerPrefs.SetFloat("ScreenWidth", ScreenWidth[(int)ResolutionIndex]);
        PlayerPrefs.SetFloat("ScreenHeight", ScreenHeight[(int)ResolutionIndex]);
        PlayerPrefs.SetFloat("ResSlideVal", ResolutionIndex);
    }

    void SetAudioSettings()
    {
        MusicComponent = GameObject.FindGameObjectsWithTag("Music");
        SFXComponent = GameObject.FindGameObjectsWithTag("SFX");

        float musicVolume = PlayerPrefs.GetFloat("MusicVol");
        float sfxVolume = PlayerPrefs.GetFloat("SFXVol");

        if (MusicComponent != null && SFXComponent != null)
        {
            foreach (GameObject Sound in MusicComponent)
            {
                Sound.GetComponent<AudioSource>().volume = musicVolume;
            }
    
            foreach (GameObject Sound in SFXComponent)
            {
                Sound.GetComponent<AudioSource>().volume = sfxVolume;
            }
        }
    }

    void StartFadingIn()
    {
        if (MusicComponent != null && SFXComponent != null)
        {
            foreach (GameObject Sound in MusicComponent)
            {
                StartCoroutine(FadeIn(Sound.GetComponent<AudioSource>(), 0.25f));
            }
    
            foreach (GameObject Sound in SFXComponent)
            {
                StartCoroutine(FadeIn(Sound.GetComponent<AudioSource>(), 0.25f));
            }
        }
    }

    void StartFadingOut()
    {
        if (MusicComponent != null && SFXComponent != null)
        {
            foreach (GameObject Sound in MusicComponent)
            {
                StartCoroutine(FadeOut(Sound.GetComponent<AudioSource>(), 0.25f));
            }
    
            foreach (GameObject Sound in SFXComponent)
            {
                StartCoroutine(FadeOut(Sound.GetComponent<AudioSource>(), 0.25f));
            }
        }
    }

    IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = PlayerPrefs.GetFloat("MusicVol");
 
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }

        audioSource.volume = 0;
    }
 
    IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = 0.1f;
 
        audioSource.volume = 0;
 
        while (audioSource.volume < PlayerPrefs.GetFloat("MusicVol"))
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }
 
        audioSource.volume = PlayerPrefs.GetFloat("MusicVol");
    }

    public void EndOfIntro()
    {
        if (!leaving)
        {
            StartCoroutine(LoadNextScene());
            leaving = true;
        }
    }

    IEnumerator LoadNextScene()
    {
        ScreenFader.Play("FadeIn");
        StartFadingOut();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}//EndScript