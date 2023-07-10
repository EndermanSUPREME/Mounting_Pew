using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteraction : MonoBehaviour
{
    [SerializeField] float interactionRange;
    displayHighLight lastObjectHighlighted;

    void Update()
    {
        SeekInteractable();
    }

    void SeekInteractable()
    {
        RaycastHit checkRay;
        if (Physics.Raycast(transform.position, transform.forward, out checkRay, interactionRange))
        {
            Debug.Log("Ray Hit");

            if (checkRay.transform != null)
            {
                if (checkRay.transform.GetComponent<interactableObject>() != null)
                {
                    // show affordance (object will highlight)
                    if (lastObjectHighlighted == null)
                    {
                        lastObjectHighlighted = checkRay.transform.GetComponent<displayHighLight>();

                        checkRay.transform.GetComponent<displayHighLight>().ShowHighLight(true);
                    } else
                        {
                            lastObjectHighlighted.ShowHighLight(false);
                            lastObjectHighlighted = checkRay.transform.GetComponent<displayHighLight>();

                            checkRay.transform.GetComponent<displayHighLight>().ShowHighLight(true);
                        }

                    if (Input.GetKeyDown(KeyCode. E))
                    {
                        // interact with object
                        checkRay.transform.GetComponent<interactableObject>().InteractWithObject();
                    }
                } else
                    {
                        if (lastObjectHighlighted != null)
                        {
                            lastObjectHighlighted.ShowHighLight(false);
                            lastObjectHighlighted = null;
                        }
                    }
            } else
                {
                    if (lastObjectHighlighted != null)
                    {
                        lastObjectHighlighted.ShowHighLight(false);
                        lastObjectHighlighted = null;
                    }
                }
        } else
            {
                if (lastObjectHighlighted != null)
                {
                    lastObjectHighlighted.ShowHighLight(false);
                    lastObjectHighlighted = null;
                }
            }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * interactionRange);
    }
}//EndScript