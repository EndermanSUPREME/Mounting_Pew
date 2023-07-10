using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private float musicVolume, sfxVolume, ResolutionIndex;
    [SerializeField] bool gamePaused = false;
    bool isDead = false;
    private int[] ScreenWidth = {800, 1024, 1152, 1280, 1280, 1440, 1600, 1920, 2560}, ScreenHeight = {600, 768, 864, 720, 1024, 900, 1200, 1080, 1440}; // [0, 8]
    [SerializeField] Slider MusicSlider, SfxSlider, ResolutionSlider;
    GameObject[] MusicComponent, SFXComponent;
    [SerializeField] GameObject HUD, Main, Setting, LevelSelectScreen;
    [SerializeField] Text MusicDisplay, SFX_Display, ResolutionDisplay;
    [SerializeField] Animator ScreenFader;

    [SerializeField] PlayerMovement movementScript;
    [SerializeField] PlayerShooting playerShootScript;
    [SerializeField] PlayerCameraScript camScript;
    [SerializeField] GameDataScript dataScript;

    [SerializeField] GameObject[] levelButtons;
    [SerializeField] Text[] fastTimeDisplay;

    void Awake()
    {
        Application.targetFrameRate = 65;

        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            ResumeGame();
            ApplyPlayerSettings();
        } else
            {
                BackToMainCanvas();
            }
    }

    public void HasDied()
    {
        isDead = true;
    }

    void Start()
    {
        MusicComponent = GameObject.FindGameObjectsWithTag("Music");
        SFXComponent = GameObject.FindGameObjectsWithTag("SFX");

        StartFadingIn();
    }

    public void ConfigForNewLevel()
    {
        Application.targetFrameRate = 65;
        
        if (Setting != null)
        {
            if (SceneManager.GetActiveScene().buildIndex > 0)
            {
                ResumeGame();
            } else
                {
                    BackToMainCanvas();
                }
    
            // Get all objects in the scene that have a script for editing
            MusicComponent = GameObject.FindGameObjectsWithTag("Music");
            SFXComponent = GameObject.FindGameObjectsWithTag("SFX");
    
            ApplyPlayerSettings();
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            if (Input.GetKeyDown(KeyCode. Escape) && !isDead)
            {
                Debug.Log("Escape Pressed");

                if (!gamePaused)
                {
                    GoToSettings();
                    Time.timeScale = 0f;
                } else
                    {
                        ResumeGame();
                    }
            }
        }

        if (Setting.activeSelf && Setting != null)
        {
            ResolutionIndex = ResolutionSlider.value;

            MusicDisplay.text = "Music: " + MusicSlider.value * 10 + " %";
            SFX_Display.text = "SFX: " + SfxSlider.value * 10 + " %";
            ResolutionDisplay.text = ScreenWidth[(int)ResolutionIndex] + "x" + ScreenHeight[(int)ResolutionIndex];

            SetAudioSettings();

            dataScript.SetPlayerSettingsData();
        }
    }

//========================== GAME SETTINGS =====================================
    public void ApplyPlayerSettings()
    {
        // Screen.SetResolution((int) PlayerPrefs.GetFloat("ScreenWidth"), (int) PlayerPrefs.GetFloat("ScreenHeight"), true);

        MusicSlider.value = PlayerPrefs.GetFloat("MusicVol") * 10;
        SfxSlider.value = PlayerPrefs.GetFloat("SFXVol") * 10;
        ResolutionSlider.value = PlayerPrefs.GetFloat("ResSlideVal");
        ResolutionIndex = PlayerPrefs.GetFloat("ResSlideVal");

        SetResolutionSettings();
        SetAudioSettings();

        Debug.Log("[Apply] => " + (int) PlayerPrefs.GetFloat("ScreenWidth") + " : " + (int) PlayerPrefs.GetFloat("ScreenHeight"));
    }

    public void SetResolutionSettings()
    {
        if (!ResolutionDisplay.gameObject.activeSelf) // webgl build doesnt use a resolution slider
        {
            ResolutionIndex = 4; // 1440:900
        }

        Screen.SetResolution(ScreenWidth[(int)ResolutionIndex], ScreenHeight[(int)ResolutionIndex], true);

        PlayerPrefs.SetFloat("ScreenWidth", ScreenWidth[(int)ResolutionIndex]);
        PlayerPrefs.SetFloat("ScreenHeight", ScreenHeight[(int)ResolutionIndex]);
        PlayerPrefs.SetFloat("ResSlideVal", ResolutionSlider.value);
    }

    private void SetAudioSettings()
    {
        MusicComponent = GameObject.FindGameObjectsWithTag("Music");
        SFXComponent = GameObject.FindGameObjectsWithTag("SFX");

        musicVolume = MusicSlider.value / 10;
        sfxVolume = SfxSlider.value / 10;

        PlayerPrefs.SetFloat("MusicVol", musicVolume);
        PlayerPrefs.SetFloat("SFXVol", sfxVolume);

        PlayerPrefs.SetFloat("MusicSlideVal", MusicSlider.value);
        PlayerPrefs.SetFloat("SFXSlideVal", SfxSlider.value);

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

    public void StartFadingIn()
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

    public void StartFadingOut()
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
    
//======================= SCENE CHANGES ==========================

    public void EndLevelFade()
    {
        ScreenFader.Play("FadeIn");
    }

    public void StartTheGame() // transtion
    {
        ScreenFader.Play("FadeIn");
        StartFadingOut();
        Invoke("LoadGameScene", 1.65f);
    }

    void LoadGameScene()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        SceneManager.LoadScene(1);
    }

    public void ReturnToMainMenuScene() // transition
    {
        ResumeGame();

        ScreenFader.Play("FadeIn");

        Invoke("GoToMain", 1f);
    }

    void GoToMain() // Main Menu Scene
    {
        SceneManager.LoadScene(0);
    }

    public void CloseApplication()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) // on main menu scene
        {
            Application.Quit();
        } else
            {
                ResumeGame();
                ReturnToMainMenuScene();
            }
    }

