using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class speedRunTimer : MonoBehaviour
{
    float currentTime = 0;
    [SerializeField] Text TimeDisplay;
    bool timeStop = false;
    GameDataScript dataScript;

    void Start()
    {
        dataScript = Object.FindObjectOfType<GameDataScript>();
    }

    void Update()
    {
        if (!timeStop)
        {
            currentTime += Time.deltaTime;
    
            float minutes = currentTime/60;
            float seconds = currentTime % 60;
    
            TimeDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

//=========================================================================

    public void EndReached()
    {
        timeStop = true;
        dataScript.EditLevelTimes(currentTime);
    }

    public void StartTutorialSpeedRun()
    {
        timeStop = false;
    }
    
    public void EndTutorialSpeedRun()
    {
        timeStop = true;

        dataScript.EditLevelTimes(currentTime);
    }
    
    public void ResetTime()
    {
        currentTime = 0;
    }

    public float GetTimeStamp()
    {
        return currentTime;
    }
}//EndScript