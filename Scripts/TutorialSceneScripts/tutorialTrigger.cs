using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class tutorialTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent gEvent = new UnityEvent();

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.transform.name);

        if (collider.transform.name.Equals("playerBody"))
        {
            gEvent.Invoke();
        }
    }
}//EndScript