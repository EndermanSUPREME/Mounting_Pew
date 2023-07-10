using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] MenuScript mainUIScript;
    [SerializeField] speedRunTimer timerScript;
    [SerializeField] PlayerHealth healthScript;
    bool canTouched = false;

    void Awake()
    {
        mainUIScript = Object.FindObjectOfType<MenuScript>();
        timerScript = Object.FindObjectOfType<speedRunTimer>();
        healthScript = Object.FindObjectOfType<PlayerHealth>();
    }

    public void MountingPewCanObtained()
    {
        if (!canTouched)
        {
            timerScript.EndReached();
            healthScript.GoingToNextLevel();
            mainUIScript.EndOfLevelReached();

            canTouched = true;
        }
    }
}//EndScript