//======================= SCREEN CHANGES ==========================

    public void RetryLevel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(ReloadLevel());
    }

    IEnumerator ReloadLevel()
    {
        ScreenFader.Play("FadeIn");
        StartFadingOut();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToPauseMenu() // InGame Scenes
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        GoToSettings();
    }

    public void GoToSettings()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        HUD.SetActive(false);
        Main.SetActive(false);
        Setting.SetActive(true);

        gamePaused = true;

        if (movementScript != null)
        {
            movementScript.enabled = false;
            playerShootScript.enabled = false;
            camScript.enabled = false;
        }
    }

    public void BackToMainCanvas()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        Main.SetActive(true);
        Setting.SetActive(false);
        
        if (LevelSelectScreen != null)
        {
            LevelSelectScreen.SetActive(false);
        }
    }

    public void ResumeGame() // InGame Scenes
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        HUD.SetActive(true);
        Main.SetActive(false);
        Setting.SetActive(false);

        gamePaused = false;

        if (movementScript != null)
        {
            movementScript.enabled = true;
            playerShootScript.enabled = true;
            camScript.enabled = true;
        }

        Time.timeScale = 1;
    }

    public void GoToLevelSelectionScreen()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        Main.SetActive(false);
        Setting.SetActive(false);
        LevelSelectScreen.SetActive(true);

        for (int i = 0; i < 4; i++)
        {
            float currentTime = dataScript.GetFastTimeArray()[i];

            if (currentTime > 0)
            {
                float minutes = currentTime/60;
                float seconds = currentTime % 60;
                string.Format("{0:00}:{1:00}", minutes, seconds);

                fastTimeDisplay[i].text = string.Format("{0:00}:{1:00}", minutes, seconds);
            } else
                {
                    fastTimeDisplay[i].text = "No Attempt Found!";
                }
        }

        for (int i = 0; i < 4; i++)
        {
            if (i <= (dataScript.GetFurthestLevel() - 2))
            {
                levelButtons[i].SetActive(true);
            } else
                {
                    levelButtons[i].SetActive(false);
                }
        }
    }

//==================================== Level Select ================================================

    public void SelectLevelOne()
    {
        StartCoroutine(loadSelectedLevel(2)); // tutorial
    }

    public void SelectLevelTwo()
    {
        StartCoroutine(loadSelectedLevel(3)); // farm house
    }

    public void SelectLevelThree()
    {
        StartCoroutine(loadSelectedLevel(4)); // ware house
    }

    public void SelectLevelFour()
    {
        StartCoroutine(loadSelectedLevel(5)); // club
    }

    public void EndOfLevelReached()
    {
        StartCoroutine(MoveToNextLevel());
    }

    IEnumerator MoveToNextLevel()
    {
        ScreenFader.Play("FadeIn");
        StartFadingOut();

        yield return new WaitForSeconds(1);

        if ((SceneManager.GetActiveScene().buildIndex + 1) < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        } else
            {
                SceneManager.LoadSceneAsync(0);
            }
    }

    IEnumerator loadSelectedLevel(int level)
    {
        ScreenFader.Play("FadeIn");
        StartFadingOut();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(level);
    }

}//EndScript