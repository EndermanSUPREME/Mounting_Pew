using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warehouseDoorControls : MonoBehaviour
{
    [SerializeField] Animator switchAnim;
    bool doorsOpened = false;
    [SerializeField] GameObject[] SealedDoors;

    public void OpenDoors()
    {
        if (!doorsOpened)
        {
            switchAnim.Play("switchOn");

            doorsOpened = true;

            foreach (GameObject door in SealedDoors)
            {
                door.SetActive(false);
            }
        }
    }
}//EndScript