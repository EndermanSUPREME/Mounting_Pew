using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaData : MonoBehaviour
{
    [SerializeField] float X, Z;

    public float GetAreaWidth()
    {
        return X;
    }

    public float GetAreaLength()
    {
        return Z;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(X, 0.1f, Z));
    }
}//EndScript