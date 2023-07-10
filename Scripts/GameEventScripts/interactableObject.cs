using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class interactableObject : MonoBehaviour
{
    [SerializeField] UnityEvent objectGameEvent = new UnityEvent();

    public void InteractWithObject()
    {
        objectGameEvent.Invoke();
    }
}//EndScript