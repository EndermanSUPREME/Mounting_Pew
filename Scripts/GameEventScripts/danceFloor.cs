using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class danceFloor : MonoBehaviour
{
    [SerializeField] Material[] danceFloorMats;
    [SerializeField] Material whiteMat;

    void Start()
    {
        StartCoroutine(chanceMat(Random.Range(1, 3)));
    }

    IEnumerator chanceMat(int t)
    {
        GetComponent<Renderer>().material = danceFloorMats[Random.Range(0, danceFloorMats.Length)];
        yield return new WaitForSeconds(t);
        StartCoroutine(chanceMat(t));
    }
}//EndScript