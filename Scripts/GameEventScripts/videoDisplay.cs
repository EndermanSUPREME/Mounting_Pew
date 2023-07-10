using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class videoDisplay : MonoBehaviour
{
    [SerializeField] VideoClip freshStart, inProg;
    
    void Start()
    {
        GameDataScript dataHolder = Object.FindObjectOfType<GameDataScript>();
        VideoPlayer thisVidPlayer = GetComponent<VideoPlayer>();

        Debug.Log(SceneManager.sceneCountInBuildSettings);

        if (dataHolder.GetFurthestLevel() < 2)
        {
            thisVidPlayer.clip = freshStart;
        } else
            {
                if (dataHolder.GetFurthestLevel() >= SceneManager.sceneCountInBuildSettings - 1)
                {
                    thisVidPlayer.clip = freshStart;
                } else
                    {
                        thisVidPlayer.clip = inProg;
                    }
            }
    }
}//EndScript