using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayHighLight : MonoBehaviour
{
    [SerializeField] GameObject HighLightObject;

    void Start()
    {
        ShowHighLight(false);
    }

    public void ShowHighLight(bool v)
    {
        HighLightObject.SetActive(v);
    }
}//EndScript