using System; // convert.To lib
using System.IO; // File Lib
using System.Security.Cryptography; // encyption lib
using System.Text; // strings lib?

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataScript : MonoBehaviour
{
    /*
        Data Stored:

        1.) Fastest Times through Levels (includes tutorial)
        2.) Levels Either Reached/Completed for Level Select
        3.) Player Settings Data
    */

    string saveFileName = @"C:\mtnPew_Data\pew.txt", saveDir = @"C:\mtnPew_Data\";
    float[] bestLevelTimes = {0, 0, 0, 0};
    float musicVol, sfxVol, resVal;
    int furthestLevel = 0;

//===================================================================================================================

    void Awake()
    {
        CheckForExistingDirectory();
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            Invoke("SaveNewData", 0.075f);
        }
    }

    void CheckForExistingDirectory()
    {
        if (!Directory.Exists(saveDir)) // no save dir on system
        {
            Directory.CreateDirectory(saveDir);
        }

        Invoke("CheckForExistingFile", 0.05f);
    }

    /* File Content Style =>  furtherestLevelReached, music, sfx, resIndex, resSlider */

    void CheckForExistingFile()
    {
        if (File.Exists(saveFileName)) // grab the data if we have a file
        {
            // print("[+] Save File Detected. . .");
            TextReader tr = new StreamReader(saveFileName); // the TextReader reads the txt file and reads the lines to then be converted to game data
            // tr.ReadLine();

            furthestLevel = Convert.ToInt32(Base64Decode(tr.ReadLine()));
            musicVol = (float)Convert.ToDouble(Base64Decode(tr.ReadLine()));
            sfxVol = (float)Convert.ToDouble(Base64Decode(tr.ReadLine()));
            resVal = (float)Convert.ToDouble(Base64Decode(tr.ReadLine()));

            bestLevelTimes[0] = (float)Convert.ToDouble(Base64Decode(tr.ReadLine()));
            bestLevelTimes[1] = (float)Convert.ToDouble(Base64Decode(tr.ReadLine()));
            bestLevelTimes[2] = (float)Convert.ToDouble(Base64Decode(tr.ReadLine()));
            bestLevelTimes[3] = (float)Convert.ToDouble(Base64Decode(tr.ReadLine()));

            PlayerPrefs.SetFloat("MusicVol", musicVol);
            PlayerPrefs.SetFloat("SFXVol", sfxVol);
            PlayerPrefs.SetFloat("ResSlideVal", resVal);

            tr.Close();

            GetComponent<MenuScript>().ApplyPlayerSettings();
        } else // make a file and set the defaults
            {
                TextWriter tw = new StreamWriter(saveFileName); // the TextReader writes data into the save file
                // tw.WriteLine();

                tw.WriteLine(Base64Encode("0")); // furthestLevelValue
                tw.WriteLine(Base64Encode("0.5"));
                tw.WriteLine(Base64Encode("0.5"));
                tw.WriteLine(Base64Encode("5"));

                tw.WriteLine(Base64Encode("0"));
                tw.WriteLine(Base64Encode("0"));
                tw.WriteLine(Base64Encode("0"));
                tw.WriteLine(Base64Encode("0"));

                PlayerPrefs.SetInt("furthestLevelReached", 0);
                PlayerPrefs.SetFloat("MusicVol", 0.5f);
                PlayerPrefs.SetFloat("SFXVol", 0.5f);
                PlayerPrefs.SetFloat("ResSlideVal", 5);

                tw.Close();

                GetComponent<MenuScript>().ApplyPlayerSettings();
            }

        Invoke("EditProgression", 2);
    }

    string Base64Encode(string info)
    {
        var infoBytes = System.Text.Encoding.UTF8.GetBytes(info);
        return System.Convert.ToBase64String(infoBytes);
    }

    string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public void SaveNewData() // runs during each level
    {
        Debug.Log("[+] Saving New Data"); // 5 = NQ

        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        Debug.Log("Checking For New Progression: " + currentLevel + " :: " + furthestLevel);

        if (currentLevel > furthestLevel)
        {
            furthestLevel = currentLevel;
        }

        TextWriter tw = new StreamWriter(saveFileName); // the TextReader writes data into the save file
        // tw.WriteLine();

        tw.WriteLine(Base64Encode(furthestLevel.ToString()));
        tw.WriteLine(Base64Encode(musicVol.ToString()));
        tw.WriteLine(Base64Encode(sfxVol.ToString()));
        tw.WriteLine(Base64Encode(resVal.ToString()));

        tw.WriteLine(Base64Encode(bestLevelTimes[0].ToString()));
        tw.WriteLine(Base64Encode(bestLevelTimes[1].ToString()));
        tw.WriteLine(Base64Encode(bestLevelTimes[2].ToString()));
        tw.WriteLine(Base64Encode(bestLevelTimes[3].ToString()));

        tw.Close();
    }

//===================================================================================================================

    public void SetPlayerSettingsData()
    {
        musicVol = PlayerPrefs.GetFloat("MusicVol");
        sfxVol = PlayerPrefs.GetFloat("SFXVol");
        resVal = PlayerPrefs.GetFloat("ResSlideVal");
    }

    void EditProgression()
    {
        Debug.Log("Checking Progression => " + SceneManager.GetActiveScene().buildIndex + " :: " + furthestLevel);

        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings)
        {
            if (SceneManager.GetActiveScene().buildIndex > furthestLevel)
            {
                furthestLevel = SceneManager.GetActiveScene().buildIndex;
            }
        }
    }

    public int GetFurthestLevel()
    {
        return furthestLevel;
    }

    public void EditLevelTimes(float currentTime)
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SetNewTime(currentTime, 0);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            SetNewTime(currentTime, 1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SetNewTime(currentTime, 2);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            SetNewTime(currentTime, 3);
        }
    }

    void SetNewTime(float currentTime, int timeTableIndex)
    {
        if (bestLevelTimes[timeTableIndex] > 0)
        {
            if (currentTime < bestLevelTimes[timeTableIndex])
            {
                bestLevelTimes[timeTableIndex] = currentTime;
            }
        } else
            {
                bestLevelTimes[timeTableIndex] = currentTime;
            }

        SaveNewData();
    }

    public float[] GetFastTimeArray()
    {
        return bestLevelTimes;
    }

}//EndScript