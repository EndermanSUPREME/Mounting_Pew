using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpeedRunAreaScript : MonoBehaviour
{
    public Animator EndGate, StartGate;
    public TargetHealthScript[] targets;
    speedRunTimer playerTimer;
    public Collider startTrigger, endTrigger;
    int[] targetPopUpSchedule;
    int targetsDown = 0;
    [SerializeField] AudioSource MusicAffordance;

    void Start()
    {
        playerTimer = Object.FindObjectOfType<speedRunTimer>();

        playerTimer.EndTutorialSpeedRun();
        playerTimer.ResetTime();

        EndGate.Play("close");
        StartGate.Play("open");

        startTrigger.enabled = true;
        endTrigger.enabled = false;
    }

    public void StartClock()
    {
        startTrigger.enabled = false;
        endTrigger.enabled = true;
        StartGate.Play("close");
        targetsDown = 0;

        playerTimer.StartTutorialSpeedRun();
        playerTimer.ResetTime();

        StartMusic();
    }

    public void TargetBeenShotDown()
    {
        targetsDown++;

        if (targetsDown >= targets.Length)
        {
            endTrigger.enabled = true;
            EndGate.Play("open");
        }
    }

    public void StopClock() // works for when player reaches the end or backs out
    {
        EndGate.Play("close");
        StartGate.Play("open");
        playerTimer.EndTutorialSpeedRun();

        startTrigger.enabled = true;
        endTrigger.enabled = false;

        StopMusic();
    }

    void OpenEndOfShootingGate() // all targets knocked down
    {
        startTrigger.enabled = true;
        endTrigger.enabled = false;
        EndGate.Play("open");
    }

    public void RandomTargetPropUp()
    {
        targetPopUpSchedule = new int[targets.Length];

        System.Random random = new System.Random();
        targetPopUpSchedule = Enumerable.Range(0, targets.Length).OrderBy(x => random.Next()).ToArray();
    }

    public void StartShootingGallary()
    {
        StartCoroutine(TargetsAppear(targetPopUpSchedule, 0));
    }

    IEnumerator TargetsAppear(int[] intArray, int index)
    {
        targets[intArray[index]].TargetPropUp();
        index++;
        yield return new WaitForSeconds(0.5f);

        if (index < intArray.Length)
        {
            StartCoroutine(TargetsAppear(intArray, index));
        }
    }

    void StartMusic()
    {
        MusicAffordance.Play();

        StartCoroutine(FadeIn(MusicAffordance, 2));
    }

    void StopMusic()
    {
        StartCoroutine(FadeOut(MusicAffordance, 3));
    }

    IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = PlayerPrefs.GetFloat("MusicVol");
 
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }

        audioSource.Stop();
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

}//EndScript