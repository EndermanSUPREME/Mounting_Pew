using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class haybaleRand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localEulerAngles = new Vector3(0, Random.Range(0, 90), 90);
        transform.localScale = Vector3.one * Random.Range(1.75f, 2.5f);
    }
}//EndScript