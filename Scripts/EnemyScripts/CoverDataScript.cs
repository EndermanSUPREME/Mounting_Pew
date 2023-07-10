using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoverDataScript : MonoBehaviour
{
    [SerializeField] int gridLength, totalNodes;
    [SerializeField] float nodeSize;
    Node[] gridNodes;
    List<Node> validCoverNodes = new List<Node>();

//=========================================================================================================================================================================================
//=========================================================================================================================================================================================
//=========================================================================================================================================================================================

    void BuildCoverGrid()
    {
        totalNodes = Mathf.RoundToInt((gridLength + 1) * (gridLength + 1));

        gridNodes = new Node[totalNodes];

        for (int i = 0; i < totalNodes; i++)
        {
            gridNodes[i] = new Node();
            gridNodes[i].SetEnvMask(GetComponent<EnemyScript>().GetEnvMask());
            gridNodes[i].SetNodeSize(nodeSize);
        }

        constructNodes(totalNodes, totalNodes);
    }

    void constructNodes(int runZ, int runX) // loop through all the nodes and set their x and y positions
    {
        validCoverNodes.Clear();

        for (int i = 0; i < totalNodes; i++)
        {
            for (int x = 0; x < (gridLength + 1); x++)
            {
                for (int z = 0; z < (gridLength + 1); z++)
                {
                    float posX = transform.position.x + ((-gridLength / 2) + (nodeSize * x));
                    gridNodes[i].SetNodeX(posX);

                    gridNodes[i].SetNodeY(transform.position.y + 0.1f);

                    float posZ = transform.position.z + ((gridLength / 2) - (nodeSize * z));
                    gridNodes[i].SetNodeZ(posZ);


                    Vector3 rayDir = (GetComponent<EnemyScript>().GetPlayerHead().position - gridNodes[i].GetNodePosition());

                    // Debug.DrawRay(gridNodes[i].GetNodePosition(), rayDir, Color.magenta);

                    if (Physics.Raycast(gridNodes[i].GetNodePosition(), rayDir, GetComponent<EnemyScript>().GetEnvMask()) && !gridNodes[i].isInvalid())
                    {
                        // mark spots the player cant see
                        validCoverNodes.Add(gridNodes[i]);
                        gridNodes[i].MarkCover();
                    }

                    i++;
                }
            }
        }
    }

//=========================================================================================================================================================================================
//=========================================================================================================================================================================================
//=========================================================================================================================================================================================

    public Vector3 FindOptimalCoverPoint()
    {
        BuildCoverGrid();
        Vector3 bestPoint = Vector3.zero;

        List<float> PathDistList = new List<float>();

        for (int i = 0; i < validCoverNodes.ToArray().Length;)
        {
            GetComponent<NavMeshAgent>().SetDestination(validCoverNodes.ToArray()[i].GetNodePosition());
            
            PathDistList.Add(GetPathRemainingDistance(GetComponent<NavMeshAgent>()));

            i++;
        }

        for (int i = 0; i < validCoverNodes.ToArray().Length; i++)
        {
            if (PathDistList.ToArray()[i] == Array.IndexOf(PathDistList.ToArray(), PathDistList.ToArray().Min()))
            {
                Debug.Log("[+] Setting Cover Destination Here!");
                bestPoint = validCoverNodes.ToArray()[i].GetNodePosition();
            }
        }

        return bestPoint;
    }

    float GetPathRemainingDistance(NavMeshAgent navMeshAgent)
    {
        float distance = 0.0f;
        for (int i = 0; i < navMeshAgent.path.corners.Length - 1; ++i)
        {
            distance += Vector3.Distance(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1]);
        }

        Debug.LogWarning(distance);

        return distance;
    }

//=========================================================================================================================================================================================
//=========================================================================================================================================================================================
//=========================================================================================================================================================================================

    void OnDrawGizmosSelected()
    {
        // Gizmos.DrawWireCube(transform.position, new Vector3(gridLength, 0.1f, gridLength));

        // BuildCoverGrid();

        // for (int i = 0; i < totalNodes; i++)
        // {
        //     Gizmos.DrawCube(gridNodes[i].GetNodePosition(), Vector3.one * (nodeSize - 0.1f));

        //     if (gridNodes[i].isInvalid())
        //     {
        //         Gizmos.color = Color.red;
        //     } else
        //         {
        //             if (gridNodes[i].CheckIfValidCover())
        //             {
        //                 Gizmos.color = Color.green;
        //             } else
        //                 {
        //                     Gizmos.color = Color.white;
        //                 }
        //         }
        // }
    }
}//EndScript

public class Node
{
    Vector3 nodePosition;
    float nodeSize;
    LayerMask envLayer;
    bool validCover = false;

    public void MarkCover()
    {
        validCover = true;
    }

    public bool CheckIfValidCover()
    {
        return validCover;
    }

    public void SetEnvMask(LayerMask m)
    {
        envLayer = m;
    }

    public bool isInvalid()
    {
        // red when true : white when false
        return Physics.CheckSphere(nodePosition, nodeSize - 0.3f, envLayer);
    }

    public void SetNodeSize(float s)
    {
        nodeSize = s;
    }

    public void SetNodeX(float x)
    {
        nodePosition.x = x;
    }

    public void SetNodeY(float y)
    {
        nodePosition.y = y;
    }

    public void SetNodeZ(float z)
    {
        nodePosition.z = z;
    }

    public Vector3 GetNodePosition()
    {
        return nodePosition;
    }
}// EndScript