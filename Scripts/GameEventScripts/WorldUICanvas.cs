using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUICanvas : MonoBehaviour
{
    public Transform playerCam;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(playerCam.position.x, playerCam.position.y, playerCam.position.z));
    }
}//EndScript