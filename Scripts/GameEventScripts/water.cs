using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water : MonoBehaviour
{
    PlayerHealth pHealth;

    void Start()
    {
        pHealth = Object.FindObjectOfType<PlayerHealth>();
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.transform.name);

        if (collider.transform.name.Equals("playerBody"))
        {
            GetComponent<Collider>().isTrigger = false;
            
            pHealth.DeathByWater();
        }
    }
}//EndScript