using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ranWarehouseLocation : MonoBehaviour
{
    [SerializeField] Transform[] possiblePoints;
    [SerializeField] Transform canOfPew;

    void Start()
    {
        canOfPew.position = possiblePoints[Random.Range(0, possiblePoints.Length)].position;
    }
    
}//EndScript